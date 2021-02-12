namespace Jiml.Errors
{
    public sealed class VariableWasNotFound : Jiml.Error
    {
        public VariableWasNotFound(string path) : base(
            message: $"Path '{path}' was not found in the input.",
            code: "variable_was_not_found")
        {
        }
    }
}