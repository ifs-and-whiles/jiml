namespace Jiml.Expressions.Errors
{
    public sealed class SpreadArrayElementCompositionFailed : Jiml.Error
    {
        public SpreadArrayElementCompositionFailed(
            string compositionPath,
            int index,
            params Jiml.Error[] innerErrors) : base(
            $"Spreading (...) of Array Element [{index}] failed at '{compositionPath}'.",
            "spread_array_element_composition_failed",
            innerErrors)
        {
        }
    }
}