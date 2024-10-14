using EventStore.Client;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;

namespace EventSourcingMedium.API.Services.EventStreaming
{
    public class EventStoreService : IEventStoreService
    {
        private readonly ILogger<EventStoreService> _logger;
        private readonly EventStoreClient _client;

        public EventStoreService(ILogger<EventStoreService> logger, EventStoreClientSettings eventStoreConnection)
        {
            _logger = logger;
            eventStoreConnection.DefaultCredentials = new UserCredentials("admin", "changeit");
            
            _client = new EventStoreClient(eventStoreConnection);
        }

        public async Task AppendEventAsync(object eventData)
        {
            try
            {
                var eventPayload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(eventData));
                var eventDataInfo = new EventStore.Client.EventData(Uuid.NewUuid(), eventData.GetType().Name, eventPayload);
                await _client.AppendToStreamAsync(EventStoreDB.StreamName, StreamState.Any, new[] { eventDataInfo });
                _logger.LogInformation($"Event {eventData.GetType().Name} appended to stream {EventStoreDB.StreamName} on EventStoreDB");
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<EventResponse>> GetAllEvents(int start, int count)
        {
            var streamEvents = new List<EventResponse>();
            List<EventStore.Client.ResolvedEvent> currentSlice;
            currentSlice = await _client.ReadStreamAsync(Direction.Forwards, EventStoreDB.StreamName, EventStore.Client.StreamPosition.FromInt64(start), count, false).ToListAsync();
            foreach (var resolvedEvent in currentSlice)
            {
                var eventData = resolvedEvent.Event;
                var eventType = eventData.EventType;
                var eventPayload = eventData.Data;
                var deserializedObject = DeserializeEvent(eventData, eventType);
                if(deserializedObject != null)
                    streamEvents.Add(deserializedObject);
            }
            return streamEvents;
        }

        public EventResponse? DeserializeEvent(EventRecord eventData, string eventType)
        {
            var item = Encoding.UTF8.GetString(eventData.Data.ToArray());
            var eventPayload = JsonConvert.DeserializeObject<EventResponse>(item);
            if(eventPayload != null)
                eventPayload.EventType = eventType;

            return eventPayload;
        }
    }
}
