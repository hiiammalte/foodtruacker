using foodtruacker.Domain.BoundedContexts.UserAccountManagement.Exceptions;
using foodtruacker.SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        public string Value { get; private set; }

        private EmailAddress()
        { }

        private EmailAddress(string value)
        {
            Value = value;
        }

        public static ValueObjectValidationResult Create(string value)
        {
            List<string> businessLogicErrors = new();

            if (value is null || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)) businessLogicErrors.Add("Email address is invalid.");

            if (businessLogicErrors?.Any() == true)
                return new ValueObjectValidationResult(null, businessLogicErrors);

            return new ValueObjectValidationResult(new EmailAddress(value), null);
        }

        public void ConfirmIsEqual(EmailAddress newValue)
        {
            if (Value != newValue)
                throw new ConfirmationEmailNotEqualException();
        }


        #region Conversion
        public static implicit operator string(EmailAddress value)
            => value.Value;
        #endregion

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
