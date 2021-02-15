using System;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.SharedKernel
{
    public abstract class AggregateRoot : IAggregateRoot
    {
        readonly ICollection<IDomainEvent> _uncommittedEvents = new List<IDomainEvent>();
        public Guid AggregateId { get; protected set; } = Guid.Empty;
        public long Version { get; private set; } = -1;

        protected List<string> _businessLogicErrors = new List<string>();

        public void MarkChangesAsCommitted()
        {
            _uncommittedEvents.Clear();
        }

        protected abstract void When(IDomainEvent @event);

        public void RaiseEvent(IDomainEvent @event)
        {
            When(@event);
            _uncommittedEvents.Add(@event);
        }

        public void LoadFromHistory(long version, IEnumerable<IDomainEvent> history)
        {
            Version = version;

            foreach (var @event in history)
            {
                When(@event);
            }
        }

        public IValueObject CheckAndAssign(ValueObjectValidationResult validatedValueObject)
        {
            if (validatedValueObject?.ValueObject != null)
            {
                return validatedValueObject.ValueObject;
            }
            else
            {
                if (validatedValueObject?.BusinessErrors?.Any() == true)
                    _businessLogicErrors.AddRange(validatedValueObject.BusinessErrors);

                return null;
            }
        }

        public IEnumerable<IDomainEvent> GetUncommittedChanges() => _uncommittedEvents;
    }
}
