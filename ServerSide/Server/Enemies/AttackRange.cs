using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    class AttackRange
    {
        public bool Attack(int AIx, int AIy, out string dir)
        {
            List<int> checkedNums = new List<int>();
            dir = "";
            Random rnd = new Random();
            int x=0, y=0;
            while (checkedNums.Count != 4)
            {
                int randInt = rnd.Next(1, 5);

                if (checkedNums.Contains(randInt))
                    continue;

                switch (randInt)
                {
                    case 1:
                        x = AIx;
                        y = AIy - 1;
                        if (canAttack(x, y, Map.GetInstance()))
                            dir = "Up";
                        break;
                    case 2:
                        x = AIx;
                        y = AIy + 1;
                        if (canAttack(x, y, Map.GetInstance()))
                            dir = "Down";
                        break;
                    case 3:
                        x = AIx - 1;
                        y = AIy;
                        if (canAttack(x, y, Map.GetInstance()))
                            dir = "Left";
                        break;
                    case 4:
                        x = AIx + 1;
                        y = AIy;
                        if (canAttack(x, y, Map.GetInstance()))
                            dir = "Right";
                        break;
                }

                if (dir != "")
                    break;

                checkedNums.Add(randInt);
            }

            if (dir != "")
            {
                return true;
            }
            return false;
        }

        private bool canAttack(int x, int y, Map map)
        {
            if (
                x < 0 ||
                x > map.Objects.GetLength(0) - 1 ||
                y < 0 ||
                y > map.Objects[x].Length - 1 || map.Objects[x][y].isSolid == true)
            {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
