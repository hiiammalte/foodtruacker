using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.QueryRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Projections
{
    public class CustomerAccountVerifiedProjection : INotificationHandler<CustomerAccountVerifiedEvent>
    {
        private readonly IProjectionRepository<CustomerInfo> _repository;

        public CustomerAccountVerifiedProjection(IProjectionRepository<CustomerInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CustomerAccountVerifiedEvent @event, CancellationToken cancellationToken)
        {
            var customer = await _repository.FindByIdAsync(@event.AggregateId);
            if (customer == null)
            {
                await _repository.InsertAsync(new CustomerInfo
                {
                    Id = @event.AggregateId,
                    Email = @event.Email.Value,
                    FirstName = @event.Name.First,
                    LastName = @event.Name.Last,
                    Version = @event.AggregateVersion
                });
            }
            else
            {
                customer.Email = @event.Email.Value;
                customer.Version = @event.AggregateVersion;

                await _repository.UpdateAsync(customer);
            }
        }
    }
}
