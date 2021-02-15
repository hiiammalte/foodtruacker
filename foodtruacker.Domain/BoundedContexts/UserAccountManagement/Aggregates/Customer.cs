using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.Domain.Exceptions;
using foodtruacker.SharedKernel;
using System;
using System.Linq;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates
{
    public class Customer : AggregateRoot
    {
        public UserFullName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public HashedPassword Password { get; private set; }
        public bool Verified { get; private set; }

        public Customer()
        { }

        public Customer(Guid userId, string email, string hashedPassword, string firstname, string lastname)
        {
            //TODO: Implement as BusinessRule
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            AggregateId = userId;

            Email = (EmailAddress)CheckAndAssign(EmailAddress.Create(email));
            Password = (HashedPassword)CheckAndAssign(HashedPassword.Create(hashedPassword));
            Name = (UserFullName)CheckAndAssign(UserFullName.Create(firstname, lastname));

            if (_businessLogicErrors?.Any() == true)
                throw new DomainBusinessLogicException(_businessLogicErrors);

            RaiseEvent(new CustomerAccountCreatedEvent(
                userId: AggregateId,
                email: Email,
                password: Password,
                name: Name
            ));
        }

        #region Aggregate Methods
        public void ChangeEmail(string email)
        {
            Email = (EmailAddress)CheckAndAssign(EmailAddress.Create(email));
            Verified = false;

            if (_businessLogicErrors?.Any() == true)
                throw new DomainBusinessLogicException(_businessLogicErrors);

            RaiseEvent(new CustomerAccountChangedEmailEvent(
                userId: AggregateId,
                email: Email,
                name: Name
            ));
        }

        public void VerifyAccount()
        {
            Verified = true;

            RaiseEvent(new CustomerAccountVerifiedEvent(
                userId: AggregateId,
                email: Email,
                name: Name,
                verified: Verified
            ));
        }

        public void ChangePassword(string currentPassword, string newPassword)
        {
            var validatedNewPassword = (HashedPassword)CheckAndAssign(HashedPassword.Create(newPassword));
            var validatedCurrentPassword = (HashedPassword)CheckAndAssign(HashedPassword.Create(currentPassword));

            if (Password.EqualsCurrentPassword(validatedCurrentPassword))
                Password = validatedNewPassword;

            if (_businessLogicErrors?.Any() == true)
                throw new DomainBusinessLogicException(_businessLogicErrors);

            RaiseEvent(new CustomerAccountChangedPasswordEvent(
                userId: AggregateId,
                newPassword: validatedNewPassword
            ));
        }
        #endregion

        #region Event Handling
        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case CustomerAccountCreatedEvent x: OnAccountCreatedChanged(x); break;
                case CustomerAccountVerifiedEvent x: OnAccountVerificationChanged(x); break;
                case CustomerAccountChangedEmailEvent x: OnEmailChanged(x); break;
                case CustomerAccountChangedPasswordEvent x: OnPasswordChanged(x); break;
            }
        }

        private void OnAccountCreatedChanged(CustomerAccountCreatedEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
            Password = @event.Password;
            Name = @event.Name;
        }

        private void OnAccountVerificationChanged(CustomerAccountVerifiedEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
            Verified = @event.Verified;
            Name = @event.Name;
        }

        private void OnEmailChanged(CustomerAccountChangedEmailEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
        }

        private void OnPasswordChanged(CustomerAccountChangedPasswordEvent @event)
        {
            AggregateId = @event.AggregateId;
            Password = @event.NewPassword;
        }
        #endregion
    }
}
