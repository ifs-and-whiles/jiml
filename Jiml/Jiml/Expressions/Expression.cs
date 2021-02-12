using Jiml.Expressions.Composing;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions
{
    public class Expression : IJimlExpression
    {
        private readonly IComposer _composer;

        public Expression(IComposer composer)
        {
            _composer = composer;
        }

        public Jiml.Result Evaluate(JObject input)
        {
            var result = _composer.Compose(
                input: input,
                parentPath: Path.Root);

            if(result.IsSuccess)
                return new Jiml.Result(
                    result.Value);

            return new Jiml.Result(
                result.Errors);
        }
    }
}