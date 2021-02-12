namespace Jiml.Expressions.Errors
{
    public sealed class CompositionFailed : Jiml.Error
    {
        public CompositionFailed(
            string compositionPath,
            params Jiml.Error[] innerErrors) : base(
            $"Composition failed at '{compositionPath}'.", 
            "composition_failed", 
            innerErrors)
        {
        }
    }
}