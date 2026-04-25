using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace HeroEngine.Core.Data
{
    public class ConfigManager
    {
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\HeroEngine.Web\Data\Files\game_config.xml");

        public GameConfig LoadConfig()
        {
            if (!File.Exists(_path)) return CreateDefault();

            XmlSerializer serializer = new XmlSerializer(typeof(GameConfig));
            using (FileStream fs = new FileStream(_path, FileMode.Open))
            {
                return (GameConfig)serializer.Deserialize(fs);
            }
        }

        public void SaveConfig(GameConfig config)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameConfig));
            using (FileStream fs = new FileStream(_path, FileMode.Create))
            {
                serializer.Serialize(fs, config);
            }
        }

        private GameConfig CreateDefault()
        {
            return new GameConfig { LevelMultiplier = 1.15, CriticalHitChance = 0.20, MaxCombatRounds = 20, MaxHeroesPerBattle = 4 };
        }
    }
}
