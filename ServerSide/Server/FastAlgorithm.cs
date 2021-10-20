using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FastAlgorithm : MoveAlgorithm
    {

        public override bool MoveDifferently(int x, int y, Map map, out int newX, out int newY)
        {
            newX = x;
            newY = y;


            List<int> checkedNumbers = new List<int>();
            Random rnd = new Random();
            int randInt = 0;
            newX = x;
            newY = y;

            while (checkedNumbers.Count != 4)
            {

                newX = x;
                newY = y;
                randInt = rnd.Next(1, 5);
                if (checkedNumbers.Contains(randInt))
                {
                    continue;
                }

                switch (randInt)
                {
                    case 1:
                        newX = x;
                        newY = y - 2;
                        break;
                    case 2:
                        newX = x;
                        newY = y + 2;
                        break;
                    case 3:
                        newX = x - 2;
                        newY = y;
                        break;
                    case 4:
                        newX = x + 2;
                        newY = y;
                        break;
                }
                if (
                    newX < 0 ||
                    newX > map.Objects.GetLength(0) - 1 ||
                    newY < 0 ||
                    newY > map.Objects[newX].Length - 1 || map.Objects[newX][newY].isSolid == true)
                {
                    checkedNumbers.Add(randInt);
                    continue;
                }
                if (map.Objects[newX][newY].isSolid != true && !isBetweenBlock(newX, newY, map, randInt))
                {
                    return true;
                    //break;
                }

            }

            return false;
        }

        private bool isBetweenBlock(int newX, int newY, Map map, int randInt)
        {
            switch (randInt)
            {
                case 1:
                    return map.Objects[newX][newY + 1].isSolid;

                case 2:
                    return map.Objects[newX][newY - 1].isSolid;


                case 3:
                    return map.Objects[newX + 1][newY].isSolid;

                case 4:
                    return map.Objects[newX - 1][newY].isSolid;
                default:
                    return true;
            }
        }
    }
}
