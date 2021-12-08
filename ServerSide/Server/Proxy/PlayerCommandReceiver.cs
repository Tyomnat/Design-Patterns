using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Proxy
{
    class PlayerCommandReceiver : CommandReceiver
    {
        public void ExecuteAction(string message, PlayerController playerController, Player player)
        {
            if (message == "Undo")
            {
                playerController.Undo();
            }
            else if (message == "memento")
            {
                player.Caretaker.Restore(4);
            }
            else
            {
                ICommand command = CommandFactory.GetCommand(message, player);
                playerController.Run(command);
            }
        }
    }
}
