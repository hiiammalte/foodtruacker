using foodtruacker.Application.Results;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.Domain.Exceptions;
using foodtruacker.EventSourcingRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands
{
    public class AdminAccountVerifyCommand : IRequest<CommandResult>
    {
        public Guid Id { get; set; }
    }

    public class AdminAccountVerifyCommandHandler : IRequestHandler<AdminAccountVerifyCommand, CommandResult>
    {
        private readonly IEventSourcingRepository<Admin> _repository;
        public AdminAccountVerifyCommandHandler(IEventSourcingRepository<Admin> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CommandResult> Handle(AdminAccountVerifyCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var admin = await _repository.FindByIdAsync(command.Id);
                if (admin is null)
                    return CommandResult.NotFound(command.Id);

                admin.VerifyAccount();

                await _repository.SaveAsync(admin);
                return CommandResult.Success(command.Id);
            }
            catch (DomainException ex)
            {
                return CommandResult.BusinessFail(ex.Message);
            }
        }
    }
}
