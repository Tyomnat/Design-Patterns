using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Subject : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void Register(IObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

            }
            else
            {
                Console.WriteLine("Could not register. already exists");
            }
        }

        public void Unregister(IObserver observer)
        {
            throw new NotImplementedException();
        }

        public void Update(Event gameEvent)
        {
            foreach (var item in observers)
            {
                item.Update(gameEvent);
            }
        }
    }
}
