using Jiml.Expressions.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Array
{
    public interface IConditionalElementComposer
    {
        Option<ElementComposition> Compose(
            JObject input,
            Path parentPath,
            int index);
    }
}