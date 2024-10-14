namespace EventSourcingMedium.API.Services.EventStreaming
{
    public class EventStoreDB
    {
        public const string ConnectionString = "esdb://localhost:2113?tls=false";
        public const string StreamName = "PostInformationStream";
        public const string TaskName = "PostInformationTask";
    }
}
