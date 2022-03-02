using System;
using System.Linq;
using System.Reflection;
using Harmony;
using Il2CppSystem.Collections;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.UI;
using VRC.UI.Elements;
using HarmonyMethod = HarmonyLib.HarmonyMethod;

namespace AvatarHider
{
    public static class NetworkEvents
    {
        public static HarmonyInstance HarmonyInstance { get; set; }
        public static event Action<Player> OnPlayerLeft;
        public static event Action<Player> OnPlayerJoined;
        public static event Action<APIUser> OnFriended;
        public static event Action<string> OnUnfriended;
        public static event Action<VRCAvatarManager, GameObject> OnAvatarChanged;
        public static event Action<string, ApiPlayerModeration.ModerationType> OnPlayerModerationSent;
        public static event Action<string, ApiPlayerModeration.ModerationType> OnPlayerModerationRemoved;
        private static void OnFriend(APIUser __0)
        {
            if (__0 == null) return;

            OnFriended?.DelegateSafeInvoke(__0);
        }
        private static void OnUnfriend(string __0)
        {
            if (string.IsNullOrEmpty(__0)) return;

            OnUnfriended?.DelegateSafeInvoke(__0);
        }
        private static void OnAvatarChange(VRCAvatarManager __instance)
        {
            OnAvatarChanged?.DelegateSafeInvoke(__instance, __instance.prop_GameObject_0);
        }
        private static void OnPlayerModerationSend1(string __1, ApiPlayerModeration.ModerationType __2)
        {
            if (__1 == null) return;

            OnPlayerModerationSent?.DelegateSafeInvoke(__1, __2);
        }
        private static void OnPlayerModerationSend2(string __0, ApiPlayerModeration.ModerationType __1)
        {
            if (__0 == null) return;

            OnPlayerModerationSent?.DelegateSafeInvoke(__0, __1);
        }
        private static void OnPlayerModerationRemove(string __0, ApiPlayerModeration.ModerationType __1)
        {
            if (__0 == null) return;

            OnPlayerModerationRemoved?.DelegateSafeInvoke(__0, __1);
        }

        public static void OnUiManagerInit()
        {
            HarmonyInstance = new HarmonyInstance("AvatarHider");
            var field0 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0;
            var field1 = NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1;

            field0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>((player) => { if (player != null) OnPlayerJoined?.DelegateSafeInvoke(player); }));
            field1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>((player) => { if (player != null) OnPlayerLeft?.DelegateSafeInvoke(player); }));

            HarmonyInstance.Patch(typeof(APIUser).GetMethod("LocalAddFriend"), null, new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnFriend), BindingFlags.NonPublic | BindingFlags.Static)));
            HarmonyInstance.Patch(typeof(APIUser).GetMethod("UnfriendUser"), null, new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnUnfriend), BindingFlags.NonPublic | BindingFlags.Static)));
            HarmonyInstance.Patch(typeof(VRCAvatarManager).GetMethods().First(mb => mb.Name.StartsWith("Method_Private_Boolean_GameObject_String_Single_String_")), null, new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnAvatarChange), BindingFlags.NonPublic | BindingFlags.Static)));
            foreach (MethodInfo method in typeof(ModerationManager).GetMethods().Where(mb => mb.Name.StartsWith("Method_Private_ApiPlayerModeration_String_String_ModerationType_")))
                HarmonyInstance.Patch(method, new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnPlayerModerationSend1), BindingFlags.NonPublic | BindingFlags.Static)));
            foreach (MethodInfo method in typeof(ModerationManager).GetMethods().Where(mb => mb.Name.StartsWith("Method_Private_Void_String_ModerationType_Action_1_ApiPlayerModeration_Action_1_String_")))
                HarmonyInstance.Patch(method, new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnPlayerModerationSend2), BindingFlags.NonPublic | BindingFlags.Static)));
            HarmonyInstance.Patch(typeof(ModerationManager).GetMethod("Method_Private_Void_String_ModerationType_0"), new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnPlayerModerationRemove), BindingFlags.NonPublic | BindingFlags.Static)));
            

            HarmonyInstance.Patch(typeof(FriendsListManager).GetMethod("Method_Private_Void_String_0"), new HarmonyMethod(typeof(NetworkEvents).GetMethod(nameof(OnUnfriend), BindingFlags.NonPublic | BindingFlags.Static)));
            

            
        }
        public static void DelegateSafeInvoke(this Delegate @delegate, params object[] args)
        {
            if (@delegate == null)
                return;

            foreach (Delegate @delegates in @delegate.GetInvocationList())
            {
                try
                {
                    @delegates.DynamicInvoke(args);
                }
                catch (Exception ex)
                {
                    AvatarHider.AvatarHiderMod.Instance.LoggerInstance.Error("Error while invoking delegate:\n" + ex.ToString());
                }
            }
        }
    }
}
