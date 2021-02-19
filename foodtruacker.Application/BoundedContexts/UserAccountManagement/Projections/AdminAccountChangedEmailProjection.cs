using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.QueryRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Projections
{
    public class AdminAccountChangedEmailProjection : INotificationHandler<AdminAccountChangedEmailEvent>
    { 
        private readonly IProjectionRepository<AdminInfo> _repository;

        public AdminAccountChangedEmailProjection(IProjectionRepository<AdminInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(AdminAccountChangedEmailEvent @event, CancellationToken cancellationToken)
        {
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
