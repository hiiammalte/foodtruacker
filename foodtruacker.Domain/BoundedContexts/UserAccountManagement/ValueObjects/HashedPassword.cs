
using foodtruacker.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects
{
    public class HashedPassword : ValueObject
    {
        public string Value { get; private set; }

        private HashedPassword()
        { }

        private HashedPassword(string value)
        {
            Value = value;
        }

        public static ValueObjectValidationResult Create(string value)
        {
            List<string> businessLogicErrors = new();

            if (value is null) businessLogicErrors.Add("Hashed Password is invalid.");

            if (businessLogicErrors?.Any() == true)
                return new ValueObjectValidationResult(null, businessLogicErrors);

            return new ValueObjectValidationResult(new HashedPassword(value), null);
        }

        public bool EqualsCurrentPassword(HashedPassword toComparePasswort)
            => (Value == toComparePasswort);

        #region Conversion
        public static implicit operator string(HashedPassword value)
            => value.Value;
        #endregion


        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
