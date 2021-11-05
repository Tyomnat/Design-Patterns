using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MoveUpCommand : ICommand
    {
        public MoveUpCommand(Player player) : base(player) { }

        public override void Execute()
        {
            Player.Move("Up");
        }

        public override void Undo()
        {
            Player.Move("Down");

        }
    }
}
