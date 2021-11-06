using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Powerups
{
    class SpeedBoost : Powerup
    {
        public SpeedBoost(Map map) : base("speedBoost", 503, map) { }
    }
}
