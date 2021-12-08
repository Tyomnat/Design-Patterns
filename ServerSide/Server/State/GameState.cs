using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.State
{
    public abstract class GameState
    {
        protected Game Game;

        public GameState(Game Game)
        {
            this.Game = Game;
        }
        public abstract void Handle();
    }
}
