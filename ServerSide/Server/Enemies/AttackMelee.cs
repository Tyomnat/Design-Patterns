using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    class AttackMelee
    {
        public bool Attack(int AIx, int AIy, List<Player> players, out string dir)
        {
            Player player = FindPlayer(AIx, AIy, players);
            dir = "";
            if (player != null)
            {
                player.HandleDamage();
                return true;
            }
            return false;
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
                y > Map.GetInstance().Objects[x].Length - 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
