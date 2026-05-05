using System.Diagnostics;


namespace BusinessLayerRestaurant.Interfaces
{
    public interface IMyLogger
    {
        void EventLogs(string Message, EventLogEntryType EventType, string Type = "Application");

    }
}
