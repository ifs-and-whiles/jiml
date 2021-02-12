using Jiml.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Array
{
    public interface IConditionalElementComposer
    {
        Option<ElementComposition> Compose(
            JObject input,
            Path parentPath,
            int index);
    }
}