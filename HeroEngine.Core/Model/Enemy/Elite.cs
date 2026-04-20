using HeroEngine.Model.Heroes;
using HeroEngine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Enemy
{
    public class Elite : Enemy
    {
        public Elite(string name, int level) : base(name, level)
        {
        }
        public Elite(string name) : base(name)
        {
        }
        public virtual bool DropKey()
        {
            return new Random().Next(0, 8) == 4;
        }
        public override void ActionsPerTurn(Hero[] teamPlayer, CombatLog log)
        {
            ReduceCooldowns();

            int heroTarget = ComprovationHpTeamPlayer(teamPlayer);

            EnemyUseSkills(teamPlayer[heroTarget], log);
        }
    }
}
