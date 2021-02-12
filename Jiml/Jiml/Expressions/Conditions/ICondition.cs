using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Conditions
{
    public interface ICondition
    {
        ConditionResult Evaluate(JObject input);
    }
}
