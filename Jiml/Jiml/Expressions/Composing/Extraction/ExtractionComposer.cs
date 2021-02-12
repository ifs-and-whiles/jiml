using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Extraction
{
    public class ExtractionComposer: IComposer
    {
        private readonly IExtractor _extractor;

        public ExtractionComposer(
            IExtractor extractor)
        {
            _extractor = extractor;
        }
        
        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            var result = _extractor.ExtractFrom(
                Context.Root(input));

            if (result.IsSuccess)
                return new CorrectComposition(
                    result.Value);

            return new IncorrectComposition(
                parentPath,
                new[] {result.Error});
        }
    }
}
