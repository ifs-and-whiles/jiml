using Jiml.Composing.Results;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing
{
    public interface IComposer
    {
        //todo should use Context class
        Composition Compose(
            JObject input,
            Path parentPath);
    }
}