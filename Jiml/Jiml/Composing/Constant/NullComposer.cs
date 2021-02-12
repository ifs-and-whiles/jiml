using Jiml.Composing.Results;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Constant
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