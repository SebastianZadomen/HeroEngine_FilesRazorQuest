using HeroEngine.Data;
using HeroEngine.Model.Heroes;
using HeroEngine.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HeroEngine.Web.Pages
{
    public class StatsModel : PageModel
    {
        public HeroAnalytics AnalyticsTool = new HeroAnalytics();

        public Dictionary<string, int> HeroDistribution { get; set; }
        public List<Hero> Top3Heroes { get; set; }
        public Dictionary<string, int> CommonAbilities { get; set; }
        public List<CombatResult> FilteredCombats { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ResultFilter { get; set; }

        public void OnGet()
        {
            List<Hero> allHeroes = GameData.Repository.GetAll();
            List<Hero> cleanHeroes = new List<Hero>();

            foreach (var h in allHeroes)
            {
                if (h != null) cleanHeroes.Add(h);
            }

            HeroDistribution = cleanHeroes
                .GroupBy(hero => hero.GetType().Name)
                .ToDictionary(group => group.Key, group => group.Count());

            Top3Heroes = AnalyticsTool.GetTopHeroesByLevel(3);

            var allSkills = new List<HeroEngine.Model.Ability.Skill>();
            foreach (var h in cleanHeroes)
            {
                if (h.Skills != null)
                {
                    allSkills.AddRange(h.Skills);
                }
            }

            CommonAbilities = allSkills
                .Where(s => s != null)
                .GroupBy(s => s.GetType().Name)
                .ToDictionary(g => g.Key, g => g.Count());

            List<CombatResult> allMatches = LoadCombatsFromCsv();

            if (string.IsNullOrEmpty(ResultFilter))
            {
                FilteredCombats = allMatches;
            }
            else
            {
                FilteredCombats = new List<CombatResult>();
                foreach (var match in allMatches)
                {
                    if (match.Result == ResultFilter)
                    {
                        FilteredCombats.Add(match);
                    }
                }
            }
        }

        private List<CombatResult> LoadCombatsFromCsv()
        {
            List<CombatResult> list = new List<CombatResult>();
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\HeroEngine.Web\Data\Files\combat_stats.csv");

            if (System.IO.File.Exists(path))
            {
                string[] lines = System.IO.File.ReadAllLines(path);

                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        string[] data = line.Split(',');

                    string[] data = line.Split(',');
                    if (data.Length >= 7)
                    {
                        CombatResult combat = new CombatResult();
                        combat.HeroesNames = new List<string> { data[1] };
                        combat.EnemiesNames = new List<string> { data[2] };
                        combat.Result = data[3];

                        int rounds = 0;
                        int.TryParse(data[4], out rounds);
                        combat.TotalRounds = rounds;

                        int damage = 0;
                        int.TryParse(data[5], out damage);
                        combat.TotalDamage = damage;

                        combat.MVP = data[6];

                        list.Add(combat);
                    }
                    }
                }
            }
            return list;
        }
    }
}
