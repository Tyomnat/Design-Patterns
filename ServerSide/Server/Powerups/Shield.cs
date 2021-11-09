using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Powerups
{
    class Shield : Powerup
    {
        public Shield(Map map) : base("shield", 504, map) { }
    }
}
