using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Authentication.Repository;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.EmailService.Services;
using foodtruacker.QueryRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Projections
{
    public class CustomerAccountChangedEmailProjection : INotificationHandler<CustomerAccountChangedEmailEvent>
    {
        private readonly IAuthenticationRepository _authRepository;
        private readonly IEmailService _emailService;
        private readonly IProjectionRepository<CustomerInfo> _repository;

        public CustomerAccountChangedEmailProjection(IAuthenticationRepository authRepository, IEmailService emailService, IProjectionRepository<CustomerInfo> repository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(CustomerAccountChangedEmailEvent @event, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAddressConfirmationLink(
                userId: @event.AggregateId,
                userEmail: @event.Email,
                userName: @event.Name.ToString(),
                token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
            );

            var customer = await _repository.FindByIdAsync(@event.AggregateId);
            if (customer is not null)
            {
                customer.Version = @event.AggregateVersion;
                await _repository.UpdateAsync(customer);
            }
            else
            {
                
            }
        }
    }
}
