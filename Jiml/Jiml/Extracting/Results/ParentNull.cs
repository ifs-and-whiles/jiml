using Jiml.Errors;

namespace Jiml.Extracting.Results
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