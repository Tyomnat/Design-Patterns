using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Proxy
{
    class CommandProxy : CommandReceiver
    {
        private readonly ReadOnlyCollection<string> AvailableCommands = new ReadOnlyCollection<string>(new[]
        {
            "Left",
            "Right",
            "Up",
            "Down",
            "Undo",
            "memento"
        });

        private PlayerCommandReceiver PlayerCommandReceiver;
        public CommandProxy(PlayerCommandReceiver playerCommandReceiver)
        {
            PlayerCommandReceiver = playerCommandReceiver;
        }

        public void ExecuteAction(string message, PlayerController playerController, Player player)
        {
            if (this.IsMessageValid(message))
            {
                PlayerCommandReceiver.ExecuteAction(message, playerController, player);
            }
        }

        private bool IsMessageValid(string message)
        {
            return AvailableCommands.Contains(message);
        }
    }
}
