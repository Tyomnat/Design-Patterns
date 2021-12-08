using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.State
{
    class GameEndState : GameState
    {
        public GameEndState(Game Game) : base(Game) { }
        public override void Handle()
        {
            Thread.Sleep(6000);
            Environment.Exit(0);
        }
    }
}
