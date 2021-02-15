using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Events;
using foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects;
using foodtruacker.Domain.Exceptions;
using foodtruacker.SharedKernel;
using System;
using System.Linq;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.Aggregates
{
    public class Admin : AggregateRoot
    {
        public UserFullName Name { get; private set; }
        public EmailAddress Email { get; private set; }
        public HashedPassword Password { get; private set; }
        public bool Verified { get; private set; }

        public Admin()
        { }

        public Admin(Guid userId, string email, string hashedPassword, string firstname, string lastname)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            AggregateId = userId;

            Email = (EmailAddress)CheckAndAssign(EmailAddress.Create(email));
            Password = (HashedPassword)CheckAndAssign(HashedPassword.Create(hashedPassword));
            Name = (UserFullName)CheckAndAssign(UserFullName.Create(firstname, lastname));

            if (_businessLogicErrors?.Any() == true)
                throw new DomainBusinessLogicException(_businessLogicErrors);

            RaiseEvent(new AdminAccountCreatedEvent(
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

            RaiseEvent(new AdminAccountChangedEmailEvent(
                userId: AggregateId,
                email: Email,
                fullName: Name
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

            RaiseEvent(new AdminAccountChangedPasswordEvent(
                userId: AggregateId,
                newPassword: validatedNewPassword
            ));
        }

        public void VerifyAccount()
        {
            Verified = true;

            if (_businessLogicErrors?.Any() == true)
                throw new DomainBusinessLogicException(_businessLogicErrors);

            RaiseEvent(new AdminAccountVerifiedEvent(
                userId: AggregateId,
                email: Email,
                name: Name,
                verified: Verified
            ));
        }
        #endregion

        #region Event Handling
        protected override void When(IDomainEvent @event)
        {
            switch (@event)
            {
                case AdminAccountCreatedEvent x: OnAccountCreatedChanged(x); break;
                case AdminAccountVerifiedEvent x: OnAccountVerificationChanged(x); break;
                case AdminAccountChangedEmailEvent x: OnEmailChanged(x); break;
                case AdminAccountChangedPasswordEvent x: OnPasswordChanged(x); break;
            }
        }
        
        private void OnAccountCreatedChanged(AdminAccountCreatedEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
            Password = @event.Password;
            Name = @event.Name;
        }

        private void OnAccountVerificationChanged(AdminAccountVerifiedEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
            Verified = @event.Verified;
            Name = @event.Name;
        }

        private void OnEmailChanged(AdminAccountChangedEmailEvent @event)
        {
            AggregateId = @event.AggregateId;
            Email = @event.Email;
            Name = @event.Name;
        }

        private void OnPasswordChanged(AdminAccountChangedPasswordEvent @event)
        {
            AggregateId = @event.AggregateId;
            Password = @event.NewPassword;
        }
        #endregion
    }
}
