using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    abstract class MoveAlgorithm
    {
        public abstract bool MoveDifferently(int x, int y, Map Map, out int newX, out int newY);
    }
}
