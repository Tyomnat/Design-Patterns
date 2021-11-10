using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    class EnemyAttackFire : MapObject
    {
        public string direction;

        public EnemyAttackFire(string dir, int X, int Y, int Id) : base(X, Y, Id, true)
        {
            direction = dir;
        }
    }
}
