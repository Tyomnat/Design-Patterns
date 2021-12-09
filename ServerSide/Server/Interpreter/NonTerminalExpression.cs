using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Interpreter
{
    public class NonterminalExpression : AbstractExpression
    {
        public Subject Subject;
        public NonterminalExpression(Subject subject)
        {
            Subject = subject;
        }
        public override void Interpret(string msg)
        {
            Subject.Update(new Event("new_message", msg));
        }
    }
}
