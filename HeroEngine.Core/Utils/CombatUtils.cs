using HeroEngine.Data;
using HeroEngine.Model.Enemy;
using HeroEngine.Model.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroEngine.Core.Data;

namespace HeroEngine.Utils
{
    public static class CombatUtils
    {
        
        public static int TotalDamage = 0;

        public static string[] heroNames = new string[10];
        private static int[] heroDamages = new int[10];
        public static int heroCount = 0;

   
        private static string fastestDefeatedEnemy = "Ninguno";
        private static int minimumRoundsToDefeat = 9999; 

        public static void RegisterDamage(string heroName, int damage)
        {
            TotalDamage += damage;

            for (int i = 0; i < heroCount; i++)
            {
                if (heroNames[i] == heroName)
                {
                    heroDamages[i] += damage;
                    return; 
                }
            }

            if (heroCount < heroNames.Length)
            {
                heroNames[heroCount] = heroName;
                heroDamages[heroCount] = damage;
                heroCount++;
            }
        }

        public static void RegisterEnemyDefeat(string enemyName, int currentRound)
        {
            if (currentRound < minimumRoundsToDefeat)
            {
                minimumRoundsToDefeat = currentRound;
                fastestDefeatedEnemy = enemyName;
            }
        }


        public static void ShowCombatStats(CombatLog logger, string path, Enemy[] enemies, int rounds)
        {
            string textDamage = $"Daño total infligido: {TotalDamage}";
            Console.WriteLine(textDamage);
            logger.LogMessageOnlyText(textDamage);

            string mvpName = "N/A";
            int maxDamage = 0;

            for (int i = 0; i < heroCount; i++)
            {
                if (heroDamages[i] > maxDamage)
                {
                    maxDamage = heroDamages[i];
                    mvpName = heroNames[i];
                }
            }

            if (maxDamage > 0)
            {
                string textMvp = $"Heroe con mas daño: {mvpName} ({maxDamage} pts de daño).";
                Console.WriteLine(textMvp);
                logger.LogMessageOnlyText(textMvp);
            }

            if (minimumRoundsToDefeat != 9999)
            {
                string textEnemy = $"Enemigo derrotado: {fastestDefeatedEnemy} (en {minimumRoundsToDefeat} rondas de {rounds}).";
                Console.WriteLine(textEnemy);
                logger.LogMessageOnlyText(textEnemy);
            }

            var resultado = new CombatResult()
            {
                HeroesNames = heroNames.ToList(),
                EnemiesNames = enemies.Where(e => e != null).Select(e => e.Name).ToList(),
                Result = enemies.All(e => e == null || !e.IsAlive) ? "Victoria" : "Derrota",

                TotalRounds = rounds,
                TotalDamage = TotalDamage,
                MVP = mvpName
            };

            var csvWriter = new CsvStatsWriter();
            csvWriter.AppendCombatStats(resultado);

            logger.SaveToFile(path);

        }

        public static void ResetStats()
        {
            TotalDamage = 0;
            heroCount = 0;
            fastestDefeatedEnemy = "Ninguno";
            minimumRoundsToDefeat = 9999;

            for (int i = 0; i < heroNames.Length; i++)
            {
                heroNames[i] = null;
                heroDamages[i] = 0;
            }
        }
        public static string GetParticipants()
        {
            string names = "";
            for (int i = 0; i < heroCount; i++)
            {
                names += heroNames[i] + (i < heroCount - 1 ? ", " : "");
            }
            return names;
        }
        public static void SaveParticipants(Hero[] team, int count)
        {
            heroCount = 0;

            for (int i = 0; i < count; i++)
            {
                if (team[i] != null)
                {
                    heroNames[heroCount] = team[i].Name;
                    heroDamages[heroCount] = 0;
                    heroCount++;
                }
            }
        }
    }
}
