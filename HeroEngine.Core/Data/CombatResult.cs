using HeroEngine.Model.Heroes;

namespace HeroEngine.Data
{
    public class CombatResult
    {
        public List<string> HeroesNames { get; set; }
        public List<string> EnemiesNames { get; set; }
        public string Result { get; set; }
        public int TotalRounds { get; set; }
        public int TotalDamage { get; set; }
        public string MVP { get; set; }
    }
}
