using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    abstract class Enemy
    {
        MoveAlgorithm MoveAlgorithm;

        public int Id;
        string State;//
        string Type;
        public abstract void Attack();

        public bool executeAlgorithm(int x, int y, Map map, out int newX, out int newY)
        {
            return this.MoveAlgorithm.MoveDifferently(x, y, map, out newX, out newY);
        }

        public void SetAlgorithm(MoveAlgorithm MoveAlgorithm)
        {
            this.MoveAlgorithm = MoveAlgorithm;
        }

        public MoveAlgorithm GetAlgorithm()
        {
            return MoveAlgorithm;
        }

        public Enemy(int Id, string Type, Map map)
        {
            this.Id = Id;
            this.Type = Type;
            SetPosition(map);
        }

        private void SetPosition(Map map)
        {
            Random rnd = new Random();
            int X = rnd.Next(0, map.Objects.GetLength(0));
            int Y = rnd.Next(0, map.Objects[X].Length);

            while (map.Objects[X][Y].Id != 0)
            {
                X = rnd.Next(0, map.Objects.GetLength(0));
                Y = rnd.Next(0, map.Objects[X].Length);
            }

            map.Objects[X][Y].Id = Id;
            map.Objects[X][Y].isSolid = true;
        }

    }
}
