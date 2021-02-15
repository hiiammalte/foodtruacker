using foodtruacker.Application.Results;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.Domain.Exceptions;
using foodtruacker.EventSourcingRepository.Repository;
using foodtruacker.Authentication.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands
{
    public class AdminAccountCreateCommand : IRequest<CommandResult>
    {
        public string Email { get; set; }
        public string PlainPassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string SecretProductKey { get; set; }
    }

    public class AdminAccountCreateCommandHandler : IRequestHandler<AdminAccountCreateCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Admin> _repository;
        private readonly IAuthenticationRepository _authRepository;
        public AdminAccountCreateCommandHandler(IEventSourcingRepository<Admin> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(AdminAccountCreateCommand command, CancellationToken cancellationToken)
        {
            if (await _authRepository.EmailAddressInUse(command.Email))
                return CommandResult.EmailInUse(command.Email);

            var hashedPassword = await _authRepository.GenerateHashedPassword(Guid.Empty, command.PlainPassword);

            try
            {
                var admin = new Admin(
                    userId: Guid.NewGuid(),
                    email: command.Email,
                    hashedPassword: hashedPassword,
                    firstname: command.Firstname,
                    lastname: command.Lastname
                );

                await _repository.SaveAsync(admin);

                return CommandResult.Success(admin.AggregateId);
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
