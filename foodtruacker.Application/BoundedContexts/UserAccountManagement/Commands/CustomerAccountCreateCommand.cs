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
    public class CustomerAccountCreateCommand : IRequest<CommandResult>
    {
        public string Email { get; set; }
        public string PlainPassword { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }

    public class CustomerAccountCreateCommandHandler : IRequestHandler<CustomerAccountCreateCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Customer> _repository;
        private readonly IAuthenticationRepository _authRepository;
        public CustomerAccountCreateCommandHandler(IEventSourcingRepository<Customer> repository, IAuthenticationRepository authRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _authRepository = authRepository ?? throw new ArgumentNullException(nameof(authRepository));
        }

        public async Task<CommandResult> Handle(CustomerAccountCreateCommand command, CancellationToken cancellationToken)
        {
            if (await _authRepository.EmailAddressInUse(command.Email))
                return CommandResult.EmailInUse(command.Email);

            var hashedPassword = await _authRepository.GenerateHashedPassword(Guid.Empty, command.PlainPassword);

            try
            {
                var customer = new Customer(
                    userId: Guid.NewGuid(),
                    email: command.Email,
                    hashedPassword: hashedPassword,
                    firstname: command.Firstname,
                    lastname: command.Lastname
                );

                await _repository.SaveAsync(customer);
                
                return CommandResult.Success(customer.AggregateId);
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
