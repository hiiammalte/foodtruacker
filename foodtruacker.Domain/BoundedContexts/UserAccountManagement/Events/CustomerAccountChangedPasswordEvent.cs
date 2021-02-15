using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.SharedKernel;
using System;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events
{
    public class CustomerAccountChangedPasswordEvent : DomainEvent
    {
        public HashedPassword NewPassword { get; protected set; }

        private CustomerAccountChangedPasswordEvent()
        { }

        public CustomerAccountChangedPasswordEvent(Guid userId, HashedPassword newPassword)
        {
            AggregateId = userId;
            NewPassword = newPassword;
        }
    }
}
