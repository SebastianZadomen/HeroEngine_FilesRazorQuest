using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Enemy
{
    public class Minion : Enemy
    {

        public Minion(string name, int level) : base(name, level)
        {
            ExperienceReward = 5 + (5 * level);

        }
        public Minion(string name) : base(name)
        {
            ExperienceReward = 5+(5 * Level);

        }

        public override void ActionsPerTurn(Hero[] teamPlayer, CombatLog log, double probability)
        {
            ReduceCooldowns();

            int heroTarget = ComprovationHpTeamPlayer(teamPlayer);

            EnemyUseSkills(teamPlayer[heroTarget], log, probability);
        }

    }
}
