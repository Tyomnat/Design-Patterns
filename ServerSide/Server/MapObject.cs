using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MapObject
    {
        public int Id { get; set; }
        int X { get; set; }
        int Y { get; set; }

        public MapObject(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
