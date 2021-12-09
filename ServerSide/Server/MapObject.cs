using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class MapObject
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool isSolid { get; set; }

        public MapObject(int x, int y)
        {
            Id = 0;
            X = x;
            Y = y;
            isSolid = false;
        }

        public MapObject(int Id)
        {
            this.Id = Id;
            isSolid = false;
        }

        public MapObject(int x, int y, int id, bool isSolid = false)
        {
            Id = id;
            X = x;
            Y = y;
            this.isSolid = isSolid;
        }
    }
}
