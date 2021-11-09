using Server.PointItems;
using Server.Powerups;
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
        protected Powerup StandingOnPowerup;
        protected bool IsOnPickupItem = false;
        protected Type PickupType;
        protected int OnPickupItemId;
        protected List<int> PickupItemsIds = new List<int>() { 500, 501, 502, 503, 504 };//pickup and power up items

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
                HandlePickupItemType(x, y);

            }
            if (ContainsPickupItem(Map.GetInstance().Objects[newX][newY].Id))
            {
                PickupType = Map.GetInstance().Objects[newX][newY].GetType();
                IsOnPickupItem = true;
                if (PickupType == typeof(Apple) || PickupType == typeof(Cherry))
                {
                    StandingOnItem = Map.GetInstance().Objects[newX][newY] as PointItem;
                }
                else if (PickupType == typeof(Rocket) || PickupType == typeof(Shield) || PickupType == typeof(SpeedBoost))
                {
                    StandingOnPowerup = Map.GetInstance().Objects[newX][newY] as Powerup;
                }


                OnPickupItemId = Map.GetInstance().Objects[newX][newY].Id;
            }
        }

        protected void HandlePickupItemType(int x, int y)
        {
            if (PickupType == typeof(Apple))//any new pickup type must be added here
            {
                Map.GetInstance().Objects[x][y] = StandingOnItem as Apple;
            }
            if (PickupType == typeof(Cherry))
            {
                Map.GetInstance().Objects[x][y] = StandingOnItem as Cherry;
            }
            if (PickupType == typeof(Rocket))
            {
                Map.GetInstance().Objects[x][y] = StandingOnPowerup as Rocket;
            }
            if (PickupType == typeof(Shield))
            {
                Map.GetInstance().Objects[x][y] = StandingOnPowerup as Shield;
            }
            if (PickupType == typeof(SpeedBoost))
            {
                Map.GetInstance().Objects[x][y] = StandingOnPowerup as SpeedBoost;
            }
        }

    }
}
