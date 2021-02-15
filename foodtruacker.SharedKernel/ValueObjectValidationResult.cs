using System.Collections.Generic;

namespace foodtruacker.SharedKernel
{
    public class ValueObjectValidationResult
    {
        public IValueObject ValueObject { get; private set; }
        public List<string> BusinessErrors { get; private set; }

        public ValueObjectValidationResult(IValueObject valueObject, List<string> businessErrors)
        {
            ValueObject = valueObject;
            BusinessErrors = businessErrors;
        }
    }
}
