using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class NormalEnemy : Enemy
    {
        public override void Attack()
        {
            throw new NotImplementedException();
        }

        public NormalEnemy(int Id, Map map) : base(Id, "normal", map)
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
