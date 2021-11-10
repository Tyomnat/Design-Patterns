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

        public abstract void Attack(int AIx, int AIy, List<Player> players);

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

        public Player FindPlayer(int AIx, int AIy, List<Player> players)
        {
            int plX = 0, plY = 0;
            Player player = null;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        plX = AIx;
                        plY = AIy - 1;
                        if (inBounds(plX, plY))
                        {
                            player = players.Find(P => P.Id == Map.GetInstance().Objects[plX][plY].Id);
                            if (player != null)
                                return player;
                        }
                        break;
                    case 1:
                        plX = AIx;
                        plY = AIy + 1;
                        if (inBounds(plX, plY))
                        {
                            player = players.Find(P => P.Id == Map.GetInstance().Objects[plX][plY].Id);
                            if (player != null)
                                return player;
                        }
                        break;
                    case 2:
                        plX = AIx - 1;
                        plY = AIy;
                        if (inBounds(plX, plY))
                        {
                            player = players.Find(P => P.Id == Map.GetInstance().Objects[plX][plY].Id);
                            if (player != null)
                                return player;
                        }
                        break;
                    case 3:
                        plX = AIx + 1;
                        plY = AIy;
                        if (inBounds(plX, plY))
                        {
                            player = players.Find(P => P.Id == Map.GetInstance().Objects[plX][plY].Id);
                            if (player != null)
                                return player;
                        }
                        break;
                    default:
                        break;
                }
            }
            return player;
        }

        private bool inBounds(int x, int y)
        {
            if (
                x < 0 ||
                x > Map.GetInstance().Objects.GetLength(0) - 1 ||
                y < 0 ||
                y > Map.GetInstance().Objects[y].Length - 1)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
}
