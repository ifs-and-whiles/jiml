using System;
using System.Collections.Generic;
using System.Linq;

namespace Jiml.Conditions
{
    public abstract class ConditionResult
    {
        public bool Value { get; }
        public bool IsSuccess { get; }
        public Jiml.Error[] Errors { get; }

        protected ConditionResult(
            bool value)
        {
            Value = value;
            IsSuccess = true;
        }

        protected ConditionResult(
            IEnumerable<Jiml.Error> errors)
        {
            Errors = errors.ToArray();
        }

        public abstract ConditionResult Select(
            Func<Success, ConditionResult> successFunc,
            Func<Failure, ConditionResult> failureFunc);

        public class Success: ConditionResult
        {
            public Success(
                bool value) : 
                base(value)
            {
            }

            public override ConditionResult Select(
                Func<Success, ConditionResult> successFunc,
                Func<Failure, ConditionResult> failureFunc)
            {
                return successFunc(this);
            }
        }

        public class Failure: ConditionResult
        {
            public Failure(
                IEnumerable<Jiml.Error> errors) :
                base(errors)
            {
            }

            public override ConditionResult Select(
                Func<Success, ConditionResult> successFunc,
                Func<Failure, ConditionResult> failureFunc)
            {
                return failureFunc(this);
            }
        }
    }
}