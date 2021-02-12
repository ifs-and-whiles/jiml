using Jiml.Errors;
using Jiml.Extracting;

namespace Jiml.Composing.Results
{
    public class IncorrectComposition : Composition
    {
        public IncorrectComposition(
            Jiml.Error[] errors) : base(
            errors)
        {
            
        }

        public IncorrectComposition(
            Path compositionPath,
            Jiml.Error[] errors) : base(
            new Jiml.Error[]
            {
                new CompositionFailed(
                    compositionPath,
                    errors)
            })
        {
        }
    }
}