namespace EventSourcingMedium.API.Services.EventStreaming
{
    public interface IEventStoreService
    {
        Task AppendEventAsync(object eventData);
        Task<IEnumerable<EventResponse>> GetAllEvents(int start, int count);
    }
}
