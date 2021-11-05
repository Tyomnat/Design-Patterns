using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MoveLeftCommand : ICommand
    {
        public MoveLeftCommand(Player player) : base(player) { }

        public override void Execute()
        {
            Player.Move("Left");
        }

        public override void Undo()
        {
            Player.Move("Right");
        }
    }
}
