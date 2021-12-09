using Server.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Proxy
{
    class PlayerCommandReceiver : CommandReceiver
    {
        public void ExecuteAction(string message, PlayerController playerController, Player player, Game game, Subject subject)
        {
            if (message == "Undo")
            {
                playerController.Undo();
            }
            else if (message == "memento")
            {
                player.Caretaker.Restore(4);
            }
            else if (message == "game_pause_changed")
            {
                game.isPaused = !game.isPaused;
            }
            else if (message.Contains("chat_"))
            {
                string parsedMsg = message.Split("chat_")[1];
                AbstractExpression expression;
                if (parsedMsg.Split(" ")[1][0] == '/')
                {
                    expression = new TerminalExpression(player);
                    parsedMsg = parsedMsg.Split(" ")[1] + " " + parsedMsg.Split(" ")[2];
                }
                else
                {
                    expression = new NonterminalExpression(subject);
                }
                expression.Interpret(parsedMsg);
            }
            else
            {
                ICommand command = CommandFactory.GetCommand(message, player);
                playerController.Run(command);
            }
        }
    }
}
