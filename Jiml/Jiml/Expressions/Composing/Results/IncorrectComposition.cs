using Jiml.Expressions.Errors;
using Jiml.Expressions.Extracting;

namespace Jiml.Expressions.Composing.Results
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