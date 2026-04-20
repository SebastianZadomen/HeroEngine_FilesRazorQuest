using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HeroEngine.Model.Ability
{
    public class SupportSkills : Skill
    {
        public int Healing { get; set; }
        public SupportSkills(string name, RarityType type, int cost, int pointUse, int healing) : base(name, type, cost, pointUse)
        {
            Healing = healing;
            Type = TypeSkills.Soporte;

        }
        public SupportSkills(string name) : base(name)
        {
            Healing = CalculatedRandomHealing(Rarity);
            Type = TypeSkills.Soporte;
        }

        public override void AbilityActivation(Hero target, Hero caster, CombatLog log)
        {
            log.LogMessage("======================================================================");
            log.LogMessage("[Support]");
            log.LogMessage($"Has decidido usar tu habilidad : {Name}");
            log.LogMessage($"Curacion : {Healing} , Costo Energia : {Cost} , Usos : {PointsUse}/{PointUseBase}");
            log.LogMessage("======================================================================");


            caster.Health += Healing;
            PointsUse -= 1;
        }

        public override string GetSkillEffect()
        {
            return $"Curacion: {Healing}";
        }
    
        public int CalculatedRandomHealing(RarityType rarity)
        {
            var random = new Random();
            int healing = 10;
            if (rarity == RarityType.Legendario) return healing = random.Next(20, 25);
            if (rarity == RarityType.Raro) return healing = random.Next(15, 20);
            if (rarity == RarityType.Epico) return healing = random.Next(10, 15);
            return healing = 5;

        }
    }
}
