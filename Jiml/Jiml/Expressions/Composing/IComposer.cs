using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing
{
    public interface IComposer
    {
        //todo should use Context class
        Composition Compose(
            JObject input,
            Path parentPath);
    }
}