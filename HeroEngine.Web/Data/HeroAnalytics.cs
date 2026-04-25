using HeroEngine.Model.Ability;
using HeroEngine.Model.Heroes;
using System.Text.RegularExpressions;
namespace HeroEngine.Web.Data;

public class HeroAnalytics
{
    private List<Hero> GetHeroes()
    {
        List<Hero> all = GameData.Repository.GetAll();
        return all.Where(h => h != null).ToList();
    }

    public List<Hero> GetTopHeroesByLevel(int n)
    {
        var query = GetHeroes().OrderByDescending(h => h.Level);
        return query.Take(n).ToList();
    }

    public List<Skill> GetAbilitiesByRarity(RarityType rarity)
    {
        List<Skill> allSkills = new List<Skill>();
        foreach (var h in GetHeroes())
        {
            if (h.Skills != null)
            {
                allSkills.AddRange(h.Skills);
            }
        }
        return allSkills
            .Where(s => s != null && s.Rarity == rarity)
            .Distinct()
            .ToList();
    }

    public List<Hero> GetHeroesWithAbilityCount(int min)
    {
        return GetHeroes()
            .Where(h => h.Skills.Count() >= min) 
            .ToList();
    }

    public Dictionary<string, double> GetAverageDamagePerClass()
    {
        var groups = GetHeroes().GroupBy(h => h.GetType().Name);

        return groups.ToDictionary(
            g => g.Key,
            g => g.Average(h => (double)h.Damage)
        );
    }

    public List<Hero> SearchHeroesByName(string pattern)
    {
        try
        {
            Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            List<Hero> matches = GetHeroes().Where(h => myRegex.IsMatch(h.Name)).ToList();
            return matches;
        }
        catch
        {
            return new List<Hero>();
        }
    }

    public Hero SearchSingleHeroByName(string pattern)
    {
        try
        {
            Regex myRegex = new Regex(pattern, RegexOptions.IgnoreCase);
            return GetHeroes().FirstOrDefault(h => myRegex.IsMatch(h.Name));
        }
        catch
        {
            return null;
        }
    }
}

