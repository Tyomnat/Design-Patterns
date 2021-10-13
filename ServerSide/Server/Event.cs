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
    class Event
    {
        public Event(string type, string data)
        {
            Type = type;
            Data = data;
        }
        public string Type { get; }

        public string Data { get; }
    }
}
