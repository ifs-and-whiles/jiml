using Jiml.Composing.Results;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Constant
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