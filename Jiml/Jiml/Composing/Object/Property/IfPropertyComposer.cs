using System;
using Jiml.Conditions;
using Jiml.Errors;
using Jiml.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Object.Property
{
    public class IfPropertyComposer : IConditionalPropertyComposer
    {
        private readonly PropertyComposer _propertyComposer;
        private readonly ICondition _condition;

        public IfPropertyComposer(
            PropertyComposer propertyComposer,
            ICondition condition = null)
        {
            _propertyComposer = propertyComposer ?? throw new ArgumentNullException(nameof(propertyComposer));
            _condition = condition ?? new TrueCondition();
        }

        public Option<PropertyComposition> Compose(
            JObject input,
            Path parentPath)
        {
            var conditionResult = _condition.Evaluate(
                input);

            if (!conditionResult.IsSuccess)
                return ConditionFailure(
                    parentPath, 
                    conditionResult);

            if (!conditionResult.Value) 
                return Option<PropertyComposition>.None;

            return _propertyComposer.Compose(
                input,
                parentPath);
        }

        private PropertyComposition.Failure ConditionFailure(
            Path parentPath,
            ConditionResult conditionResult)
        {
            return new PropertyComposition.Failure(
                _propertyComposer.Name,
                new PropertyCompositionFailed(
                    compositionPath: parentPath,
                    propertyName: _propertyComposer.Name,
                    innerErrors: conditionResult.Errors));
        }
    }
}