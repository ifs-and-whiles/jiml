using Jiml.Composing;
using Jiml.Errors;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Conditions
{
    public class EqualityCondition : ICondition
    {
        private readonly IComposer _left;
        private readonly IComposer _right;

        public EqualityCondition(
            IComposer left,
            IComposer right)
        {
            _left = left;
            _right = right;
        }

        public ConditionResult Evaluate(JObject input)
        {
            var left = _left.Compose(
                input,
                Path.Root);

            if(!left.IsSuccess) 
                return new ConditionResult.Failure(new []
                {
                    new ConditionFailed(
                        left.Errors), 
                });

            var right = _right.Compose(
                input,
                Path.Root);

            if (!right.IsSuccess)
                return new ConditionResult.Failure(new[]
                {
                    new ConditionFailed(
                        left.Errors),
                });

            var areEqual = JToken.DeepEquals(
                left.Value,
                right.Value);

            return new ConditionResult.Success(
                areEqual);
        }
    }
}