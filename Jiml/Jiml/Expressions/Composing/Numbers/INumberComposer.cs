using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Numbers
{
    public interface INumberComposer
    {
        NumberComposition Compose(
            JObject input,
            Path parentPath);
    }
}
