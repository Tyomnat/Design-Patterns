using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Caretaker<T>
    {
        private List<Memento<T>> MementoList = new List<Memento<T>>();
        private IOriginator<T> Originator { get; set; }

        public Caretaker(IOriginator<T> originator)
        {
            Originator = originator;
        }


        public Memento<T> Save()
        {
            var memento = Originator.CreateMemento();
            MementoList.Add(memento);
            return memento;
        }


        public void Restore(int index)
        {
            var match = MementoList[MementoList.Count - 1 - index];

            if (match == null)
            {
                throw new ArgumentException($"Memento at index [{index}] not found, cannot restore.");
            }

            // Restore Memento.
            Originator.SetMemento(match);
        }


        public void Restore(Memento<T> memento)
        {
            var match = MementoList.FirstOrDefault(x => x == memento);

            if (match == null)
            {
                throw new ArgumentException($"Memento [{memento}] not found, cannot restore.");
            }

            Originator.SetMemento(match);
        }
    }
}

