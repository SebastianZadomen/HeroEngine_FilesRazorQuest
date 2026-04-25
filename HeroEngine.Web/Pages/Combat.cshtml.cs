using HeroEngine.Model.Heroes;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data;
using System.IO;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class CombatModel : PageModel
{
    public List<string> LogLine { get; set; } = new List<string>();
    public List<string> FinalStats { get; set; } = new List<string>();

    public void OnGet()
    {
        string path = @"../../../../HeroEngine.Core/Files/CombatLog.txt";
        string pathFinal = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        if (System.IO.File.Exists(pathFinal))
        {
            string[] allLines = System.IO.File.ReadAllLines(pathFinal);

            int indexLastDate = 0;

            for (int i = 0; i < allLines.Length; i++)
            {
                string lineClean = allLines[i].Replace(" ", "").ToUpper();

                if (lineClean.Contains("FECHA:"))
                {
                    indexLastDate = i;
                }
            }

            int indexReal = indexLastDate;
            if (indexLastDate > 0)
            {
                indexReal = indexLastDate - 1;
            }

            LogLine.Clear();
            FinalStats.Clear();
            bool lineFinalStats = false;

            for (int i = indexReal; i < allLines.Length; i++)
            {
                string linea = allLines[i];

                if (!string.IsNullOrWhiteSpace(linea))
                {
                    if (linea.Contains("Daño total") || linea.Contains("Heroe con mas daño"))
                    {
                        lineFinalStats = true;
                    }

                    if (lineFinalStats == true)
                    {
                        FinalStats.Add(linea);

                    }
                    else
                    {
                        LogLine.Add(linea);
                    }
                }
            }
        }
        else
        {
            throw new Exception("No se ha encontrado CombatLog.txt  ");
        }
    }
}