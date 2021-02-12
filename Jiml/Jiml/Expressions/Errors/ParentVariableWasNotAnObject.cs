using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Errors
{
    public sealed class ParentVariableWasNotAnObject : Jiml.Error
    {
        private const string ErrorCode = "parent_variable_was_not_an_object";

        public ParentVariableWasNotAnObject(
            string currentPath,
            string previousPath,
            JValue parentValue) : base(
            message: $"'{currentPath}' accessing failed. " +
                     $"Path '{previousPath}' did not have any properties. " +
                     $"Actual value of '{previousPath}' was " +
                     $"type: '{parentValue.Type}' " +
                     $"value: '{parentValue.Value}'.", 
            code: ErrorCode)
        {
        }

        public ParentVariableWasNotAnObject(
            string currentPath,
            string previousPath,
            JArray parentValue) : base(
            message: $"'{currentPath}' accessing failed. " +
                     $"Path '{previousPath}' did not have any properties. " +
                     $"Actual value of '{previousPath}' was " +
                     $"an array: '{parentValue}'.",
            code: ErrorCode)
        {
        }
    }
}