using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.ByIndex.PickIndex
{
    public interface IConditionalPickIndexElementComposer
    {
        Option<PickIndexElementComposition> Compose(
            JObject input,
            Path parentPath,
            int index);
    }
}