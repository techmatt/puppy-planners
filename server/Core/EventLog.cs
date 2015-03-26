using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server
{
    public enum EventType
    {
        General,
        Error,
        Server,
        StateRequest,
        UserAction,
    }

    class EventLog
    {
        public EventLog()
        {
            foreach (EventType type in Enum.GetValues(typeof(EventType)))
            {
                data[type] = new List<string>();
            }
        }
        public void log(EventType type, string message)
        {
            data[type].Add(message);
            Console.WriteLine(type.ToString() + ": " + message);
        }

        public Dictionary<EventType, List<string>> data = new Dictionary<EventType, List<string>>();
    }
}
