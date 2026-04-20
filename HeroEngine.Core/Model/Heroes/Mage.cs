using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Heroes
{
    public class Mage : Hero
    {
        public const int ManaBase = 60;
        public const int ManaScale = 20;

        public int ArcaneLevel => Level * 2;
        public int ManaMax => ManaBase + ManaScale * Level;

        private int _mana;
        public int Mana { get { return _mana; }  set {
                _mana = StatsControl(value, ManaMax);  
            }
        }
        public Mage(string name, int level) : base(name, level)
        {
            Mana = ManaMax;
        }
        public Mage(string name) : base(name)
        {
            Mana = ManaMax;

        }


        public override bool Attack(Hero target, CombatLog log)
        {
            if (!base.Attack(target, log))
            {
                return false;
            }
            if (Mana.Equals(0))
            {
                log.LogMessage("No tienes mana....");
                return false;
            }
            if (Mana < 20)
            {
                log.LogMessage("No tienes suficiente mana");
                return false;
            }
            Mana -= 20;
            log.LogMessage($"{Name} Lanza un encantamiento !!");
            target.TakeDamage(DamageCritical(log), log);
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
            return base.ToString() + $" | Mana : {Mana}/{ManaMax} | Arcane Level : {ArcaneLevel} ";
        }

    }
}
