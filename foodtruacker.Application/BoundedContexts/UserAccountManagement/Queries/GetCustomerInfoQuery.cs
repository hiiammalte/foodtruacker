using foodtruacker.Application.BoundedContexts.UserAccountManagement.QueryObjects;
using foodtruacker.QueryRepository.Repository;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace foodtruacker.Application.BoundedContexts.UserAccountManagement.Queries
{
    public class GetCustomerInfoQuery : IRequest<CustomerInfo>
    {
        public Guid Id { get; private set; }
        public GetCustomerInfoQuery(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class GetCustomerInfoQueryHandler : IRequestHandler<GetCustomerInfoQuery, CustomerInfo>
    {
        private readonly IQueryRepository<CustomerInfo> _repository;

        public GetCustomerInfoQueryHandler(IQueryRepository<CustomerInfo> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<CustomerInfo> Handle(GetCustomerInfoQuery request, CancellationToken cancellationToken)
        {
            return await _repository.FindByIdAsync(request.Id);
        }
    }
}
