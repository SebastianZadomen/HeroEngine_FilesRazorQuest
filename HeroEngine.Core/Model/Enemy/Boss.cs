using HeroEngine.Model.Ability;
using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Enemy
{
    public class Boss : Enemy
    {
        public Boss(string name, int level) : base(name, level)
        {
        }
        public Boss(string name) : base(name)
        {
        }
        public virtual bool DropKey()
        {
            return new Random().Next(0, 5) == 2;
        }
        public override void ActionsPerTurn(Hero[] teamPlayer, CombatLog log)
        {
            ReduceCooldowns();
            int heroTarget = ComprovationHpTeamPlayer(teamPlayer);
            EnemyUseSkills(teamPlayer[heroTarget], log);
        }

        public override bool EnemyUseSkills(Hero player,CombatLog log )
        {
            if (Health < (HealthMax * 0.20))
            {
                for (int i = 0; i < Skills.Length; i++)
                {
                    if (Skills[i] != null && AbilityCooldowns[i] == 0 && Skills[i].Type == TypeSkills.Soporte)
                    {
                        Console.WriteLine($"\n¡CUIDADO! {Name} está en apuros y usa {Skills[i].Name} para curarse.");

                        ControlPositionAbilityCooldowns(i + 1);

                        Skills[i].AbilityActivation(this, this, log);

                        return true; 
                    }
                }
            }

            return base.EnemyUseSkills(player,log);
        }
    }
}
