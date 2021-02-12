using System;
using Jiml.Errors;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Numbers
{
    public class VariableNumberComposer : INumberComposer
    {
        private readonly IExtractor _extractor;

        public VariableNumberComposer(
            IExtractor extractor)
        {
            _extractor = extractor ?? throw new ArgumentNullException(nameof(extractor));
        }

        public NumberComposition Compose(
            JObject input, 
            Path parentPath)
        {
            var result = _extractor.ExtractFrom(
                Context.Root(input));

            if (!result.IsSuccess)
                return new NumberComposition.Failure(
                    new[] {result.Error});

            if (!IsNumber(result))
                return new NumberComposition.Failure(
                    new[]
                    {
                        new VariableWasNotANumber(
                            result.Path,
                            result.Value),
                    });

            var number = result
                .Value
                .ToObject<decimal>();
                
            return new NumberComposition.Success(
                number);
        }

        private static bool IsNumber(Extracting.Results.Extraction result)
        {
            return result.Value.Type == JTokenType.Integer ||
                   result.Value.Type == JTokenType.Float;
        }
    }
}