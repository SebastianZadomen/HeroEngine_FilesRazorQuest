using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Ability
{
    public class AttackSkills : Skill
    {
        public int Damage { get; set; }

        public AttackSkills() : base() { }

        public AttackSkills(string name, RarityType type, int cost, int pointUse,int damage) : base(name, type, cost, pointUse)
        {
            Damage = damage;
            Type = TypeSkills.Ataque;
        }
        public AttackSkills(string name) : base(name)
        {
            Damage = CalculatedDamage(Rarity);
            Type = TypeSkills.Ataque;
        }

        public override void AbilityActivation(Hero target, Hero caster, CombatLog log, double probability)
        {
            log.LogMessage("======================================================================");
            log.LogMessage("[Attack]");
            log.LogMessage($"Has decidido usar tu habilidad : {Name}");
            log.LogMessage($"Daño : {Damage} , Costo Energia : {Cost} , Usos : {PointsUse}/{PointUseBase}");
            log.LogMessage("======================================================================");
            

            target.TakeDamage(CalculteProbabilityCritic(probability), log);
            PointsUse -= 1;

            if (caster is IEnergy energyUser)
            {
                energyUser.Energy -= Cost;
            }
            else if (caster is Mage mg)
            {
                mg.Mana -= Cost;
            }
        }
        public override string GetSkillEffect()
        {
            return $"Damage: {Damage}";
        }

        public int CalculatedDamage(RarityType rarity)
        {
            var random = new Random();
            int damage = 10;
            if (rarity == RarityType.Legendario) return damage = random.Next(30,41);
            if (rarity == RarityType.Raro) return damage = random.Next(15, 21);
            if (rarity == RarityType.Epico) return damage = random.Next(20, 31);
            return damage = 10;

        }
        public int CalculteProbabilityCritic(double probability)
        {
            Random rnd = new Random();
            double probabilidadExito = probability;
            double damage = Damage;

            if (rnd.NextDouble() < probabilidadExito)
            {
                damage = damage * 1.2; 
            }
            return (int)damage;
        }
    }
}
