using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FastEnemy : Enemy
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

        public FastEnemy(int Id, Map Map) : base(Id, "fast", Map, 1)
        {
            //
        }

        override public FastEnemy DeepCopy()
        {
            FastEnemy clone = (FastEnemy)this.MemberwiseClone();

            clone.SetAlgorithm(new FastAlgorithm());

            return clone;
        }

        //public FastEnemy(int Id, string Type)
        //{
        //    this.Id = Id;
        //    this.Type = Type;
        //}
    }
}
