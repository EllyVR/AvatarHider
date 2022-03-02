using System.Collections;
using System.Diagnostics;
using System.Linq;
using AvatarHider.DataTypes;
using MelonLoader;
using VRC;
using VRC.UI.Core;

//Refrencing that its not the official one. 
[assembly: MelonInfo(typeof(AvatarHider.AvatarHiderMod), "AvatarHider (Modified)", "1.3.2", "Sleepers (Modded By Elly)", "https://github.com/SleepyVRC/Mods")]
[assembly: MelonGame("VRChat", "VRChat")]
[assembly: MelonOptionalDependencies("UIExpansionKit")]

namespace AvatarHider
{
    public class AvatarHiderMod : MelonMod
    {
        public static AvatarHiderMod Instance { get; private set; }

        public static readonly Stopwatch timer = Stopwatch.StartNew();

        public override void OnApplicationStart()
        {
            Instance = this;
            AvatarHider.DataTypes.AvatarHiderPlayer._reloadAllAvatarsMethod = typeof(VRCPlayer).GetMethods().First(mi => mi.Name.StartsWith("Method_Public_Void_Boolean_") && mi.Name.Length < 30 && mi.GetParameters().All(pi => pi.IsOptional) && XrefUtils.CheckUsedBy(mi, "Method_Public_Void_", typeof(FeaturePermissionManager)));
            Config.RegisterSettings();
            OnPreferencesSaved();
            AvatarHiderPlayer.Init();
            PlayerManager.Init();
            RefreshManager.Init();
            Config.OnConfigChange();
            MelonCoroutines.Start(WaitForUI());
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            if (buildIndex != -1) return;

            PlayerManager.OnSceneWasLoaded();

        }
        public override void OnUpdate()
        {
            // About 50-100 microseconds (0.05 - 0.1 milliseconds) per refresh in instance of ~20 people;
            RefreshManager.Refresh();
        }
        public IEnumerator WaitForUI()
        {
            while (UIManager.prop_UIManager_0 == null) yield return null;

            NetworkEvents.OnUiManagerInit();
        }
    }
}
