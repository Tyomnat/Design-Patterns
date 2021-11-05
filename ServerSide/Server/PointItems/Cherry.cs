using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.PointItems
{
    class Cherry : PointItem
    {
        public Cherry(Map Map, int X, int Y) : base(500, 10, Map, X, Y)
        {

        }
        public Cherry(Map Map) : base(500, 10, Map)
        {

        }
    }
}
