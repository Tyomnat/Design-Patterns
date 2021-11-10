using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    abstract class AttackType
    {
        public abstract bool Attack(int AIx, int AIy, List<Player> players, out string dir);
    }
}
