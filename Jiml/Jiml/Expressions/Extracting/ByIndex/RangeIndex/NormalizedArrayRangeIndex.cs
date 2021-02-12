using System;
using System.Collections.Generic;

namespace Jiml.Expressions.Extracting.ByIndex.RangeIndex
{
    public class NormalizedArrayRangeIndex
    {
        private readonly int _from;

        private readonly int _to;

        private NormalizedArrayRangeIndex(int @from, int to)
        {
            _from = @from;
            _to = to;
        }

        public IEnumerable<int> AllIndexes()
        {
            for (var i = _from; i < _to; i++)
            {
                yield return i;
            }
        }

        public static NormalizedArrayRangeIndex Normalize(
            RangeIndexComposition rangeIndex,
            int arrayLength)
        {
            if (rangeIndex == null) 
                throw new ArgumentNullException(nameof(rangeIndex));

            if (arrayLength < 0) 
                throw new ArgumentOutOfRangeException(
                    nameof(arrayLength),
                    $"{nameof(arrayLength)} cannot be negative, but found {arrayLength}");

            var normalizedFrom = NormalizedArrayIndex.Normalize(
                rangeIndex.From ?? 0, 
                arrayLength);

            var normalizedTo = NormalizedArrayIndex.Normalize(
                rangeIndex.To ?? arrayLength,
                arrayLength);

            return new NormalizedArrayRangeIndex(
                normalizedFrom.NormalizedValue,
                normalizedTo.NormalizedValue);
        }
    }
}