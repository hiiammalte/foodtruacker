using foodtruacker.SharedKernel;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.Domain.BoundedContexts.UserAccountManagement.ValueObjects
{
    public class UserFullName : ValueObject
    {
        public string First { get; private set; }
        public string Last { get; private set; }

        private UserFullName()
        { }

        private UserFullName(string first, string last)
        {
            First = first;
            Last = last;
        }

        public static ValueObjectValidationResult Create(string first, string last)
        {
            List<string> businessLogicErrors = new();

            if (string.IsNullOrWhiteSpace(first)) businessLogicErrors.Add("First name is invalid.");
            if (string.IsNullOrWhiteSpace(last)) businessLogicErrors.Add("Last name is invalid.");

            if (businessLogicErrors?.Any() == true)
                return new ValueObjectValidationResult(null, businessLogicErrors);

            return new ValueObjectValidationResult(new UserFullName(first, last), null);
        }

        public override string ToString() => $"{First} {Last}";
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return First;
            yield return Last;
        }
    }
}
