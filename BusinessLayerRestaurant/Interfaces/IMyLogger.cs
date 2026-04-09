using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayerRestaurant.Interfaces
{
    public interface IMyLogger
    {
        void EventLogs(string Message, EventLogEntryType EventType, string Type = "Application");

    }
}
