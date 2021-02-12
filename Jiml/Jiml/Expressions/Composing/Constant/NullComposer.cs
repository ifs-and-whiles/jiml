using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Constant
{
    public class NullComposer : IComposer
    {
        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            return new CorrectComposition(
                JValue.CreateNull());
        }
    }
}