using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.SharedKernel;
using System;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events
{
    public class AdminAccountCreatedEvent : DomainEvent
    {
        public EmailAddress Email { get; private set; }
        public HashedPassword Password { get; private set; }
        public UserFullName Name { get; private set; }

        private AdminAccountCreatedEvent()
        { }

        public AdminAccountCreatedEvent(Guid userId, EmailAddress email, HashedPassword password, UserFullName name)
        {
            AggregateId = userId;
            Email = email;
            Password = password;
            Name = name;
        }
    }
}
