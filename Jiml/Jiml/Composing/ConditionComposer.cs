using Jiml.Composing.Results;
using Jiml.Conditions;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing
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
