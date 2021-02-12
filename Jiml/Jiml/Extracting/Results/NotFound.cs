using Jiml.Errors;

namespace Jiml.Extracting.Results
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