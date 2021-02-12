namespace Jiml.Errors
{
    public sealed class PickIndexExtractionFailed : Jiml.Error
    {
        public PickIndexExtractionFailed(
            int position,
            params Jiml.Error[] innerErrors) : base(
            $"Picking array element with Index at position [{position}] failed.",
            "picking_array_element_failed",
            innerErrors)
        {
        }
    }
}