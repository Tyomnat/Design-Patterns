using Server.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FastEnemy : Enemy
    {
        public override bool Attack(int AIx, int AIy, List<Player> players, AttackType attackType, out string dir)
        {
            this.attackType = attackType;
            this.State = "standing";
            return attackType.Attack(AIx, AIy, players, out dir);
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
