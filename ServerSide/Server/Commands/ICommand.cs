using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public abstract class ICommand
    {
        protected Player Player;

        public ICommand(Player player)
        {
            Player = player;
        }
        public abstract void Execute();

        public abstract void Undo();

    }
}
