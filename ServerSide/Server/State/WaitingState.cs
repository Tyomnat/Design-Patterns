using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.State
{
    class WaitingState : GameState
    {
        public WaitingState(Game Game) : base(Game) { }
        public override void Handle()
        {
            Game.state = new InProgressState(Game);
        }
    }
}
