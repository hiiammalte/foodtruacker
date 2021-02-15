using FluentAssertions;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands;
using foodtruacker.Application.Results;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.Authentication.Entities;
using foodtruacker.SharedKernel;
using foodtruacker.AcceptanceTests.Framework;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.AcceptanceTests.UserAccountManagement
{
    public class When_registering_a_new_Customer : Specification<Customer, CustomerAccountCreateCommand, CommandResult>
    {
        private readonly string _Email = "test@user.com";
        private readonly string _PlainPassword = "123456";
        private readonly string _Firstname = "Test";
        private readonly string _Lastname = "User";

        protected override IRequestHandler<CustomerAccountCreateCommand, CommandResult> CommandHandler()
            => new CustomerAccountCreateCommandHandler(
                MockEventSourcingRepository.Object,
                MockIdentityRepository.Object
            );

        protected override IEnumerable<User> GivenIdentityUsers()
            => new List<User>();

        protected override IEnumerable<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>();

        protected override CustomerAccountCreateCommand When()
            => new CustomerAccountCreateCommand
            {
                Email = _Email,
                PlainPassword = _PlainPassword,
                Firstname = _Firstname,
                Lastname = _Lastname
            };

        [Then]
        public void Then_a_CustomerAccountCreatedEvent_will_be_published()
        {
            PublishedEvents.Last().As<CustomerAccountCreatedEvent>().AggregateId.Should().NotBe(Guid.Empty);
            PublishedEvents.Last().As<CustomerAccountCreatedEvent>().Name.ToString().Should().Be($"{_Firstname} {_Lastname}");
            PublishedEvents.Last().As<CustomerAccountCreatedEvent>().Email.ToString().Should().Be(_Email);
            PublishedEvents.Last().As<CustomerAccountCreatedEvent>().Password.ToString().Should().NotBeNullOrWhiteSpace();
            PublishedEvents.Last().As<CustomerAccountCreatedEvent>().Password.ToString().Should().NotBe(_PlainPassword);
        }
    }
}
