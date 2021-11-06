using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Powerups
{
    abstract class Powerup : MapObject
    {
        public string Type;
        private Map Map;

        public Powerup(string type, int Id, Map map) : base(Id)
        {
            Type = type;
            this.Map = map;
            int[] positions = SetPosition();
            X = Map.Objects[positions[0]][positions[1]].X;
            Y = Map.Objects[positions[0]][positions[1]].Y;
            Map.Objects[positions[0]][positions[1]] = this;
        }

        private int[] SetPosition()
        {
            Random rnd = new Random();

            int X = rnd.Next(0, Map.Objects.GetLength(0));
            int Y = rnd.Next(0, Map.Objects[X].Length);

            while (Map.Objects[X][Y].Id != 0)
            {
                X = rnd.Next(0, Map.Objects.GetLength(0));
                Y = rnd.Next(0, Map.Objects[X].Length);
            }

            //Map.Objects[X][Y].Id = Id;


            return new int[] { X, Y };
        }
    }
}
