using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Numbers
{
    public interface INumberComposer
    {
        NumberComposition Compose(
            JObject input,
            Path parentPath);
    }
}
