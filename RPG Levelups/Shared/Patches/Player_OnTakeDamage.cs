using HarmonyLib;

namespace RPG_Levelups.Patches
{
    [HarmonyPatch(typeof(Player), nameof(Player.OnTakeDamage))]
    internal class Player_OnTakeDamage
    {
        [HarmonyPrefix]
        internal static bool Prefix(Player __instance, DamageInfo damageInfo)
        {
            var resistance = StatManager.GetStat(damageInfo.type);
            resistance.RaiseExp(damageInfo.damage);

            double percentToIgnore = resistance.GetTotalCurrentBonus() / damageInfo.damage;
            double amountToIgnore = damageInfo.damage * percentToIgnore;
            __instance.liveMixin.AddHealth((float)amountToIgnore);

            damageInfo.damage -= (float)amountToIgnore;
            return true;
        }

        [HarmonyPostfix]
        internal static void Postfix(Player __instance, DamageInfo damageInfo)
        {
            
        }
    }
}
