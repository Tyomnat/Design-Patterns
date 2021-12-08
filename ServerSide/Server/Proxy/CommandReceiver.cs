﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Proxy
{
    interface CommandReceiver
    {
        public void ExecuteAction(string message, PlayerController playerController, Player player, Game game);
    }
}
