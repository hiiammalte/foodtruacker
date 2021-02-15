using EventStore.Client;
using foodtruacker.SharedKernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace foodtruacker.EventSourcingRepository.Client
{
    public class EventStoreDbClient : IEventSourcingClient
    {
        private readonly EventStoreClient _client;

        public EventStoreDbClient(EventStoreClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task AppendEventAsync(IDomainEvent @event)
        {
            var eventData = new EventData(
                    Uuid.NewUuid(),
                    @event.GetType().AssemblyQualifiedName,
                    SerializeEvent(@event)
                );

            await _client.AppendToStreamAsync(
                @event.AggregateId.ToString(),
                StreamState.Any,
                new[] { eventData });
        }

        public async Task<(long Version, IEnumerable<IDomainEvent> Events)> ReadEventsAsync(Guid aggregateId)
        {
            var persistedEvents = new List<IDomainEvent>();
            long aggregateVersion = -1;

            EventStoreClient.ReadStreamResult events = _client.ReadStreamAsync(
                Direction.Backwards,
                aggregateId.ToString(),
                StreamPosition.End);

            if (await events.ReadState == ReadState.StreamNotFound)
                return default;

            await foreach (var e in events)
            {
                persistedEvents.Add(DeserializeEvent(e.Event.EventType, e.Event.Data));
                aggregateVersion++;
            }

            return (aggregateVersion, persistedEvents);
        }

        private static byte[] SerializeEvent(IDomainEvent @event)
            => Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

        private static IDomainEvent DeserializeEvent(string eventType, ReadOnlyMemory<byte> data)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = new PrivateSetterContractResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return (IDomainEvent)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(data.ToArray()), Type.GetType(eventType), settings);
        }
    }
}
