using Newtonsoft.Json.Linq;

namespace Jiml.Conditions
{
    public class NotCondition : ICondition
    {
        private readonly ICondition _innerCondition;

        public NotCondition(
            ICondition innerCondition)
        {
            _innerCondition = innerCondition;
        }

        public ConditionResult Evaluate(JObject input)
        {
            var innerResult = _innerCondition.Evaluate(
                input);

            return innerResult.Select(
                (success) => new ConditionResult.Success(!success.Value),
                (failure) => failure);
        }
    }
}