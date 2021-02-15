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
    public class AdminAccountChangePasswordCommand : IRequest<CommandResult>
    {
        public long ExpectedVersion { get; set; }
        public Guid UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class AdminAccountChangePasswordCommandHandler : IRequestHandler<AdminAccountChangePasswordCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Admin> _repository;
        private readonly IAuthenticationRepository _authRepository;

        public AdminAccountChangePasswordCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(AdminAccountChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var admin = await _repository.FindByIdAsync(command.UserId);
                if (admin is null)
                    return CommandResult.NotFound(command.UserId);

                if (admin.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);


                if (!await _authRepository.IsCurrentPassword(command.UserId, command.CurrentPassword))
                    return CommandResult.BusinessFail("Invalid Password.");

                admin.ChangePassword(
                    currentPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.CurrentPassword),
                    newPassword: await _authRepository.GenerateHashedPassword(command.UserId, command.NewPassword)
                );

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
