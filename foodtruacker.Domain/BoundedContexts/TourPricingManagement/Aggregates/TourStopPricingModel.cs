using foodtruacker.Domain.BoundedContexts.TourPricingManagement.Events;
using foodtruacker.SharedKernel;
using System;

namespace foodtruacker.Domain.BoundedContexts.TourPricingManagement.Aggregates
{
    public class TourStopPricingModel : AggregateRoot
    {
        private string Title { get; set; }
        private string Description { get; set; }

        public TourStopPricingModel()
        { }

        public TourStopPricingModel(Guid modelId, string title, string description)
        {
            if (modelId == Guid.Empty)
                throw new ArgumentNullException(nameof(modelId));

            AggregateId = modelId;
            Title = title;
            Description = description;

            RaiseEvent(new TourStopPricingModelCreatedEvent(AggregateId, Title, Description));
        }

        #region Aggregate Methods

        #endregion

        #region Event Handling
        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case TourStopPricingModelCreatedEvent x: OnTourStopPricingModelCreated(x); break;
            }
        }

        private void OnTourStopPricingModelCreated(TourStopPricingModelCreatedEvent @event)
        {
            AggregateId = @event.AggregateId;
            Title = @event.Title;
            Description = @event.Description;
        }
        #endregion
    }
}
