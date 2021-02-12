using System;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting
{
    public class Context
    {
        public JObject Input { get; }
        public JToken Value { get; }
        public Path Path { get; }

        public Context(
            JObject input, 
            JToken value, 
            Path path)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Input = input ?? throw new ArgumentNullException(nameof(input));
        }

        public static Context Root(JObject input) => new Context(
            input,
            input,
            Path.Empty);
    }
}