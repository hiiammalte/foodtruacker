using foodtruacker.SharedKernel;
using System;

namespace foodtruacker.Domain.BoundedContexts.TourPricingManagement.Events
{
    public class TourStopPricingModelCreatedEvent : DomainEvent
    {
        public string Title { get; private set; }
        public string Description { get; private set; }

        public TourStopPricingModelCreatedEvent(Guid modelId, string title, string description)
        {
            AggregateId = modelId;
            Title = title;
            Description = description;
        }
    }
}
