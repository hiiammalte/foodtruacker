using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.SharedKernel;
using System;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events
{
    public class AdminAccountChangedEmailEvent : DomainEvent
    {
        public EmailAddress Email { get; protected set; }
        public UserFullName Name { get; protected set; }

        private AdminAccountChangedEmailEvent()
        { }

        public AdminAccountChangedEmailEvent(Guid userId, EmailAddress email, UserFullName fullName)
        {
            AggregateId = userId;
            Email = email;
            Name = fullName;
        }
    }
}
