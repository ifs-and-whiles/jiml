using System;
using Jiml.Expressions.Composing.Numbers;
using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Constant
{
    public class NumberComposer : IComposer
    {
        private readonly INumberComposer _number;

        public NumberComposer(
            INumberComposer number)
        {
            _number = number ?? throw new ArgumentNullException(nameof(number));
        }
        
        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            var number = _number.Compose(
                input,
                parentPath);

            if(!number.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            return new CorrectComposition(
                new JValue(number.Value));
        }
    }
}