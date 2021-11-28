using HarmonyLib;
using System;

namespace RPG_Levelups.Patches
{
    [HarmonyPatch(typeof(SaveLoadManager), nameof(SaveLoadManager.SaveToDeepStorageAsync))]
    [HarmonyPatch(new Type[] { typeof(IOut<SaveLoadManager.SaveResult>) })]
    internal class SaveLoadManager_SaveToDeepStorageAsync
    {
        [HarmonyPrefix]
        internal static bool Prefix(SaveLoadManager __instance)
        {
            bool success = StatManager.Instance.Save();
            string message = success ? "RPG Data saved" : "Failed to save RPG Data";
            ErrorMessage.AddMessage(message);
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(SaveLoadManager __instance)
        {
            
        }
    }
}
