using Server.PointItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    abstract class MoveAlgorithm
    {
        protected PointItem StandingOnItem;
        protected bool IsOnPickupItem = false;
        protected Type PickupType;
        protected int OnPickupItemId;
        protected List<int> PickupItemsIds = new List<int>() { 500, 501 };
        public abstract bool MoveDifferently(int x, int y, Map Map, out int newX, out int newY);

        protected bool ContainsPickupItem(int id)
        {
            return PickupItemsIds.Contains(id);
        }

        protected void HandlePickupItem(int x, int y, int newX, int newY)
        {
            if (IsOnPickupItem)
            {
                Map.GetInstance().Objects[x][y].Id = OnPickupItemId;
                IsOnPickupItem = false;
                Map.GetInstance().Objects[x][y].isSolid = false;
                if (PickupType == typeof(Apple))//any new pickup type must be added here
                {
                    // Map.GetInstance().Objects[x][y] = new Apple(Map.GetInstance(), Map.GetInstance().Objects[x][y].X, Map.GetInstance().Objects[x][y].Y);
                    Map.GetInstance().Objects[x][y] = StandingOnItem as Apple;
                }
                if (PickupType == typeof(Cherry))
                {
                    Map.GetInstance().Objects[x][y] = StandingOnItem as Cherry;
                    //Map.GetInstance().Objects[x][y] = new Cherry(Map.GetInstance(), Map.GetInstance().Objects[x][y].X, Map.GetInstance().Objects[x][y].Y);
                }

            }
            if (ContainsPickupItem(Map.GetInstance().Objects[newX][newY].Id))
            {
                PickupType = Map.GetInstance().Objects[newX][newY].GetType();
                IsOnPickupItem = true;
                StandingOnItem = Map.GetInstance().Objects[newX][newY] as PointItem;
                OnPickupItemId = Map.GetInstance().Objects[newX][newY].Id;
            }
        }

    }
}
