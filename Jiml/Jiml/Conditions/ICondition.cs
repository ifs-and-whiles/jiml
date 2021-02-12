using Newtonsoft.Json.Linq;

namespace Jiml.Conditions
{
    public interface ICondition
    {
        ConditionResult Evaluate(JObject input);
    }
}
