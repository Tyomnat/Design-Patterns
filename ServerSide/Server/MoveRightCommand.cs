using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MoveRightCommand : ICommand
    {
        public MoveRightCommand(Player player) : base(player) { }

        public override void Execute()
        {
            Player.Move("Right");
        }

        public override void Undo()
        {
            Player.Move("Left");
        }
    }
}
