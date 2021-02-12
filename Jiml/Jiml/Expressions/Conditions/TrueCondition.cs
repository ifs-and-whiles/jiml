using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Conditions
{
    public class TrueCondition : ICondition
    {
        public ConditionResult Evaluate(
            JObject input)
        {
            return new ConditionResult.Success(
                true);
        }

        public override string ToString()
        {
            return "true";
        }
    }
}