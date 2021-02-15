using foodtruacker.SharedKernel;
using System;
using System.Threading.Tasks;

namespace foodtruacker.EventSourcingRepository.Repository
{
    public interface IEventSourcingRepository<TAggregate> where TAggregate : IAggregateRoot
    {
        Task<TAggregate> FindByIdAsync(Guid id);
        Task SaveAsync(TAggregate aggregate);
    }
}
