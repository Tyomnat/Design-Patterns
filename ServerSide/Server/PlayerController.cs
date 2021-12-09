using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class PlayerController
    {
        private List<ICommand> Commands = new List<ICommand>();

        public List<ICommand> GetCommands()
        {
            return Commands;
        }

        public void Run(ICommand cmd)
        {
            Commands.Add(cmd);
            cmd.Execute();
        }

        public void Undo()
        {
            int index = Commands.Count;
            if (index > 0)
            {
                ICommand cmd = Commands[index - 1];
                Commands.RemoveAt(index - 1);
                cmd.Undo();
            }
        }


    }
}
