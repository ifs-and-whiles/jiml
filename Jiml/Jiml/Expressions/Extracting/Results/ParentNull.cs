using Jiml.Expressions.Errors;

namespace Jiml.Expressions.Extracting.Results
{
    public sealed class ParentNull : Extraction
    {
        public ParentNull(
            Path path,
            Path previousPath) : base(
            path, 
            new ParentVariableWasNull(
                path,
                previousPath))
        {
        }
    }
}