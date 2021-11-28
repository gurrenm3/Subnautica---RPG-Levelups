using HarmonyLib;
using StatsCore.Extensions;

namespace RPG_Levelups.Patches
{

    [HarmonyPatch(typeof(Player), nameof(Player.Start))]
    internal class Player_Start
    {
        [HarmonyPrefix]
        internal static bool Prefix(Player __instance)
        {
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(Player __instance)
        {
            
        }
    }
}
