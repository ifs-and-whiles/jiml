using System.Collections.Generic;
using System.Linq;
using Jiml.Extracting.ByIndex.PickIndex;
using Jiml.Extracting.ByIndex.RangeIndex;
using Jiml.Extracting.Results;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.ByIndex
{
    public class VariableByPickIndexExtractor : JArrayExtractor
    {
        private readonly PickIndexComposer _pickIndexComposer;

        public VariableByPickIndexExtractor(
            PickIndexComposer pickIndexComposer)
        {
            _pickIndexComposer = pickIndexComposer;
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
            var indexes = _pickIndexComposer.Compose(
                jArrayContext.Input,
                jArrayContext.Path);

            var normalizedPickIndex = NormalizedArrayPickIndex.Normalize(
                indexes: indexes,
                arrayLength: jArrayContext.Value.Count);

            var outOfRangeIndexes = TryGetOutOfRangeIndexes(
                normalizedPickIndex, 
                jArrayContext);

            if (outOfRangeIndexes.Any())
                return IndexesOutOfRange(
                    jArrayContext,
                    outOfRangeIndexes);

            var array = normalizedPickIndex
                .AllIndexes()
                .Aggregate(
                    seed: new JArray(),
                    (acc, index) =>
                    {
                        acc.Add(jArrayContext.Value[index.NormalizedValue]);
                        return acc;
                    });

            if (indexes.Length == 1)
                return new CorrectExtraction(
                    GetPath(
                        jArrayContext.Path),
                    array[0]);

            return new CorrectExtraction(
                GetPath(
                    jArrayContext.Path),
                array);
        }

        private IndexOutOfRange IndexesOutOfRange(
            JArrayContext jArrayContext, 
            List<NormalizedArrayIndex> outOfRangeIndexes)
        {
            return new IndexOutOfRange(
                path: GetPath(
                    jArrayContext.Path),
                previousPath: jArrayContext.Path,
                jArray: jArrayContext.Value,
                wrongIndexes: outOfRangeIndexes
                    .Select(index => index.OriginalValue));
        }
        
        private static List<NormalizedArrayIndex> TryGetOutOfRangeIndexes(
            NormalizedArrayPickIndex normalizedPickIndex, 
            JArrayContext jArrayContext)
        {
            return normalizedPickIndex
                .AllIndexes()
                .Where(index => IsIndexOutOfRange(
                    index.NormalizedValue, 
                    jArrayContext))
                .ToList();
        }

        private static bool IsIndexOutOfRange(
            int index, 
            JArrayContext jArrayContext)
        {
            return index < 0 || index >= jArrayContext.Value.Count;
        }

        private Path GetPath(
            Path previousPath)
        {
            return previousPath.Append(
                $"[{_pickIndexComposer}]");
        }
    }
}