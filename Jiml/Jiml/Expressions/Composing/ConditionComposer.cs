using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Conditions;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing
{
    public class ConditionComposer: IComposer
    {
        private readonly ICondition _condition;

        public ConditionComposer(
            ICondition condition)
        {
            _condition = condition;
        }

        public Composition Compose(
            JObject input, 
            Path parentPath)
        {
            var condition = _condition.Evaluate(
                input);

            if(condition.IsSuccess)
                return new CorrectComposition(
                    new JValue(condition.Value));

            return new IncorrectComposition(
                parentPath,
                condition.Errors);
        }
    }
}
