using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Utils
{
    public static class CombatUtils
    {
        
        public static int TotalDamage = 0;

        private static string[] heroNames = new string[10];
        private static int[] heroDamages = new int[10];
        private static int heroCount = 0;

   
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


        public static void ShowCombatStats()
        {
            Console.WriteLine("=== ESTADÍSTICAS FINALES DEL COMBATE ===");
            Console.WriteLine($"Daño total infligido: {TotalDamage}");

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
                Console.WriteLine($"Heroe mas efectivo: {mvpName} ({maxDamage} pts de daño).");
            }

            if (minimumRoundsToDefeat != 9999)
            {
                Console.WriteLine($"Enemigo derrotados: {fastestDefeatedEnemy} (en {minimumRoundsToDefeat} rondas).");
            }

            Console.WriteLine("========================================");
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
    }
}
