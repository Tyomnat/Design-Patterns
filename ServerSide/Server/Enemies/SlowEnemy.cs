using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class SlowEnemy : Enemy
    {
        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public SlowEnemy(int Id, Map Map) : base(Id, "slow", Map)
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
