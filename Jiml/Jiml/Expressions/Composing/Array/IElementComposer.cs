using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Array
{
    public interface IElementComposer
    {
        ElementComposition Compose(
            JObject input,
            Path parentPath,
            int index);
    }
}