using System.Collections.Generic;
using System.Linq;
using Jiml.Expressions.Extracting.Results;

namespace Jiml.Expressions.Extracting
{
    public class ExtractorComposite : IExtractor
    {
        private readonly List<IExtractor> _extractors;

        public ExtractorComposite(
            IEnumerable<IExtractor> extractors)
        {
            _extractors = extractors.ToList();
        }

        public Extraction ExtractFrom(
            Context context)
        {
            Extraction current = null;

            foreach (var extractor in _extractors)
            {
                var contentToExtractFrom = current == null
                    ? context
                    : new Context(
                        context.Input,
                        current.Value, 
                        current.Path);

                current = extractor.ExtractFrom(
                    contentToExtractFrom);

                if (!current.IsSuccess)
                    return current;
            }

            return current;
        }
    }
}