using HarmonyLib;
using StatsCore.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG_Levelups.Patches
{

    [HarmonyPatch(typeof(Player), nameof(Player.OnKill))]
    internal class Player_OnKill
    {
        static Survival survival;

        [HarmonyPrefix]
        internal static bool Prefix(Player __instance)
        {
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(Player __instance)
        {
            survival = __instance.GetSurvival();
            ResetUnsavedStats();
            StatManager statManager = StatManager.LoadFromFile();
        }

        private static void ResetUnsavedStats()
        {
            // Max Health
            Player.main.liveMixin.ReduceMaxHealth((int)StatManager.GetStat(StatType.MaxHealth).GetUnsavedBonus());


            // Max Food
            survival.ReduceStomachSize((int)StatManager.GetStat(StatType.MaxFood).GetUnsavedBonus());
            survival.ReduceStomachOverfillSize((int)StatManager.GetStat(StatType.MaxFood).GetUnsavedBonus());


            // Max Water
            survival.ReduceWaterCapacity((int)StatManager.GetStat(StatType.MaxWater).GetUnsavedBonus());
            survival.ReduceWaterOverfillSize((int)StatManager.GetStat(StatType.MaxWater).GetUnsavedBonus());


            // Max Lungs
            Player.main.GetLungs().oxygenCapacity -= (int)StatManager.GetStat(StatType.MaxLungs).GetUnsavedBonus();


            // Suffocation
            Player.main.ReduceSuffocationTime((float)StatManager.GetStat(StatType.Suffocation).GetUnsavedBonus());
        }
    }
}