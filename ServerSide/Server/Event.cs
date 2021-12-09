using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Game event class for Observer update actions
    /// </summary>
    public class Event
    {
        public Event(string type, string data)
        {
            Type = type;
            Data = data + "eventend";
        }
        public string Type { get; }

        public string Data { get; }
    }
}
