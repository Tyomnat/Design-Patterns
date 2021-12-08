using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.State
{
    class PausedState : GameState
    {
        public PausedState(Game Game) : base(Game) { }
        public override void Handle()
        {
            if (!Game.isPaused)
            {
                Game.state = new InProgressState(Game);
            }
        }
    }
}
