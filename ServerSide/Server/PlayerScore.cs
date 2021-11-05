using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class PlayerScore
    {
        public int PlayerId { get; set; }
        public int Score { get; set; }

        public PlayerScore(int PlayerId)
        {
            this.Score = 0;
            this.PlayerId = PlayerId;
        }
    }
}
