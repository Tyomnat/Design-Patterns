using Server.PointItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    abstract class PointItem : MapObject
    {
        Sound Sound;
        public int Amount { get; set; }
        private Map Map;

        public void SetSound(Sound sound)
        {
            this.Sound = sound;
        }

        public string Play()
        {
            return Sound != null ? this.Sound.Play() : "";
        }

        public PointItem(int Id, int Amount, Map Map) : base(Id)
        {
            this.Map = Map;
            this.Amount = Amount;
            int[] positions = SetPosition();
            X = Map.Objects[positions[0]][positions[1]].X;
            Y = Map.Objects[positions[0]][positions[1]].Y;
            Map.Objects[positions[0]][positions[1]] = this;

        }

        public PointItem(int Id, int Amount, Map Map, int X, int Y) : base(X, Y, Id)
        {
            this.Map = Map;
            this.Amount = Amount;
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
