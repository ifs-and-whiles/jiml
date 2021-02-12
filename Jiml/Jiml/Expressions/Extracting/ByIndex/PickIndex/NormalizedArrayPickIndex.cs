using System;
using System.Collections.Generic;
using System.Linq;
using Jiml.Expressions.Extracting.ByIndex.RangeIndex;

namespace Jiml.Expressions.Extracting.ByIndex.PickIndex
{
    public class NormalizedArrayPickIndex
    {
        private readonly NormalizedArrayIndex[] _normalizedIndexes;

        private NormalizedArrayPickIndex(
            IEnumerable<NormalizedArrayIndex> normalizedIndexes)
        {
            _normalizedIndexes = normalizedIndexes?.ToArray() 
                                 ?? throw new ArgumentNullException(nameof(normalizedIndexes));
        }

        public IEnumerable<NormalizedArrayIndex> AllIndexes()
        {
            return _normalizedIndexes.ToArray();
        }

        public static NormalizedArrayPickIndex Normalize(
            IEnumerable<int> indexes,
            int arrayLength)
        {
            if (indexes == null) 
                throw new ArgumentNullException(nameof(indexes));

            if (arrayLength < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(arrayLength),
                    $"{nameof(arrayLength)} cannot be negative, but found {arrayLength}");

            var normalizedIndexes = indexes
                .Select(index => NormalizedArrayIndex.Normalize(
                    index,
                    arrayLength));

            return new NormalizedArrayPickIndex(
                normalizedIndexes);
        }
    }
}