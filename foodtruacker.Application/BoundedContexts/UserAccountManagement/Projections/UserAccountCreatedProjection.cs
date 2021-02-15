using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.EmailService.Services;
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
        private readonly IEmailService _emailService;

        public UserAccountCreatedProjection(IAuthenticationRepository authRepository, IEmailService emailService)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        }

        public async Task Handle(AdminAccountCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _authRepository.CreateAdmin(userId: @event.AggregateId, email: @event.Email, password: @event.Password);

            await _emailService.SendEmailAddressConfirmationLink(
                userId: @event.AggregateId,
                userEmail: @event.Email,
                userName: @event.Name.ToString(),
                token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
            );
        }

        public async Task Handle(CustomerAccountCreatedEvent @event, CancellationToken cancellationToken)
        {
            await _authRepository.CreateCustomer(userId: @event.AggregateId, email: @event.Email, password: @event.Password);

            await _emailService.SendEmailAddressConfirmationLink(
                userId: @event.AggregateId,
                userEmail: @event.Email,
                userName: @event.Name.ToString(),
                token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
            );
        }
    }
}
