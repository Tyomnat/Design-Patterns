using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class NormalEnemy : Enemy
    {
        public override void Attack(int AIx, int AIy, List<Player> players)
        {
            Player player = FindPlayer(AIx, AIy, players);
            if (player != null)
            {
                player.HandleDamage();
                this.State = "standing";
            }
        }

        public NormalEnemy(int Id, Map map) : base(Id, "normal", map, 2)
        {
            //
        }

        override public NormalEnemy DeepCopy()
        {
            NormalEnemy clone = (NormalEnemy)this.MemberwiseClone();

            clone.SetAlgorithm(new NormalAlgorithm());

            return clone;
        }
    }
}
