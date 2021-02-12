namespace Jiml.Errors
{
    public sealed class PickIndexElementCompositionFailed : Jiml.Error
    {
        public PickIndexElementCompositionFailed(
            params Jiml.Error[] innerErrors) : base(
            $"Composition of Pick Index Element failed.",
            "pick_index_element_composition_failed",
            innerErrors)
        {
        }
    }
}