using System;
using Jiml.Expressions.Conditions;
using Jiml.Expressions.Errors;
using Jiml.Expressions.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Array
{
    public class IfElementComposer : IConditionalElementComposer
    {
        private readonly IElementComposer _elementComposer;
        private readonly ICondition _condition;

        public IfElementComposer(
            IElementComposer elementComposer,
            ICondition condition = null)
        {
            _elementComposer = elementComposer ?? throw new ArgumentNullException(nameof(elementComposer));
            _condition = condition ?? new TrueCondition();
        }

        public Option<ElementComposition> Compose(
            JObject input,
            Path parentPath,
            int index)
        {
            var conditionResult = _condition.Evaluate(
                input);

            if (!conditionResult.IsSuccess)
                return ConditionFailure(
                    parentPath,
                    index,
                    conditionResult);

            if (!conditionResult.Value)
                return Option<ElementComposition>.None;

            return _elementComposer.Compose(
                input,
                parentPath,
                index);
        }

        private ElementComposition ConditionFailure(
            Path parentPath,
            int index,
            ConditionResult conditionResult)
        {
            return new ElementComposition.Failure(
                index,
                new ElementCompositionFailed(
                    compositionPath: parentPath,
                    index: index,
                    innerErrors: conditionResult.Errors));
        }
    }
}