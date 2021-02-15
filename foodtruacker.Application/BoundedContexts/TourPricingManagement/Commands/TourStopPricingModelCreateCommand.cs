using foodtruacker.Application.Results;
using foodtruacker.Domain.BoundedContexts.TourPricingManagement.Aggregates;
using foodtruacker.Domain.Exceptions;
using foodtruacker.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.TourPricingManagement.Commands
{
    public class TourStopPricingModelCreateCommand : IRequest<CommandResult>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class TourStopPricingModelCreateCommandHandler : IRequestHandler<TourStopPricingModelCreateCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<TourStopPricingModel> _repository;
        public TourStopPricingModelCreateCommandHandler(IEventSourcingRepository<TourStopPricingModel> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CommandResult> Handle(TourStopPricingModelCreateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var pricingModelId = Guid.NewGuid();

                var pricingModel = new TourStopPricingModel(pricingModelId, command.Title, command.Description);
                await _repository.SaveAsync(pricingModel);

                return CommandResult.Success(pricingModelId);
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
