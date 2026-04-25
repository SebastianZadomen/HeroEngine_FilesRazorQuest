using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HeroEngine.Core.Data
{
    [XmlRoot("GameConfig")]
    public class GameConfig
    {
        public double LevelMultiplier { get; set; }
        public double CriticalHitChance { get; set; }
        public int MaxCombatRounds { get; set; }
        public int MaxHeroesPerBattle { get; set; }
    }
}
