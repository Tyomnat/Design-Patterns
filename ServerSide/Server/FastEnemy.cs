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

        public FastEnemy(int Id, Map map) : base(Id, "fast", map)
        {
        }

        //public FastEnemy(int Id, string Type)
        //{
        //    this.Id = Id;
        //    this.Type = Type;
        //}
    }
}
