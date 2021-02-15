using System;
using System.Collections.Generic;
using System.Linq;

namespace foodtruacker.Application.Results
{
    public class CommandResult
    {
        private CommandResult()
        { }
        private CommandResult(object successObject)
        {
            SuccessObject = successObject;
        }
        private CommandResult(FailureTypes failureType, string failureReason)
        {
            FailureType = failureType;
            FailureReasons ??= new();
            FailureReasons.Add(failureReason);
        }
        private CommandResult(FailureTypes failureType, List<string> failureReasons)
        {
            FailureType = failureType;
            FailureReasons ??= new();
            FailureReasons.AddRange(failureReasons);
        }
        private CommandResult(FailureTypes failureType, Guid aggregateId)
        {
            FailureType = failureType;
            FailureReasons ??= new();
            FailureReasons.Add($"Cannot find: {aggregateId}");
        }

        public object SuccessObject { get; private set; }
        public List<string> FailureReasons { get; private set; }
        public FailureTypes FailureType { get; private set; }
        public bool IsSuccess => !(FailureReasons?.Any() == true);

        public static CommandResult Success()
            => new CommandResult();
        public static CommandResult Success(object successObject)
            => new CommandResult(successObject);
        public static CommandResult BusinessFail(string reason)
            => new CommandResult(FailureTypes.BusinessRule, reason);
        public static CommandResult BusinessFail(List<string> reasons)
            => new CommandResult(FailureTypes.BusinessRule, reasons);
        public static CommandResult NotFound(Guid aggregateId)
            => new CommandResult(FailureTypes.NotFound, aggregateId);
        public static CommandResult EmailInUse(string email)
            => new CommandResult(FailureTypes.Duplicate, $"Provided email address has already been linked to another account: {email}");
    }
}
