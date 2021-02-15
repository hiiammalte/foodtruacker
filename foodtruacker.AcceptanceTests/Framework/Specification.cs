using foodtruacker.EventSourcingRepository.Repository;
using foodtruacker.Authentication.Entities;
using foodtruacker.Authentication.Repository;
using foodtruacker.SharedKernel;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;

namespace foodtruacker.AcceptanceTests.Framework
{
    public abstract class Specification<TAggregateRoot, TCommand, TReturnType>
        where TAggregateRoot : IAggregateRoot, new()
        where TCommand : IRequest<TReturnType>
    {
        protected Mock<IAuthenticationRepository> MockIdentityRepository;
        protected Mock<IEventSourcingRepository<TAggregateRoot>> MockEventSourcingRepository;
        protected abstract IRequestHandler<TCommand, TReturnType> CommandHandler();
        
        protected TAggregateRoot AggregateRoot;
        protected IEnumerable<IDomainEvent> PublishedEvents;
        protected Exception CaughtException;

        protected virtual IEnumerable<User> GivenIdentityUsers()
            => new List<User>();

        protected virtual IEnumerable<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>();

        protected abstract TCommand When();

        protected Specification()
        {
            MockIdentityRepository = new Mock<IAuthenticationRepository>();
            MockIdentityRepository.Setup(x => x.CreateAdmin(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));
            MockIdentityRepository.Setup(x => x.CreateCustomer(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()));
            MockIdentityRepository.Setup(x => x.VerifyEmail(It.IsAny<Guid>(), It.IsAny<string>()));
            MockIdentityRepository.Setup(x => x.ChangePassword(It.IsAny<Guid>(), It.IsAny<string>()));
            MockIdentityRepository.Setup(x => x.EmailAddressInUse(It.IsAny<string>())).ReturnsAsync(false);
            MockIdentityRepository.Setup(x => x.GenerateHashedPassword(It.IsAny<Guid>(), It.IsAny<string>())).ReturnsAsync("hashedPassword");
            MockIdentityRepository.Setup(x => x.GenerateEmailValidationToken(It.IsAny<Guid>())).ReturnsAsync("token");
            MockIdentityRepository.Setup(x => x.GenerateJWT(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("token");

            MockEventSourcingRepository = new Mock<IEventSourcingRepository<TAggregateRoot>>();
            MockEventSourcingRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(AggregateRoot);
            MockEventSourcingRepository.Setup(x => x.SaveAsync(It.IsAny<TAggregateRoot>()))
                .Callback<TAggregateRoot>((x) => AggregateRoot = x);

            AggregateRoot = new TAggregateRoot();
            AggregateRoot.LoadFromHistory(-1, GivenEvents());

            try
            {
                CommandHandler().Handle(When(), CancellationToken.None);
                PublishedEvents = new List<IDomainEvent>(AggregateRoot.GetUncommittedChanges());
            }
            catch (Exception exception)
            {
                CaughtException = exception;
            }
        }
    }
}
