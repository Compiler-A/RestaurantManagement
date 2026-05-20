using System.Diagnostics;

namespace ContractsLayerRestaurant.Interfaces.Services
{
    public interface IMyLogger
    {
        void EventLogs(string Message, EventLogEntryType EventType, string Type = "Application");

    }
}
