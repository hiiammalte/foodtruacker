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
    class AdminAccountChangedEmailProjection : INotificationHandler<AdminAccountChangedEmailEvent>
    { 
        private readonly IAuthenticationRepository _authRepository;
        private readonly IEmailService _emailService;
        private readonly IProjectionRepository<AdminInfo> _repository;

        public AdminAccountChangedEmailProjection(IAuthenticationRepository authRepository, IEmailService emailService, IProjectionRepository<AdminInfo> repository)
        {
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(AdminAccountChangedEmailEvent @event, CancellationToken cancellationToken)
        {
            await _emailService.SendEmailAddressConfirmationLink(
                userId: @event.AggregateId,
                userEmail: @event.Email,
                userName: @event.Name.ToString(),
                token: await _authRepository.GenerateEmailValidationToken(@event.AggregateId)
            );

            var admin = await _repository.FindByIdAsync(@event.AggregateId);
            if (admin is not null)
            {
                admin.Version = @event.AggregateVersion;
                await _repository.UpdateAsync(admin);
            }
            else
            {
                
            }
        }
    }
}
