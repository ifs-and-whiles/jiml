using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Array
{
    public interface IElementComposer
    {
        ElementComposition Compose(
            JObject input,
            Path parentPath,
            int index);
    }
}