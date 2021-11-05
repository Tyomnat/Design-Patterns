using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatternsClientSide
{
    class PlayerScore
    {
        public int PlayerId { get; set; }
        public int Score { get; set; }

        public PlayerScore(int PlayerId, int Score)
        {
            this.Score = Score;
            this.PlayerId = PlayerId;
        }
    }
}
