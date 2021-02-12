using Jiml.Composing.Results;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Constant
{
    public class BoolComposer: IComposer
    {
        private readonly bool _value;

        public BoolComposer(
            bool value)
        {
            _value = value;
        }

        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            return new CorrectComposition(
                new JValue(_value));
        }
    }
}
