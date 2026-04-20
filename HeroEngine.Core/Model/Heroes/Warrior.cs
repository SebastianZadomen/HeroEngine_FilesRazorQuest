using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Heroes
{
    public class Warrior : Hero , IEnergy
    {
        public const int ArmorBase = 3;
        public const int EnergyBase = 100;
        public const int EnergyScaled = 10;

        public int Armor => ArmorBase + (ArmorBase * Level );

        public int Energy { get; set; }
        public int EnergyMax => EnergyBase + (EnergyScaled * Level);

        public Warrior(string name) : base(name)
        {
            Energy = EnergyMax;
            
        }

        public Warrior(string name, int level) : base(name, level)
        {
            Energy = EnergyMax;
           
        }
        public override bool Attack(Hero target, CombatLog log)
        {
            if (!base.Attack(target, log))
            {
                return false;
            }

            log.LogMessage("Grito de batalla: «¡Por Bytecroft! ¡Mi código compila a la primera!»");
            log.LogMessage($"{Name} ataca !!");
            target.TakeDamage(DamageCritical(log), log);
            return true;
        }
        public override bool TakeDamage(int damage, CombatLog log)
        {
            if (!base.TakeDamage(damage, log))
            {
                return false;
            }

            Health -= Math.Max(0, damage - (Armor + Defense));
            log.LogMessage($"{Name} ha recibido {damage} de daño pero tu armadura te ha protegido {Armor} de daño\nHP : {Health}/{HealthMax}");
            log.LogMessage("======================================================================");

            return true;
        }
        public override string ToString()
        {
            return base.ToString() + $" Armor : {Armor}  | Energia : {Energy}/{EnergyMax}";
        }
    }
}
