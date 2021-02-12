using System;
using System.Collections.Generic;
using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Iterators
{
    public class ReduceIterator : IComposer
    {
        private readonly IComposer _iterable;
        private readonly IComposer _initialAccumulator;
        private readonly string _accumulatorName;
        private readonly string _lambdaVarName;
        private readonly IComposer _reducer;

        public ReduceIterator(
            IComposer iterable,
            IComposer initialAccumulator,
            string accumulatorName,
            string lambdaVarName,
            IComposer reducer)
        {
            _iterable = iterable ?? throw new ArgumentNullException(nameof(iterable));
            _initialAccumulator = initialAccumulator;
            _accumulatorName = accumulatorName ?? throw new ArgumentNullException(nameof(accumulatorName));
            _lambdaVarName = lambdaVarName ?? throw new ArgumentNullException(nameof(lambdaVarName));
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
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

            var initialAccumulator = _initialAccumulator.Compose(
                input,
                parentPath);

            if(!initialAccumulator.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            var accValue = initialAccumulator.Value;

            if (iterable.Value is JArray array)
            {
                foreach (var jToken in array)
                {
                    var scopedInput = GetScopedInput(
                        input,
                        jToken,
                        accValue);

                    var composition = _reducer.Compose(
                        scopedInput,
                        parentPath);

                    if (!composition.IsSuccess)
                        throw new NotImplementedException("handle possible problems");

                    accValue = composition.Value;
                }

                return new CorrectComposition(accValue);
            }
            else
            {
                var scopedInput = GetScopedInput(
                    input,
                    iterable.Value,
                    accValue);

                var composition = _reducer.Compose(
                    scopedInput,
                    parentPath);

                if (!composition.IsSuccess)
                    throw new NotImplementedException("handle possible problems");
                
                return new CorrectComposition(composition.Value);
            }
        }

        private JObject GetScopedInput(JObject input, JToken value, JToken acc)
        {
            var scopedInput = (JObject)input.DeepClone();

            scopedInput.Add(_lambdaVarName, value);
            scopedInput.Add(_accumulatorName, acc);

            return scopedInput;
        }
    }
}