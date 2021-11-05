using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class CommandFactory
    {
        public static ICommand GetCommand(string message, Player player)
        {
            switch (message)
            {
                case "Up":
                    return new MoveUpCommand(player);
                case "Down"://sometimes DownDown, etc., duplicates
                    return new MoveDownCommand(player);
                case "Left":
                    return new MoveLeftCommand(player);
                case "Right":
                    return new MoveRightCommand(player);
                default:
                    throw new Exception("Unknown command");
            }
        }
    }
}
