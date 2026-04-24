using HeroEngine.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroEngine.Core.Data
{
    public class CsvStatsWriter
    {

        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\HeroEngine.Web\Data\Files\combat_stats.csv");
        public void AppendCombatStats(CombatResult result)
        {
            try
            {
                string directory = Path.GetDirectoryName(_path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (!File.Exists(_path))
                {
                    string header = "Fecha,Heroes,Enemigos,Resultado,Rondas,DanyoTotal,MVP" + Environment.NewLine;
                    File.WriteAllText(_path, header, Encoding.UTF8);
                }

                string heroes = string.Join(" - ", result.HeroesNames.Where(h => !string.IsNullOrWhiteSpace(h)));
                string enemies = string.Join(" - ", result.EnemiesNames.Where(e => !string.IsNullOrWhiteSpace(e)));

                string line = $"{DateTime.Now:dd/MM/yyyy HH:mm}," +
                              $"{heroes}," +
                              $"{enemies}," +
                              $"{result.Result}," +
                              $"{result.TotalRounds}," +
                              $"{result.TotalDamage}," +
                              $"{result.MVP}" + Environment.NewLine;

                File.AppendAllText(_path, line, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al escribir en el CSV: " + ex.Message);
            }
        }
    }
}
