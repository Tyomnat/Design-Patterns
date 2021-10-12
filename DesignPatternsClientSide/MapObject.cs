using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsClientSide
{
    class MapObject
    {
        // ID value meanings: 0 - empty; 1 - wall; 100-200 - Clients (players)
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public MapObject(int x, int y, int id)
        {
            X = x;
            Y = y;
            Id = id;
        }
    }
}
