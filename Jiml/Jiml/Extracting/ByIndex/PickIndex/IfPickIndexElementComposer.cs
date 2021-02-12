using System;
using Jiml.Composing.Numbers;
using Jiml.Conditions;
using Jiml.Errors;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.ByIndex.PickIndex
{
    public class IfPickIndexElementComposer : IConditionalPickIndexElementComposer
    {
        private readonly INumberComposer _valueComposer;
        private readonly ICondition _condition;

        public IfPickIndexElementComposer(
            INumberComposer valueComposer,
            ICondition condition = null)
        {
            _valueComposer = valueComposer ?? throw new ArgumentNullException(nameof(valueComposer));
            _condition = condition ?? new TrueCondition();
        }

        public Option<PickIndexElementComposition> Compose(
            JObject input,
            Path parentPath,
            int index)
        {
            var conditionResult = _condition.Evaluate(
                input);

            if (!conditionResult.IsSuccess)
                return new PickIndexElementComposition.Failure(
                    index,
                    new PickIndexElementCompositionFailed(
                        innerErrors: conditionResult.Errors));

            if (!conditionResult.Value)
                return Option<PickIndexElementComposition>.None;

            var composition = _valueComposer.Compose(
                input,
                parentPath);

            if (composition.GetDecimal().IsInteger(out var value))
                return new PickIndexElementComposition.Success(
                    index,
                    new[] {value});

            throw new NotImplementedException();
        }
    }
}