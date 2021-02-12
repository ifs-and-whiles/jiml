using Jiml.Expressions.Errors;

namespace Jiml.Expressions.Extracting.Results
{
    public sealed class NotFound : Extraction
    {
        public NotFound(
            Path path) : base(
            path,
            new VariableWasNotFound(
                path))
        {
        }
    }
}