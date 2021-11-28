using RPG_Framework;

namespace RPG_Levelups
{
    public class ModStat : RPGStat
    {
        public double BonusPerLevel { get; set; }

        public ModStat() : base()
        {

        }

        public ModStat(string name, int maxLevel) : base(name, maxLevel)
        {
            
        }

        public ModStat(string name, int maxLevel, double baseExp, double expMultiplier) : base(name, maxLevel, baseExp, expMultiplier)
        {

        }

        public double GetTotalCurrentBonus()
        {
            return CurrentLevel * BonusPerLevel;
        }

        public static ModStat FromRPGStat(RPGStat rpgStat)
        {
            var stat = new ModStat(rpgStat.Name, rpgStat.MaxLevel);
            stat.ExpTable = rpgStat.ExpTable;
            stat.RaiseLevel(rpgStat.CurrentLevel);
            stat.RaiseExp(rpgStat.CurrentLevel);
            stat.onLevelRaised = rpgStat.onLevelRaised;
            stat.OnLevelReduced = rpgStat.OnLevelReduced;
            return stat;
        }
    }
}