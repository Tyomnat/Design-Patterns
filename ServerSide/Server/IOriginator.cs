using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public interface IOriginator<T>
    {
        public Memento<T> CreateMemento();
        public void SetMemento(Memento<T> memento);


    }
}
