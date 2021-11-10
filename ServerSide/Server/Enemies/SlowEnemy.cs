using Server.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class SlowEnemy : Enemy
    {
        public override bool Attack(int AIx, int AIy, List<Player> players, AttackType attackType, out string dir)
        {
            this.attackType = attackType;
            this.State = "standing";
            return attackType.Attack(AIx, AIy, players, out dir);
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
