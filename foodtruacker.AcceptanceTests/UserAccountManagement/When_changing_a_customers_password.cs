using FluentAssertions;
using foodtruacker.AcceptanceTests.Framework;
using foodtruacker.Application.BoundedContexts.UserAccountManagement.Commands;
using foodtruacker.Application.Results;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.SharedKernel;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.AcceptanceTests.UserAccountManagement
{
    public class When_changing_a_customers_password : Specification<Customer, CustomerAccountChangePasswordCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _CurrentPassword = "123456";
        private readonly string _NewPlainPassword = "654321";

        protected override IRequestHandler<CustomerAccountChangePasswordCommand, CommandResult> CommandHandler()
            => new CustomerAccountChangePasswordCommandHandler(
                MockEventSourcingRepository.Object,
                MockIdentityRepository.Object
            );

        protected override ICollection<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>()
            {
                new CustomerAccountCreatedEvent
                (
                    userId: _UserId,
                    email: (EmailAddress)EmailAddress.Create("test@user.com").ValueObject,
                    password: (HashedPassword)HashedPassword.Create(_CurrentPassword).ValueObject,
                    name: (UserFullName)UserFullName.Create("Test", "User").ValueObject
                )
            };

        protected override CustomerAccountChangePasswordCommand When()
            => new CustomerAccountChangePasswordCommand
            {
                UserId = _UserId,
                CurrentPassword = _CurrentPassword,
                NewPassword = _NewPlainPassword,
                ExpectedVersion = 0
            };

        [Then]
        public void Then_a_CustomerAccountChangedPasswordEvent_will_be_published()
        {
            PublishedEvents.Last().As<CustomerAccountChangedPasswordEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<CustomerAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBeNullOrEmpty();
            PublishedEvents.Last().As<CustomerAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBe(_NewPlainPassword);
            PublishedEvents.Last().As<CustomerAccountChangedPasswordEvent>().NewPassword.ToString().Should().NotBe(_CurrentPassword);
        }
    }
}