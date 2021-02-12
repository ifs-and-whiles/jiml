using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Constant
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
