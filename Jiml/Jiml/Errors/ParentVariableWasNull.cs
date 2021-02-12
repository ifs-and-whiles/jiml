namespace Jiml.Errors
{
    public sealed class ParentVariableWasNull : Jiml.Error
    {
        public ParentVariableWasNull(
            string currentPath,
            string previousPath) : base(
            message: $"'{currentPath}' accessing failed. " +
                     $"Path '{previousPath}' was <null>.",
            code: "parent_variable_was_null")
        {
        }
    }
}