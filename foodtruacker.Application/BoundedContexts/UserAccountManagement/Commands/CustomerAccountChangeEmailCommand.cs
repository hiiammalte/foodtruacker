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
    public class CustomerAccountChangeEmailCommand : IRequest<CommandResult>
    {
        public long ExpectedVersion { get; set; }
        public Guid UserId { get; set; }
        public string NewEmail { get; set; }
    }

    public class CustomerAccountChangeEmailCommandHandler : IRequestHandler<CustomerAccountChangeEmailCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Customer> _repository;
        private readonly IAuthenticationRepository _authRepository;
        public CustomerAccountChangeEmailCommandHandler(IEventSourcingRepository<Customer> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(CustomerAccountChangeEmailCommand command, CancellationToken cancellationToken)
        {
            if (await _authRepository.EmailAddressInUse(command.NewEmail))
                return CommandResult.EmailInUse(command.NewEmail);

            try
            {
                var customer = await _repository.FindByIdAsync(command.UserId);
                if (customer is null)
                    return CommandResult.NotFound(command.UserId);

                if (customer.Version != command.ExpectedVersion)
                    return CommandResult.NotFound(command.UserId);

                customer.ChangeEmail(command.NewEmail);

                await _repository.SaveAsync(customer);
                return CommandResult.Success();
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
