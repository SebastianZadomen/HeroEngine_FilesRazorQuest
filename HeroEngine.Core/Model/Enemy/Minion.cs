using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Enemy
{
    public class Minion : Enemy
    {

        public Minion(string name, int level) : base(name, level)
        {
        }
        public Minion(string name) : base(name)
        {
        }

        public override void ActionsPerTurn(Hero[] teamPlayer, CombatLog log)
        {
            ReduceCooldowns();

            int heroTarget = ComprovationHpTeamPlayer(teamPlayer);

            EnemyUseSkills(teamPlayer[heroTarget], log);
        }

    }
}
