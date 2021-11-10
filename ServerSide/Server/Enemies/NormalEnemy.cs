using Server.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class NormalEnemy : Enemy
    {
        public override bool Attack(int AIx, int AIy, List<Player> players, AttackType attackType, out string dir)
        {
            this.attackType = attackType;
            this.State = "standing";
            return attackType.Attack(AIx, AIy, players, out dir);
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
