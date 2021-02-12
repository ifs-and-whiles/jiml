using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Errors
{
    public sealed class VariableWasNotAnArray : Jiml.Error
    {
        private const string ErrorCode = "variable_was_not_an_array";

        public VariableWasNotAnArray(
            string path,
            string previousPath,
            JValue value) : base(
            message: $"Cannot execute '{path}'. " +
                     $"'{previousPath}' cannot be accessed by index as it is not an array. " +
                     $"Actual value was " +
                     $"type: '{value.Type}', " +
                     $"value: '{value.Value}'.", 
            code: ErrorCode)
        {
        }

        public VariableWasNotAnArray(
            string path,
            string previousPath,
            JObject value) : base(
            message: $"Cannot execute '{path}'. " +
                     $"'{previousPath}' cannot be accessed by index as it is not an array. " +
                     $"Actual value was " +
                     $"type: '{value.Type}', " +
                     $"value: '{value}'.",
            code: ErrorCode)
        {
        }
    }
}