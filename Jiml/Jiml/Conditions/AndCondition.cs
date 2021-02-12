using Newtonsoft.Json.Linq;

namespace Jiml.Conditions
{
    public class AndCondition: ICondition
    {
        private readonly ICondition _left;
        private readonly ICondition _right;

        public AndCondition(
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
                    if(!leftSuccess.Value)
                        return new ConditionResult.Success(false);

                    return _right.Evaluate(
                        input);
                },
                (failure) => failure);
        }
    }
}