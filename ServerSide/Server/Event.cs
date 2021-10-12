using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
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
