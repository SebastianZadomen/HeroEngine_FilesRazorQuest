using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Heroes
{
    public class Rogue : Hero , IEnergy
    {
        
        public const int SneakAttackScale = 5;
        public const int Multiplier = 2;
        public const int EnergyBase = 100;
        public const int EnergyScaled = 10;


        public int Dagas { get; set; }
        public int SneakAttack => SneakAttackScale * Level ;
        public int Energy { get; set; }
        public int EnergyMax => EnergyBase + (EnergyScaled * Level);
        public Rogue(string name) : base(name)
        {
            Energy = EnergyMax;
        }

        public Rogue(string name, int level) : base(name, level)
        {
            Energy = EnergyMax;
            Dagas = 0;
        }
        public override bool Attack(Hero target, CombatLog log)
        {
            if (!base.Attack(target, log))
            {
                return false;
            }

            log.LogMessage($"{Name} ataca !! Dagas : {Dagas}");
            int damage = DamageCritical(log) + SneakAttack + (Dagas * Multiplier);
            target.TakeDamage(damage, log);

            if (Dagas < 5)
            {
                Dagas += 1;
            }
            else
            {
                Dagas = 0;
            }

            return true;
        }


        public override bool TakeDamage(int damage, CombatLog log)
        {
            if (!base.TakeDamage(damage, log))
            {
                return false;
            }

            Health -= Math.Max(0, damage - Defense);
            log.LogMessage($"{Name} ha recibido {damage} de daño. \nHP : {Health}/{HealthMax}");
            log.LogMessage("======================================================================");

            Defense = 0;
            return true;
        }
        public override string ToString()
        {
            return base.ToString() + $" SneakAttack : {SneakAttack} | N.Dagas : {Dagas} ";
        }
    }
}
