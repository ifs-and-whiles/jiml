using System;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Numbers
{
    public class PowerNumberComposer : INumberComposer
    {
        private readonly INumberComposer _x;
        private readonly INumberComposer _y;

        public PowerNumberComposer(
            INumberComposer x,
            INumberComposer y)
        {
            _x = x ?? throw new ArgumentNullException(nameof(x));
            _y = y ?? throw new ArgumentNullException(nameof(y));
        }

        public NumberComposition Compose(
            JObject input, 
            Path parentPath)
        {
            var xResult = _x.Compose(
                input,
                parentPath);

            if(!xResult.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            var yResult = _y.Compose(
                input,
                parentPath);

            if(!yResult.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            return new NumberComposition.Success(
                (decimal) Math.Pow(
                    (double) xResult.Value, 
                    (double) yResult.Value));
        }
    }
}