using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Observer Design Patter Subject Class
    /// </summary>
    public class Subject : ISubject
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
