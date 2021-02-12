using System;
using System.Collections.Generic;
using System.Linq;
using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Iterators
{
    public class MapIterator : IComposer
    {
        private readonly IComposer _iterable;
        private readonly string _lambdaVarName;
        private readonly IComposer _lambdaOperation;

        public MapIterator(
            IComposer iterable,
            string lambdaVarName,
            IComposer lambdaOperation)
        {
            _iterable = iterable ?? throw new ArgumentNullException(nameof(iterable));
            _lambdaVarName = lambdaVarName ?? throw new ArgumentNullException(nameof(lambdaVarName));
            _lambdaOperation = lambdaOperation ?? throw new ArgumentNullException(nameof(lambdaOperation));
        }

        public Composition Compose(
            JObject input, 
            Path parentPath)
        {
            var iterable = _iterable.Compose(
                input,
                parentPath);

            if(!iterable.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            
            if (iterable.Value is JArray array)
            {
                var results = new List<Composition>();

                foreach (var jToken in array)
                {
                    var scopedInput = GetScopedInput(
                        input,
                        jToken);

                    var composition = _lambdaOperation.Compose(
                        scopedInput,
                        parentPath);

                    if(!composition.IsSuccess)
                        throw new NotImplementedException("handle possible problems");

                    results.Add(composition);
                }

                return new CorrectComposition(
                    new JArray(
                        results.Select(x => x.Value).ToArray()));
            }
            else
            {
                var scopedInput = GetScopedInput(
                    input, 
                    iterable.Value);

                var composition = _lambdaOperation.Compose(
                    scopedInput,
                    parentPath);

                if (!composition.IsSuccess)
                    throw new NotImplementedException("handle possible problems");

                return new CorrectComposition(
                    new JArray(composition.Value));
            }
        }

        private JObject GetScopedInput(JObject input, JToken value)
        {
            var scopedInput = (JObject) input.DeepClone();

            scopedInput.Add(_lambdaVarName, value);

            return scopedInput;
        }
    }
}
