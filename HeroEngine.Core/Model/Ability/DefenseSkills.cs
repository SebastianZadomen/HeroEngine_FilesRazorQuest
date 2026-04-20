using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Ability
{
    public class DefenseSkills : Skill
    {
        public int Defense { get; set; }
        public DefenseSkills(string name, RarityType type, int cost, int pointUse, int healing) : base(name, type, cost, pointUse)
        {
            Defense = healing;
            Type = TypeSkills.Defensa;

        }
        public DefenseSkills(string name) : base(name)
        {
            Defense = CalculatedRandomDefense(Rarity);
            Type = TypeSkills.Defensa;

        }

        public override void AbilityActivation(Hero target, Hero caster, CombatLog log)
        {
            log.LogMessage("======================================================================");
            log.LogMessage("[Defense]");
            log.LogMessage($"Has decidido usar tu habilidad : {Name}");
            log.LogMessage($"Defensa : {Defense} , Costo Energia : {Cost} , Usos : {PointsUse}/{PointUseBase}");

            caster.Defense += Defense;
            PointsUse -= 1;
        }

        public override string GetSkillEffect()
        {
            return $"Defense: {Defense}";
        }
        public int CalculatedRandomDefense(RarityType rarity)
        {
            var random = new Random();
            int defense = 0;
            if (rarity == RarityType.Legendario) return defense = random.Next(16, 21);
            if (rarity == RarityType.Raro) return defense = random.Next(12, 16);
            if (rarity == RarityType.Epico) return defense = random.Next(6, 13);
            return defense = 5;
        }
    }
}
