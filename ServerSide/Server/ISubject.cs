using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    interface ISubject
    {
        public void Update(Event gameEvent);

        void Register(IObserver observer);
        void Unregister(IObserver observer);
    }
}
