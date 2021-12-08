using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Originator<T>
    {
        private T State;

        public Memento<T> CreateMemento()
        {
            // Create memento and set state to current state.
            var memento = new Memento<T>(State);


            return memento;
        }

        public void SetMemento(Memento<T> memento)
        {
            State = memento.State;
        }

        public void SetState(T state)
        {
            State = state;
        }
    }
}
