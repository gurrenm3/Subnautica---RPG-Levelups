using HarmonyLib;
using StatsCore.Extensions;
using System;

namespace RPG_Levelups.Patches
{

    [HarmonyPatch(typeof(SaveLoadManager), nameof(SaveLoadManager.InitializeNewGame))]
    internal class SaveLoadManager_InitializeNewGame
    {
        private static Survival survival;

        [HarmonyPrefix]
        internal static bool Prefix(SaveLoadManager __instance)
        {
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(SaveLoadManager __instance)
        {
            StatManager statManager = StatManager.LoadFromFile();
            survival = Player.main.GetSurvival();
            ApplyStatBonuses();
        }

        private static void ApplyStatBonuses()
        {
            // Max Health
            var maxHealth = StatManager.GetStat(StatType.MaxHealth);
            maxHealth.onLevelRaised.Add((stat) => Player.main.liveMixin.RaiseMaxHealth((int)maxHealth.BonusPerLevel));
            Player.main.liveMixin.RaiseMaxHealth((int)maxHealth.GetTotalCurrentBonus());


            // Max Food
            var maxFood = StatManager.GetStat(StatType.MaxFood);
            survival.RaiseStomachSize((int)maxFood.GetTotalCurrentBonus());
            survival.RaiseStomachOverfillSize((int)maxFood.GetTotalCurrentBonus());
            maxFood.onLevelRaised.Add((stat) =>
            {
                survival.RaiseStomachSize((int)maxFood.BonusPerLevel);
                survival.RaiseStomachOverfillSize((int)maxFood.BonusPerLevel);
            });


            // Max Water
            var maxWater = StatManager.GetStat(StatType.MaxWater);
            survival.RaiseWaterCapacity((int)maxWater.GetTotalCurrentBonus());
            survival.RaiseWaterOverfillSize((int)maxWater.GetTotalCurrentBonus());
            maxWater.onLevelRaised.Add((stat) =>
            {
                survival.RaiseWaterCapacity((int)maxWater.BonusPerLevel);
                survival.RaiseWaterOverfillSize((int)maxWater.BonusPerLevel);
            });


            // Max Lungs
            var maxLungs = StatManager.GetStat(StatType.MaxLungs);
            Player.main.GetLungs().oxygenCapacity += (int)maxLungs.GetTotalCurrentBonus();
            maxLungs.onLevelRaised.Add((stat) => Player.main.GetLungs().oxygenCapacity += (int)maxLungs.BonusPerLevel);


            // Suffocation
            var suffocation = StatManager.GetStat(StatType.Suffocation);
            Player.main.RaiseSuffocationTime((float)suffocation.GetTotalCurrentBonus());
            suffocation.onLevelRaised.Add((stat) => Player.main.RaiseSuffocationTime((float)suffocation.BonusPerLevel));


            // Max Depth
            /*var maxDepth = StatManager.GetStat(StatType.MaxDepth);
            Player.main.dep((float)maxDepth.GetTotalCurrentBonus());
            maxDepth.onLevelRaised.Add((stat) => Player.main.RaiseSuffocationTime((float)maxDepth.BonusPerLevel));*/
        }
    }
}
