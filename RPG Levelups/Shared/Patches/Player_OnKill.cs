using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_Levelups.Patches
{

    [HarmonyPatch(typeof(Player), nameof(Player.OnKill))]
    internal class Player_OnKill
    {
        [HarmonyPrefix]
        internal static bool Prefix(Player __instance)
        {
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(Player __instance)
        {
            StatManager statManager = StatManager.LoadFromFile();
        }
    }
}
