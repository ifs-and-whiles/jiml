using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Errors
{
    public sealed class VariableWasNotANumber : Jiml.Error
    {
        public VariableWasNotANumber(
            Path variablePath,
            JToken variableValue) : base(
            $"Variable '{variablePath}' was expected to be a number, but found '{variableValue}'.",
            "variable_was_not_a_number",
            new Jiml.Error[0])
        {
        }
    }
}