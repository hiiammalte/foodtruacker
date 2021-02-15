using foodtruacker.EventSourcingRepository.Client;
using foodtruacker.SharedKernel;
using MediatR;
using System;
using System.Threading.Tasks;

namespace foodtruacker.EventSourcingRepository.Repository
{
    public class EventSourcingRepository<TAggregate> : IEventSourcingRepository<TAggregate>
        where TAggregate : IAggregateRoot, new()
    {
        private readonly IEventSourcingClient _client;
        private readonly IMediator _mediator;

        public EventSourcingRepository(IEventSourcingClient client, IMediator mediator)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<TAggregate> FindByIdAsync(Guid aggregateId)
        {
            if (aggregateId == Guid.Empty)
                throw new ArgumentNullException(nameof(aggregateId));

            var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate));
            var result = await _client.ReadEventsAsync(aggregateId);
            if (result == default)
                return default;

            aggregate.LoadFromHistory(result.Version, result.Events);
            return aggregate;
        }

        public async Task SaveAsync(TAggregate aggregate)
        {
            var aggregateVersion = aggregate.Version;
            foreach (var @event in aggregate.GetUncommittedChanges())
            {
                @event.AggregateVersion = aggregateVersion++;
                await _client.AppendEventAsync(@event);

                await _mediator.Publish((dynamic)@event);
            }
            aggregate.MarkChangesAsCommitted();
        }
    }
}
