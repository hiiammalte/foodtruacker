using foodtruacker.Application.Results;
using foodtruacker.Authentication.Repository;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.Domain.Exceptions;
using foodtruacker.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands
{
    public class AdminAccountChangeEmailCommand : IRequest<CommandResult>
    {
        public long ExpectedVersion { get; set; }
        public Guid UserId { get; set; }
        public string NewEmail { get; set; }
    }

    public class AdminAccountChangeEmailCommandHandler : IRequestHandler<AdminAccountChangeEmailCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Admin> _repository;
        private readonly IAuthenticationRepository _authRepository;
        public AdminAccountChangeEmailCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(AdminAccountChangeEmailCommand command, CancellationToken cancellationToken)
        {
            if (await _authRepository.EmailAddressInUse(command.NewEmail))
                return CommandResult.EmailInUse(command.NewEmail);

            try
            {
                var admin = await _repository.FindByIdAsync(command.UserId);
                if (admin is null)
                    return CommandResult.NotFound(command.UserId);

                if (admin.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);

                admin.ChangeEmail(command.NewEmail);

                await _repository.SaveAsync(admin);
                return CommandResult.Success();
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
