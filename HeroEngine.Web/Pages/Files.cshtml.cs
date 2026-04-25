using HeroEngine.Core.Data;
using HeroEngine.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeroEngine.Web.Pages
{
    public class FilesModel : PageModel
    {
        public List<CombatResult> LastMatches { get; set; } = new List<CombatResult>();

        private string _csvPath = @"Data/Files/combat_stats.csv";

        [BindProperty] 
        public GameConfig Config { get; set; }
        public void OnGet()
        {
            if (System.IO.File.Exists(_csvPath))
            {
                var lines = System.IO.File.ReadAllLines(_csvPath);

                var lastLines = lines.Skip(1).Reverse().Take(10);

                foreach (var line in lastLines)
                {
                    var data = line.Split(',');

                    if (data.Length >= 7)
                    {
                        CombatResult combat = new CombatResult();
                        combat.HeroesNames = new List<string> { data[1] };
                        combat.EnemiesNames = new List<string> { data[2] };
                        combat.Result = data[3];
                        string textRound = data[4];
                        combat.TotalRounds = int.Parse(textRound);
                        string textDamage = data[5];
                        combat.TotalDamage = int.Parse(textDamage);
                        combat.MVP = data[6];

                        LastMatches.Add(combat);
                    }
                }
            }
            else
            {
                throw new Exception("No se ha encontrado el archivo");
            }

            ConfigManager manager = new ConfigManager();
            Config = manager.LoadConfig();

        }

        public IActionResult OnGetDownload()
        {
            if (!System.IO.File.Exists(_csvPath)) return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(_csvPath);

            return File(fileBytes, "text/csv", "combat_stats.csv");

        }
        public IActionResult OnPostSaveConfig()
        {
            ConfigManager manager = new ConfigManager();
            manager.SaveConfig(Config);
            return RedirectToPage(); 
        }
    }
}

