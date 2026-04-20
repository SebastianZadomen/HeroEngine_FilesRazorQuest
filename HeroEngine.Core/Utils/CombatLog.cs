using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroEngine.Utils
{
    public class CombatLog
    {
        private StringBuilder _logContent;

        public CombatLog()
        {
            _logContent = new StringBuilder();
        }

        public void LogRoundStart(int round)
        {
          
            string header = $@"
====================================================================================================

        BATTLE LOG - Round {round}

====================================================================================================";

            _logContent.AppendLine(header);
            Console.WriteLine(header);
        }

        public void LogAction(string type, string attacker, string skill, string target, int damage, bool isDefeated = false)
        {
            string defeatText = isDefeated ? " | DEFEATED!" : "";

            string logLine = $"  [{type}]   {attacker}  > {skill}  > {target}   -> {damage} dmg{defeatText}";

            _logContent.AppendLine(logLine);
            Console.WriteLine(logLine);
        }

        public void LogRoundEnd(int remainingEnemies, int heroesStanding)
        {
            string countEnemiesHeroes = $@"
====================================================================================================

    Remaining enemies: {remainingEnemies}       |           Heroes standing: {heroesStanding}

====================================================================================================
";

            _logContent.AppendLine(countEnemiesHeroes);
            Console.WriteLine(countEnemiesHeroes);
        }

    
        public void LogMessage(string message)
        {
            _logContent.AppendLine(message);
            Console.WriteLine(message);
        }
        public void SaveToFile(string filename)
        {
            string directory = Path.GetDirectoryName(filename);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filename, _logContent.ToString());
        }
    }
}
