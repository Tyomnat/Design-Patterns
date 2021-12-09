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
            "memento",
            "game_pause_changed",
        });

        private PlayerCommandReceiver PlayerCommandReceiver;
        public CommandProxy(PlayerCommandReceiver playerCommandReceiver)
        {
            PlayerCommandReceiver = playerCommandReceiver;
        }

        public void ExecuteAction(string message, PlayerController playerController, Player player, Game game, Subject subject)
        {
            if (this.IsMessageValid(message))
            {
                PlayerCommandReceiver.ExecuteAction(message, playerController, player, game, subject);
            }
        }

        private bool IsMessageValid(string message)
        {
            return AvailableCommands.Contains(message) || message.Contains("chat_");
        }
    }
}
