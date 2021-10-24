using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class MoveDownCommand : ICommand
    {
        public MoveDownCommand(Player player) : base(player) { }
        public override void Execute()
        {
            Player.Move("Down");
        }

        public override void Undo()
        {
            Player.Move("Up");
        }
    }
}
