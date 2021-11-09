using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class SlowEnemy : Enemy
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

        public SlowEnemy(int Id, Map Map) : base(Id, "slow", Map, 2)
        {
            //
        }

        override public SlowEnemy DeepCopy()
        {
            SlowEnemy clone = (SlowEnemy) this.MemberwiseClone();

            clone.SetAlgorithm(CloneMovementAlgorithm(clone.GetAlgorithm()));

            return clone;
        }
    }
}
