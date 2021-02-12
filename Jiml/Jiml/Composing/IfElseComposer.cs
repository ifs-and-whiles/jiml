using Jiml.Composing.Results;
using Jiml.Conditions;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing
{
    public class IfElseComposer : IComposer
    {
        private readonly ICondition _condition;
        private readonly IComposer _ifComposer;
        private readonly IComposer _elseComposer;

        public IfElseComposer(
            ICondition condition,
            IComposer ifComposer,
            IComposer elseComposer)
        {
            _condition = condition;
            _elseComposer = elseComposer;
            _ifComposer = ifComposer;
        }

        public Composition Compose(
            JObject input, 
            Path parentPath)
        {
            var conditionResult = _condition.Evaluate(input);

            return conditionResult.Value
                ? _ifComposer.Compose(
                    input, 
                    parentPath)
                : _elseComposer.Compose(
                    input,
                    parentPath);

        }
    }
}