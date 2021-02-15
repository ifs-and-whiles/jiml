using Jiml.Expressions.Composing.Numbers;
using Jiml.Expressions.Errors;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Conditions.Numeric
{
    public abstract class NumericCondition : ICondition
    {
        private readonly INumberComposer _left;
        private readonly INumberComposer _right;

        public NumericCondition(
            INumberComposer left,
            INumberComposer right)
        {
            _left = left;
            _right = right;
        }

        public ConditionResult Evaluate(JObject input)
        {
            var left = _left.Compose(
                input,
                Path.Root);

            if (!left.IsSuccess)
                return new ConditionResult.Failure(new[]
                {
                    new ConditionFailed(
                        left.Errors),
                });

            var right = _right.Compose(
                input,
                Path.Root);

            if (!right.IsSuccess)
                return new ConditionResult.Failure(new[]
                {
                    new ConditionFailed(
                        left.Errors),
                });

            var leftNumber = left.GetDecimal();
            var rightNumber = right.GetDecimal();

            var conditionResult = EvaluateNumericCondition(
                leftNumber,
                rightNumber);

            return new ConditionResult.Success(conditionResult);
        }

        protected abstract bool EvaluateNumericCondition(
            decimal left, 
            decimal right);
    }
}