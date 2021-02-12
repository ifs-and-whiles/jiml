namespace Jiml.Expressions.Errors
{
    public sealed class ElementCompositionFailed : Jiml.Error
    {
        public ElementCompositionFailed(
            string compositionPath,
            int index,
            params Jiml.Error[] innerErrors) : base(
            $"Composition of Array Element [{index}] failed at '{compositionPath}'.",
            "array_element_composition_failed",
            innerErrors)
        {
        }
    }
}