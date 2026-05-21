#pragma warning disable CA1416 // Validate platform compatibility
using System.Diagnostics;
using ContractsLayerRestaurant.Interfaces.Services;

namespace ContractsLayerRestaurant.Configuration
{
    public class clsMyLogger : IMyLogger
    {
        public void EventLogs(string Message, EventLogEntryType EventType, string Type = "Application")
        {
            string Source = "RMLog";
            if (!EventLog.SourceExists(Source))
            {
                EventLog.CreateEventSource(Source, Type);
            }

            EventLog.WriteEntry(Source, Message, EventType);
        }
    }
}
