using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class FastEnemy : Enemy
    {
        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public FastEnemy(int Id, Map Map) : base(Id, "fast", Map)
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
