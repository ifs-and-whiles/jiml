using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Constant
{
    public class StringComposer : IComposer
    {
        private readonly string _value;

        public StringComposer(
            string value)
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