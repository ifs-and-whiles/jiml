using Newtonsoft.Json.Linq;

namespace Jiml
{
    public interface IJimlExpression
    {
        Jiml.Result Evaluate(JObject input);
    }
}