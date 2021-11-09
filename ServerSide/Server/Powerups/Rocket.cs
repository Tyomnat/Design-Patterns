using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Powerups
{
    class Rocket : Powerup
    {
        public Rocket(Map map) : base("rocket", 502, map) { }
    }
}
