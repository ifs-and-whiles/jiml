namespace Jiml.Errors
{
    public sealed class ElementsExtractionFailed : Jiml.Error
    {
        public ElementsExtractionFailed(
            string currentPath,
            params Jiml.Error[] innerErrors) : base(
            $"Array elements extraction failed at '{currentPath}'.",
            "array_elements_extraction_failed",
            innerErrors)
        {
        }
    }
}