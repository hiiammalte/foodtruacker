using System;

namespace foodtruacker.SharedKernel
{
    public abstract class DomainEvent : EventSourcingNotification, IDomainEvent
    {
        public Guid EventId { get; private set; }
        public Guid AggregateId { get; protected set; }
        public long AggregateVersion { get; set; }

        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
        }

        protected DomainEvent(Guid aggregateId) : this()
        {
            AggregateId = aggregateId;
        }

        protected DomainEvent(Guid aggregateId, long aggregateVersion) : this(aggregateId)
        {
            AggregateVersion = aggregateVersion;
        }
    }
}
