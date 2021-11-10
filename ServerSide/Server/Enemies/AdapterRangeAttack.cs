using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    

    class AdapterRangeAttack : AttackType
    {
        private AttackRange adaptee;

        public AdapterRangeAttack(AttackRange adaptee)
        {
            this.adaptee = adaptee;
        }

        public override bool Attack(int AIx, int AIy, List<Player> players, out string dir)
        {
            return adaptee.Attack(AIx, AIy, out dir);
        }
    }
}
