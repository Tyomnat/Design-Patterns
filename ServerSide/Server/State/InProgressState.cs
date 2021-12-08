using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.State
{
    class InProgressState : GameState
    {
        public InProgressState(Game Game) : base(Game) { }

        public override void Handle()
        {
            if (Game.isPaused)
            {
                Game.state = new PausedState(Game);
            }
            else if (Game.PlayerCount == 1 || Game.MaxScore >= 100)
            {
                Game.state = new GameEndState(Game);
            }
        }
    }
}
