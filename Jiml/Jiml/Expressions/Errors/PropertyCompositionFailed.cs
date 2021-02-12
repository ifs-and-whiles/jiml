namespace Jiml.Expressions.Errors
{
    public sealed class PropertyCompositionFailed : Jiml.Error
    {
        public PropertyCompositionFailed(
            string compositionPath,
            string propertyName,
            params Jiml.Error[] innerErrors) : base(
            $"Composition of Property '{propertyName}' failed at '{compositionPath}'.", 
            "property_composition_failed", 
            innerErrors)
        {
        }
    }
}