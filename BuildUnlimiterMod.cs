using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

[assembly: AssemblyVersion("0.0.0.1")]
[assembly: AssemblyInformationalVersion("0.0.0.1")]

namespace BuildUnlimiter
{
    [BepInPlugin("BuildUnlimiter", "BuildUnlimiter", "0.0.0.1")]
    public class BuildUnlimiterMod : BaseUnityPlugin
    {
        private static bool isBuildLimitSet = false;

        void Awake()
        {
            Debug.Log("BuildUnlimiter mod loaded!");
            new Harmony("BuildUnlimiter").PatchAll();
        }

        public static bool InTreehouse()
        {
            return SceneManager.GetActiveScene().name == "TreeHouseLobby";
        }

        [HarmonyPatch(typeof(Character), nameof(Character.Update))]
        static class CharacterUpdatePatch
        {
            static void Prefix(Character __instance)
            {
                if (Input.GetKeyDown(KeyCode.F4))
                {
                    if (!InTreehouse())
                    {
                        UserMessageManager.Instance.UserMessage("Treehouse only!");
                        Debug.Log("Attempted to enable build limit outside Treehouse");
                        return;
                    }

                    if (!isBuildLimitSet)
                    {
                        GameSettings instance = GameSettings.GetInstance();
                        instance.LevelFullnessScoreLimit = 1000000;

                        UserMessageManager.Instance.UserMessage("Build Limit Increased");
                        Debug.Log("Build limit set to 1,000,000.");
                        isBuildLimitSet = true;
                    }
                }
            }
        }
    }
}