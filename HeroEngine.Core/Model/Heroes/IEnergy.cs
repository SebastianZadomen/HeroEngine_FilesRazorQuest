using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HeroEngine.Model.Heroes
{
    public interface IEnergy
    {
      
        public int Energy { get; set; }
        public int EnergyMax { get;  }

    }
}
