using System;
using System.Collections.Generic;
using Jiml.Composing.Results;
using Jiml.Conditions;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Iterators
{
    public class FilterIterator : IComposer
    {
        private readonly IComposer _iterable;
        private readonly string _lambdaVarName;
        private readonly ICondition _condition;

        public FilterIterator(
            IComposer iterable,
            string lambdaVarName,
            ICondition condition)
        {
            _iterable = iterable ?? throw new ArgumentNullException(nameof(iterable));
            _lambdaVarName = lambdaVarName ?? throw new ArgumentNullException(nameof(lambdaVarName));
            _condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            var iterable = _iterable.Compose(
                input,
                parentPath);

            if (!iterable.IsSuccess)
                throw new NotImplementedException("handle possible problems");
            
            if (iterable.Value is JArray array)
            {
                var results = new List<JToken>();

                foreach (var jToken in array)
                {
                    var scopedInput = GetScopedInput(
                        input,
                        jToken);

                    var conditionResult = _condition.Evaluate(
                        scopedInput);

                    if (!conditionResult.IsSuccess)
                        throw new NotImplementedException("handle possible problems");

                    if(conditionResult.Value)
                        results.Add(jToken);
                }

                return new CorrectComposition(
                    new JArray(results.ToArray()));
            }
            else
            {
                var scopedInput = GetScopedInput(
                    input,
                    iterable.Value);

                var conditionResult = _condition.Evaluate(
                    scopedInput);


                if (!conditionResult.IsSuccess)
                    throw new NotImplementedException("handle possible problems");

                return new CorrectComposition(
                    new JArray(iterable.Value));
            }
        }

        private JObject GetScopedInput(JObject input, JToken value)
        {
            var scopedInput = (JObject)input.DeepClone();

            scopedInput.Add(_lambdaVarName, value);

            return scopedInput;
        }
    }
}