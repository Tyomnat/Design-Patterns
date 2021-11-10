using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class NormalAlgorithm : MoveAlgorithm
    {
        public override string MoveDifferently(int x, int y, Map Map, out int newX, out int newY)
        {
            List<int> checkedNumbers = new List<int>();
            Random rnd = new Random();
            int randInt = 0;
            newX = x;
            newY = y;

            if (rnd.Next(1, 100) <= 20)
            {
                return "attacking";
            }

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
                        newY = y - 1;
                        break;
                    case 2:
                        newX = x;
                        newY = y + 1;
                        break;
                    case 3:
                        newX = x - 1;
                        newY = y;
                        break;
                    case 4:
                        newX = x + 1;
                        newY = y;
                        break;
                }
                if (
                    newX < 0 ||
                    newX > Map.GetInstance().Objects.GetLength(0) - 1 ||
                    newY < 0 ||
                    newY > Map.GetInstance().Objects[newX].Length - 1 || Map.GetInstance().Objects[newX][newY].isSolid == true)
                {
                    checkedNumbers.Add(randInt);
                    continue;
                }
                if (Map.GetInstance().Objects[newX][newY].isSolid != true)
                {
                    if (ContainsPickupItem(Map.GetInstance().Objects[newX][newY].Id) || IsOnPickupItem)
                    {
                        HandlePickupItem(x, y, newX, newY);
                    }
                    return "moving";
                    //break;
                }

            }
            return "standing";
        }
    }
}
