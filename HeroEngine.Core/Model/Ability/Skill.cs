using HeroEngine.Model.Heroes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroEngine.Utils;

namespace HeroEngine.Model.Ability
{
    public abstract class Skill
    {
      
        public string Name { get; set; }
        public RarityType Rarity { get; set;  }
        public TypeSkills Type { get; init; }
        protected int PointUseBase { get; init; }
        protected int Cost { get; set; } 
       
        protected int PointsUse { get; set; }

        public Skill(string name, RarityType type, int cost, int pointUse)
        {
            Name = name;
            Rarity = type;
            Cost = cost;     
            PointsUse = pointUse;
            PointUseBase = PointsUse;
        }

        public Skill(string name)
        {
            Name = name;
            Rarity = RarityTypeRandom();
            Cost = CalculatedCost(Rarity);        
            PointsUse = CalculatedPointUse(Rarity);
            PointUseBase = PointsUse;
        }
        public abstract void AbilityActivation(Hero target, Hero caster, CombatLog log);
        public static RarityType RarityTypeRandom()
        {
            var random = new Random();
            int probability = random.Next(0, 12);  

            if (probability == 1)
            {
                return RarityType.Legendario;
            }
            else if (probability > 1 && probability <= 3)
            {
                return RarityType.Raro;
            }
            else if (probability > 3 && probability <= 7) 
            {
                return RarityType.Epico;
            }

            return RarityType.Comun;
        }
        public abstract string GetSkillEffect();

        public int CalculatedCost(RarityType rarity)
        {
            if (rarity == RarityType.Legendario) return 20;
            if (rarity == RarityType.Raro) return 15;
            if (rarity == RarityType.Epico) return 10;
            return 8; 
        }
        public int CalculatedPointUse(RarityType rarity)
        {
            if (rarity == RarityType.Legendario) return 4;
            if (rarity == RarityType.Raro) return 8;
            if (rarity == RarityType.Epico) return 12;
            return 15; 
        }
    }
}
