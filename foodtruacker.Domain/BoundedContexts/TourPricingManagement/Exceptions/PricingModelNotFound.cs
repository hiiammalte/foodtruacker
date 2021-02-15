using foodtruacker.Domain.Exceptions;

namespace foodtruacker.Domain.BoundedContexts.TourPricingManagement.Exceptions
{
    class PricingModelNotFound : DomainException
    {
        public PricingModelNotFound() : base("Pricing Model not found.") { }
    }
}
