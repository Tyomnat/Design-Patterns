using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Observer Design Pattern Observer Interface
    /// </summary>
    public interface IObserver
    {
        void Update(Event gameEvent);
    }
}
