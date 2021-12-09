using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Memento<T>
    {
        public T State { get; set; }

        public Memento() { }

        public Memento(T state)
        {
            State = state;
        }

        public override string ToString()
        {
            return State.ToString();
        }
    }
}
