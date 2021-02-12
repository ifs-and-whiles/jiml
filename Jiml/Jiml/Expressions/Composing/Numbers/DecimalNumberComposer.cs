using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Numbers
{
    public class IntegerNumberComposer : INumberComposer
    {
        private readonly long _value;

        public IntegerNumberComposer(
            long value)
        {
            _value = value;
        }

        public NumberComposition Compose(
            JObject input,
            Path parentPath)
        {
            return new NumberComposition.Success(
                _value);
        }
    }

    public class DecimalNumberComposer : INumberComposer
    {
        private readonly decimal _value;

        public DecimalNumberComposer(
            decimal value)
        {
            _value = value;
        }

        public NumberComposition Compose(
            JObject input, 
            Path parentPath)
        {
            return new NumberComposition.Success(
                _value);
        }
    }
}