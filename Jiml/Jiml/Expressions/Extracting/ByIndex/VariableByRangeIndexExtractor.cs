using System.Linq;
using Jiml.Expressions.Extracting.ByIndex.RangeIndex;
using Jiml.Expressions.Extracting.Results;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.ByIndex
{
    public class VariableByRangeIndexExtractor : JArrayExtractor
    {
        private readonly RangeIndex.RangeIndex _rangeIndex;

        public VariableByRangeIndexExtractor(
            RangeIndex.RangeIndex rangeIndex)
        {
            _rangeIndex = rangeIndex;
        }

        protected override Path GetPath(
            Context context)
        {
            return GetPath(
                context.Path);
        }

        protected override Extraction ExtractFromJArray(
            JArrayContext jArrayContext)
        {
            var rangeIndexComposition = _rangeIndex.Compose(
                jArrayContext.Input,
                jArrayContext.Path);

            var normalizedRangeIndex = NormalizedArrayRangeIndex.Normalize(
                rangeIndex: rangeIndexComposition,
                arrayLength: jArrayContext.Value.Count);
            
            var array = normalizedRangeIndex
                .AllIndexes()
                .Where(index => index >= 0 && index < jArrayContext.Value.Count)
                .Aggregate(
                    seed: new JArray(),
                    (acc, index) =>
                    {
                        acc.Add(jArrayContext.Value[index]);
                        return acc;
                    });

            var path = GetPath(
                jArrayContext.Path);

            return new CorrectExtraction(
                path,
                array);
        }

        private Path GetPath(
            Path previousPath)
        {
            return previousPath.Append(
                $"[{_rangeIndex}]");
        }
    }
}