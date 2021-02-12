namespace Jiml.Errors
{
    public sealed class IndexWasOutOfRange : Jiml.Error
    {
        public IndexWasOutOfRange(
            string currentPath,
            string previousPath,
            int count) : base(
            message: $"'{currentPath}' accessing failed. " +
                     $"Path '{previousPath}' has only '{count}' elements.",
            code: "index_was_out_of_range")
        {
        }

        public IndexWasOutOfRange(
            string currentPath,
            string previousPath,
            int count,
            int[] wrongIndexes) : base(
            message: $"'{currentPath}' accessing failed. " +
                     $"Path '{previousPath}' has only '{count}' elements. " +
                     $"Following indexes were out of range: {string.Join(", ", wrongIndexes)}.",
            code: "index_was_out_of_range")
        {
        }
    }
}