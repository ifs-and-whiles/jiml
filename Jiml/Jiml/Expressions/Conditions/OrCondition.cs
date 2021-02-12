using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Conditions
{
    public class OrCondition : ICondition
    {
        private readonly ICondition _left;
        private readonly ICondition _right;

        public OrCondition(
            ICondition left,
            ICondition right)
        {
            _left = left;
            _right = right;
        }

        public ConditionResult Evaluate(JObject input)
        {
            var leftResult = _left.Evaluate(
                input);

            return leftResult.Select(
                (leftSuccess) =>
                {
                    if (leftSuccess.Value)
                        return new ConditionResult.Success(true);

                    return _right.Evaluate(
                        input);
                },
                (failure) => failure);
        }
    }
}