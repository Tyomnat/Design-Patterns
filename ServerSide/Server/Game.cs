using Server.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Game
    {
        public GameState state;
        public bool isPaused = false;
        public int PlayerCount;
        public int MaxScore;
        public int winnerId = -1;

        public Game()
        {
            state = new WaitingState(this);
        }
    }
}
