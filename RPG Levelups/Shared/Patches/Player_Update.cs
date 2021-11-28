using HarmonyLib;
using UnityEngine;
using StatsCore.Extensions;

namespace RPG_Levelups.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.Update))]
    internal class Player_Update
    {
        [HarmonyPrefix]
        internal static bool Prefix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return true;

            return true;
        }

        [HarmonyPostfix]
        internal static void PostFix(Player __instance)
        {
            if (Guard.IsGamePaused())
                return;


            // add EXP for max health
            if (__instance.liveMixin.IsFullHealth())
            {
                double amountPerSecond = 0.1;
                var amountToAdd = Time.deltaTime * amountPerSecond;
                StatManager.AddExp(StatType.MaxHealth, amountToAdd);
            }


            // add EXP for suffocation time
            if (__instance.IsSuffocating())
            {
                ErrorMessage.AddMessage("Suffocating");
                double amountPerSecond = 1;
                var amountToAdd = Time.deltaTime * amountPerSecond;
                StatManager.AddExp(StatType.Suffocation, amountToAdd);
            }


            // add EXP for swim speed
            if (__instance.IsSwimmingInOcean())
            {
                double amountPerSecond = 1;
                var amountToAdd = Time.deltaTime * amountPerSecond;
                StatManager.AddExp(StatType.SwimSpeed, amountToAdd);
            }


            // add EXP for walk speed
            bool isOnGround = __instance.motorMode == Player.MotorMode.Run || __instance.motorMode == Player.MotorMode.Walk;
            if (isOnGround)
            {
                double amountPerSecond = 1;
                var amountToAdd = Time.deltaTime * amountPerSecond;
                StatManager.AddExp(StatType.WalkSpeed, amountToAdd);
            }


            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                var health = StatManager.GetStat(StatType.MaxHealth);

                ErrorMessage.AddMessage("Adding EXP");
                StatManager.AddExp(StatType.MaxHealth, 1000);
                ErrorMessage.AddMessage("Current level: " + health.CurrentLevel);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Player.main.liveMixin.health -= 10;
            }
        }
    }
}