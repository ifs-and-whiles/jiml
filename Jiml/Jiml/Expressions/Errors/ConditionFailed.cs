namespace Jiml.Expressions.Errors
{
    public sealed class ConditionFailed : Jiml.Error
    {
        public ConditionFailed(
            params Jiml.Error[] innerErrors) : base(
            $"Condition failed.",
            "condition_failed",
            innerErrors)
        {
        }
    }
}