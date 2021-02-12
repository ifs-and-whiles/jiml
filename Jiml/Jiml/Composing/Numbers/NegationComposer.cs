using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Numbers
{
    public class NegationComposer : INumberComposer
    {
        private readonly INumberComposer _x;

        public NegationComposer(
            INumberComposer x)
        {
            _x = x;
        }

        public NumberComposition Compose(
            JObject input, 
            Path parentPath)
        {
            var xResult = _x.Compose(
                input,
                parentPath);

            return new NumberComposition.Success(
                xResult.GetDecimal() * -1);
        }
    }
}