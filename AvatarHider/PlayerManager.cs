﻿using System.Collections.Generic;
using System.Linq;
using AvatarHider.DataTypes;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;

namespace AvatarHider
{
    public static class PlayerManager
    {
        public static Dictionary<int, AvatarHiderPlayer> players = new Dictionary<int, AvatarHiderPlayer>();
        public static Dictionary<int, AvatarHiderPlayer> filteredPlayers = new Dictionary<int, AvatarHiderPlayer>();

        public static void Init()
        {
            NetworkEvents.OnPlayerJoined += OnPlayerJoin;
            NetworkEvents.OnPlayerLeft += OnPlayerLeave;
            NetworkEvents.OnFriended += OnFriend;
            NetworkEvents.OnUnfriended += OnUnfriend;
            NetworkEvents.OnAvatarChanged += OnAvatarChanged;
            NetworkEvents.OnPlayerModerationSent += OnPlayerModerationSent;
            NetworkEvents.OnPlayerModerationRemoved += OnPlayerModerationRemoved;
        }
        public static void OnSceneWasLoaded()
        {
            players.Clear();
            filteredPlayers.Clear();
        }

        private static void OnAvatarChanged(VRCAvatarManager manager, GameObject gameObject)
        {
            int photonId = manager.field_Private_VRCPlayer_0.prop_PlayerNet_0.prop_PhotonView_0.field_Private_Int32_0;

            if (!players.ContainsKey(photonId)) return;

            players[photonId].SetAvatar(gameObject);
            if (filteredPlayers.ContainsKey(photonId))
                RefreshManager.RefreshPlayer(players[photonId], Player.prop_Player_0.transform.position);
            else
                if (Config.IncludeHiddenAvatars.Value && players[photonId].isHidden)
                    players[photonId].SetInActive();
        }

        private static void OnPlayerJoin(Player player)
        {
            if (player == null || player.prop_APIUser_0 == null) //player.prop_APIUser_0.id == APIUser.CurrentUser.id) // The apiuser in player will only be null on the first join of the first instance of the client, and only occasionally. So it can be garunteed to be local player
                return;

            int photonId = player.prop_VRCPlayer_0.prop_PlayerNet_0.prop_PhotonView_0.field_Private_Int32_0;
            
            if (players.ContainsKey(photonId)) 
                return;

            AvatarHiderPlayer playerProp = new AvatarHiderPlayer()
            {
                active = true,
                photonId = photonId,
                userId = player.prop_APIUser_0.id,
                player = player,
                avatar = player.prop_VRCPlayer_0.prop_VRCAvatarManager_0.prop_GameObject_0,
                isFriend = APIUser.IsFriendsWith(player.prop_APIUser_0.id),
                isShown = IsAvatarExplcitlyShown(player.prop_APIUser_0),
                isHidden = IsAvatarExplcitlyHidden(player.prop_APIUser_0)
            };

            players.Add(playerProp.photonId, playerProp);
            HideOrShowAvatar(playerProp);
            RefreshFilteredList();
        }
        private static void OnPlayerLeave(Player player)
        {
            if (player.prop_APIUser_0 == null)
                return;

            if (TryGetPlayerFromId(player.prop_APIUser_0.id, out AvatarHiderPlayer avatarHiderPlayer))
            {
                players.Remove(avatarHiderPlayer.photonId);
                filteredPlayers.Remove(avatarHiderPlayer.photonId);
            }
        }

        private static void OnFriend(APIUser apiUser)
        {
            foreach (AvatarHiderPlayer playerProp in players.Values)
            {
                if (playerProp.userId == apiUser.id)
                {
                    playerProp.isFriend = true;
                    HideOrShowAvatar(playerProp);
                    RefreshFilteredList();
                }
            }
        }
        private static void OnUnfriend(string userId)
        {
            foreach (AvatarHiderPlayer playerProp in players.Values)
            {
                if (playerProp.userId == userId)
                {
                    playerProp.isFriend = false;
                    RefreshFilteredList();
                }
            }
        }
        private static void OnPlayerModerationSent(string userId, ApiPlayerModeration.ModerationType moderationType)
        {
            if (moderationType == ApiPlayerModeration.ModerationType.ShowAvatar)
            {
                foreach (AvatarHiderPlayer playerProp in players.Values)
                {
                    if (playerProp.userId == userId)
                    {
                        playerProp.isShown = true;
                        HideOrShowAvatar(playerProp);
                        RefreshFilteredList();
                    }
                }
            }
            else if (moderationType == ApiPlayerModeration.ModerationType.HideAvatar)
            {
                foreach (AvatarHiderPlayer playerProp in players.Values)
                {
                    if (playerProp.userId == userId)
                    {
                        playerProp.isHidden = true;
                        HideOrShowAvatar(playerProp);
                        RefreshFilteredList();
                    }
                }
            }
        }
        private static void OnPlayerModerationRemoved(string userId, ApiPlayerModeration.ModerationType moderationType)
        {
            if (moderationType == ApiPlayerModeration.ModerationType.ShowAvatar)
            {
                foreach (AvatarHiderPlayer playerProp in players.Values)
                {
                    if (playerProp.userId == userId)
                    {
                        playerProp.isShown = false;
                        RefreshFilteredList();
                    }
                }
            }
            else if (moderationType == ApiPlayerModeration.ModerationType.HideAvatar)
            {
                foreach (AvatarHiderPlayer playerProp in players.Values)
                {
                    if (playerProp.userId == userId)
                    {
                        playerProp.isHidden = false;
                        RefreshFilteredList();
                    }
                }
            }
        }

        public static List<AvatarHiderPlayer> RefreshFilteredList()
        {
            ExcludeFlags excludeFlags = ExcludeFlags.None;
            if (Config.IgnoreFriends.Value)
                excludeFlags |= ExcludeFlags.Friends;
            if (Config.ExcludeShownAvatars.Value)
                excludeFlags |= ExcludeFlags.Shown;
            if (Config.IncludeHiddenAvatars.Value)
                excludeFlags |= ExcludeFlags.Hidden;

            List<AvatarHiderPlayer> removedPlayers = new List<AvatarHiderPlayer>();
            filteredPlayers = players.ToDictionary(entry => entry.Key, entry => entry.Value);
            foreach (KeyValuePair<int, AvatarHiderPlayer> item in players)
            {
                if (((excludeFlags.HasFlag(ExcludeFlags.Friends) && item.Value.isFriend) ||
                     (excludeFlags.HasFlag(ExcludeFlags.Shown) && item.Value.isShown)) &&
                     !(excludeFlags.HasFlag(ExcludeFlags.Hidden) && item.Value.isHidden))
                { 
                    filteredPlayers.Remove(item.Key);
                    removedPlayers.Add(item.Value);
                }
            }
            return removedPlayers;
        }

        public static void HideOrShowAvatar(AvatarHiderPlayer avatarHiderPlayer)
        {
            //if (avatarHiderPlayer.userId == APIUser.CurrentUser.id)
            //    return;
            if (Config.IncludeHiddenAvatars.Value && avatarHiderPlayer.isHidden)
                avatarHiderPlayer.SetInActive();
            else if ((Config.IgnoreFriends.Value && avatarHiderPlayer.isFriend) ||
                (Config.ExcludeShownAvatars.Value && avatarHiderPlayer.isShown))
                avatarHiderPlayer.SetActive();
        }

        public static bool TryGetPlayerFromId(string userId, out AvatarHiderPlayer avatarHiderPlayer)
        {
            foreach (AvatarHiderPlayer loopedAvatarHiderPlayer in players.Values)
            {
                if (loopedAvatarHiderPlayer.userId == userId)
                {
                    avatarHiderPlayer = loopedAvatarHiderPlayer;
                    return true;
                }
            }

            avatarHiderPlayer = null;
            return false;
        }
        
        public static bool IsAvatarExplcitlyShown(APIUser user)
        {
            if (ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(user.id))
                foreach (ApiPlayerModeration moderation in ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[user.id])
                    if (moderation.moderationType == ApiPlayerModeration.ModerationType.ShowAvatar)
                        return true;

            return false;
        }
        public static bool IsAvatarExplcitlyHidden(APIUser user)
        {
            if (ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0.ContainsKey(user.id))
                foreach (ApiPlayerModeration moderation in ModerationManager.prop_ModerationManager_0.field_Private_Dictionary_2_String_List_1_ApiPlayerModeration_0[user.id])
                    if (moderation.moderationType == ApiPlayerModeration.ModerationType.HideAvatar)
                        return true;

            return false;
        }
    }
}
