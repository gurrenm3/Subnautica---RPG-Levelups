using RPG_Framework;
using RPG_Framework.Extensions;
using StatsCore.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace RPG_Levelups
{
    public class StatManager
    {
        [NonSerialized]
        public static List<Action<StatManager>> onStatMgrLoaded = new List<Action<StatManager>>();
        public static StatManager Instance { get; private set; }
        private const string fileName = "RPG Leveling.json";
        public List<ModStat> AllStats { get; set; } = new List<ModStat>();

        public StatManager()
        {
            Instance = this;
        }

        private void CreateAllStats()
        {
            var maxHealth = CreateNewStat(StatType.MaxHealth.ToString(), 30, 100, 1.5);
            maxHealth.BonusPerLevel = 5;
            maxHealth.DisplayName = "Max Health";

            var maxFood = CreateNewStat(StatType.MaxFood.ToString(), 50, 150, 1.5);
            maxFood.BonusPerLevel = 5;
            maxFood.DisplayName = "Food Capacity";

            var maxWater = CreateNewStat(StatType.MaxWater.ToString(), 50, 150, 1.5);
            maxWater.BonusPerLevel = 5;
            maxWater.DisplayName = "Water Capacity";

            var maxLungs = CreateNewStat(StatType.MaxLungs.ToString(), 50, 150, 1.5);
            maxLungs.BonusPerLevel = 5;
            maxLungs.DisplayName = "Lung Capacity";

            var suffocation = CreateNewStat(StatType.Suffocation.ToString(), 30, 15, 1.25);
            suffocation.BonusPerLevel = 0.5;
            suffocation.DisplayName += " Time";

            var maxDepth = CreateNewStat(StatType.MaxDepth.ToString(), 500, 350, 1.5);
            maxDepth.BonusPerLevel = 5;
            maxDepth.DisplayName = "Max Depth";

            var walkSpeed = CreateNewStat(StatType.WalkSpeed.ToString(), 20, 350, 1.5);
            walkSpeed.BonusPerLevel = 0.25;
            walkSpeed.DisplayName = "Walk Speed";

            var swimSpeed = CreateNewStat(StatType.SwimSpeed.ToString(), 30, 350, 1.5);
            swimSpeed.BonusPerLevel = 0.25;
            swimSpeed.DisplayName = "Swim Speed";

            foreach (DamageType damageType in Enum.GetValues(typeof(DamageType)))
            {
                var resistance = CreateNewStat(damageType.ToString(), 40, 50, 1.5);
                resistance.BonusPerLevel = 2.5;
                resistance.DisplayName += " Resistance";
            }
        }

        public bool Save()
        {
            string json = JsonUtil.Serialize(this);
            string savePath = Path.Combine(SaveLoadManager.GetTemporarySavePath(), fileName);
            File.WriteAllBytes(savePath, Encoding.Default.GetBytes(json));
            //File.WriteAllText(savePath, json);

            var bytes = File.ReadAllBytes(savePath);
            string text = Encoding.UTF8.GetString(bytes);
            return json == text;
        }

        public static void AddExp(StatType statType, double amount) => GetStat(statType)?.RaiseExp(amount);
        public static void AddExp(DamageType damageType, double amount) => GetStat(damageType)?.RaiseExp(amount);

        public static ModStat GetStat(StatType statType) => Instance.AllStats.FirstOrDefault(stat => stat.Name == statType.ToString());
        public static ModStat GetStat(DamageType damageType) => Instance.AllStats.FirstOrDefault(stat => stat.Name == damageType.ToString());

        public static ModStat CreateNewStat(string name, int maxLevel, double baseExp, double expMultiplier)
        {
            var rpgStat = RPGStatFactory.Create(name, maxLevel, baseExp, expMultiplier);
            var stat = ModStat.FromRPGStat(rpgStat);
            Instance.AllStats.Add(stat);
            return stat;
        }

        public static StatManager LoadFromFile()
        {
            StatManager statManager = null;
            string savePath = Path.Combine(SaveLoadManager.GetTemporarySavePath(), fileName);
            if (!File.Exists(savePath))
            {
                statManager = new StatManager();
                statManager.CreateAllStats();
                statManager.Save();
            }
            else
            {
                string json = Encoding.UTF8.GetString(File.ReadAllBytes(savePath));
                statManager = JsonUtil.Deserialize<StatManager>(json);
            }

            statManager.AllStats.ForEach(stat =>
            {
                InitializeStat(stat);
                stat.lastSavedLevel = stat.CurrentLevel;
            });

            onStatMgrLoaded?.InvokeAll(statManager);
            return statManager;
        }

        private static void InitializeStat(RPGStat stat)
        {
            stat.onLevelRaised.Add((s) =>
            {
                var modStat = (ModStat)s;
                ErrorMessage.AddMessage($"{modStat.DisplayName} has just leveled up. Current level: {modStat.CurrentLevel}");
            });
        }
    }
}
