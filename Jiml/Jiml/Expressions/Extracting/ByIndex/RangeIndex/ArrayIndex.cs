namespace Jiml.Expressions.Extracting.ByIndex.RangeIndex
{
    public class NormalizedArrayIndex
    {
        public int OriginalValue { get; }
        public int NormalizedValue { get; }

        private NormalizedArrayIndex(
            int originalValue,
            int normalizedValue)
        {
            OriginalValue = originalValue;
            NormalizedValue = normalizedValue;
        }

        public static NormalizedArrayIndex Normalize(
            int index,
            int arrayLength)
        {
            if (index >= 0)
                return new NormalizedArrayIndex(
                    originalValue: index,
                    normalizedValue: index);

            return new NormalizedArrayIndex(
                originalValue: index,
                normalizedValue: arrayLength + index);
        }
    }
}