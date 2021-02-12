using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Numbers
{
    public class MultiplicationComposer : INumberComposer
    {
        private readonly INumberComposer _x;
        private readonly INumberComposer _y;

        public MultiplicationComposer(
            INumberComposer x,
            INumberComposer y)
        {
            _x = x;
            _y = y;
        }

        public NumberComposition Compose(
            JObject input, 
            Path parentPath)
        {
            var xResult = _x.Compose(
                input,
                parentPath);

            var yResult = _y.Compose(
                input,
                parentPath);

            return new NumberComposition.Success(
                xResult.GetDecimal() * yResult.GetDecimal());
        }
    }
}