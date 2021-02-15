using System;
using System.Collections.Generic;

namespace foodtruacker.SharedKernel
{
    public interface IAggregateRoot
    {
        /// <summary>
        /// The aggregate identifier
        /// </summary>
        public Guid AggregateId { get; }
        public long Version { get; }

        /// <summary>
        /// Apply given events in order to rebuild aggregate
        /// </summary>
        public void LoadFromHistory(long version, IEnumerable<IDomainEvent> history);

        /// <summary>
        /// List all new events to be persisted in the event store
        /// </summary>
        public IEnumerable<IDomainEvent> GetUncommittedChanges();

        /// <summary>
        /// Clear all new events to be persisted in the event store
        /// </summary>
        public void MarkChangesAsCommitted();
    }
}
