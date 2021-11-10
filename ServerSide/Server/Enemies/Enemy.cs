using System;
using System.Collections.Generic;
using Server.Enemies;

namespace Server
{
    abstract class Enemy : Prototype<Enemy>
    {
        MoveAlgorithm MoveAlgorithm;

        public int Id;
        int lives;
        public string State;
        string Type;
        int initialX;
        int initialY;
        public AttackType attackType;

        public abstract bool Attack(int AIx, int AIy, List<Player> players, AttackType attackType, out string dir);

        public Enemy(int Id, string Type, Map map, int lives)
        {
            this.Id = Id;
            this.Type = Type;
            this.lives = lives;
            SetPosition(map);
        }

        public MoveAlgorithm GetAlgorithm()
        {
            return MoveAlgorithm;
        }

        public string executeAlgorithm(int x, int y, Map map, out int newX, out int newY)
        {
            return this.MoveAlgorithm.MoveDifferently(x, y, map, out newX, out newY);
        }

        public void SetAlgorithm(MoveAlgorithm MoveAlgorithm)
        {
            this.MoveAlgorithm = MoveAlgorithm;
        }

        public MoveAlgorithm CloneMovementAlgorithm(MoveAlgorithm moveAlgorithm)
        {
            Type temp = moveAlgorithm.GetType();

            if (temp == typeof(NormalAlgorithm))
            {
                return new NormalAlgorithm();
            }
            else if (temp == typeof(SlowAlgorithm))
            {
                return new SlowAlgorithm();
            }
            else if (temp == typeof(FastAlgorithm))
            {
                return new FastAlgorithm();
            }
            else
            {
                return new GhostAlgorithm();
            }
        }

        public void SetPosition(Map map)
        {
            Random rnd = new Random();
            int X = rnd.Next(0, map.Objects.GetLength(0));
            int Y = rnd.Next(0, map.Objects[X].Length);

            while (map.Objects[X][Y].Id != 0)
            {
                X = rnd.Next(0, map.Objects.GetLength(0));
                Y = rnd.Next(0, map.Objects[X].Length);
            }

            initialX = X;
            initialY = Y;

            map.Objects[X][Y].Id = Id;
            map.Objects[X][Y].isSolid = true;
        }
    }
}
