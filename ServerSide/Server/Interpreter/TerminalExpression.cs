using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interpreter
{
    public class TerminalExpression : AbstractExpression
    {
        private Player Player;
        public TerminalExpression(Player player)
        {
            Player = player;
        }
        private readonly ReadOnlyCollection<string> AvailableTokens = new ReadOnlyCollection<string>(new[]
        {
            "/name"
        });
        public override void Interpret(string msg)
        {
            if (AvailableTokens.Contains(msg.Split(" ")[0]))
            {
                Player.username = msg.Split(" ")[1];
                Player.SendMessage("username_updated" + Player.GetUsername() + "eventend");
            }
        }
    }
}
