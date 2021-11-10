using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    class AdapterMeleeAttack : AttackType
    {
        private AttackMelee adaptee;

        public AdapterMeleeAttack(AttackMelee adaptee)
        {
            this.adaptee = adaptee;
        }

        public override bool Attack(int AIx, int AIy, List<Player> players, out string dir)
        {
            return adaptee.Attack(AIx, AIy, players, out dir);
        }
    }
}
