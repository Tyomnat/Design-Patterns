using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PointItems
{
    class PoisonousSound : Sound
    {
        public override string Play()
        {
            return "poisonousSound";
        }
    }
}
