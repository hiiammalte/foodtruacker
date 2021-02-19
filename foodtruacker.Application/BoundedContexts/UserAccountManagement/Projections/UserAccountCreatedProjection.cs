using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.Authentication.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Projections
{
    public class UserAccountCreatedProjection : INotificationHandler<AdminAccountCreatedEvent>, INotificationHandler<CustomerAccountCreatedEvent>
    {
        private readonly IAuthenticationRepository _authRepository;

        public UserAccountCreatedProjection(IAuthenticationRepository authRepository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task Handle(AdminAccountCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _authRepository.CreateAdmin(userId: @event.AggregateId, email: @event.Email, password: @event.Password);
        }

        public async Task Handle(CustomerAccountCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _authRepository.CreateCustomer(userId: @event.AggregateId, email: @event.Email, password: @event.Password);
        }
    }
}
