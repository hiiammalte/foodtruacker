using System;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.SharedKernel
{
    public abstract class ValueObject : IValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            
            if (GetType() != obj.GetType())
                throw new ArgumentException($"Invalid comparison of Value Objects of different types: {GetType()} and {obj.GetType()}");
            
            var valueObject = (ValueObject)obj;
            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode() =>
            GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    return HashCode.Combine(current, obj);
                });

        public static bool operator == (ValueObject a, ValueObject b)
        {
            if (a is null && b is null)
                return true;
            
            if (a is null || b is null)
                return false;
            
            return a.Equals(b);
        }
        public static bool operator != (ValueObject a, ValueObject b) => !(a == b);
    }
}
