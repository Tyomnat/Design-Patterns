using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PointItems
{
    class Apple : PointItem
    {
        public Apple(Map Map) : base(501, 20, Map)
        {

        }
        public Apple(Map Map, int X, int Y) : base(501, 20, Map, X, Y)
        {

        }

    }
}
