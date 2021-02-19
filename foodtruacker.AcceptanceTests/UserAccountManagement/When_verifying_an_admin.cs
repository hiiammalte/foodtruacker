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
    public class When_verifying_an_admin : Specification<Admin, AdminAccountVerifyCommand, CommandResult>
    {
        private readonly Guid _UserId = Guid.NewGuid();
        private readonly string _Email = "test@user.com";
        private readonly string _PlainPassword = "123456";
        private readonly string _Firstname = "Test";
        private readonly string _Lastname = "User";

        protected override IRequestHandler<AdminAccountVerifyCommand, CommandResult> CommandHandler()
            => new AdminAccountVerifyCommandHandler(MockEventSourcingRepository.Object);

        protected override ICollection<IDomainEvent> GivenEvents()
            => new List<IDomainEvent>()
            {
                new AdminAccountCreatedEvent
                (
                    userId: _UserId,
                    email: (EmailAddress)EmailAddress.Create(_Email).ValueObject,
                    password: (HashedPassword)HashedPassword.Create(_PlainPassword).ValueObject,
                    name: (UserFullName)UserFullName.Create(_Firstname, _Lastname).ValueObject
                )
            };

        protected override AdminAccountVerifyCommand When()
            => new AdminAccountVerifyCommand
            {
                UserId = _UserId
            };

        [Then]
        public void Then_a_AdminAccountVerifiedEvent_will_be_published()
        {
            PublishedEvents.Last().As<AdminAccountVerifiedEvent>().AggregateId.Should().Be(_UserId);
            PublishedEvents.Last().As<AdminAccountVerifiedEvent>().Name.ToString().Should().Be($"{_Firstname} {_Lastname}");
            PublishedEvents.Last().As<AdminAccountVerifiedEvent>().Email.ToString().Should().Be(_Email);
            PublishedEvents.Last().As<AdminAccountVerifiedEvent>().Verified.Should().BeTrue();
        }
    }
}
