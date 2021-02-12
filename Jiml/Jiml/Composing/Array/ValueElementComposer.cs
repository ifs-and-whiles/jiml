using System;
using Jiml.Composing.Results;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Array
{
    public class ValueElementComposer: IElementComposer
    {
        private readonly IComposer _innerComposer;

        public ValueElementComposer(
            IComposer innerComposer)
        {
            _innerComposer = innerComposer ?? throw new ArgumentNullException(nameof(innerComposer));
        }

        public ElementComposition Compose(
            JObject input,
            Path parentPath,
            int index)
        {
            var composition = _innerComposer.Compose(
                input,
                parentPath.Append(
                    $"[{index}]"));

            return composition.IsSuccess
                ? CompositionSuccess(
                    index,
                    composition)
                : CompositionFailure(
                    index,
                    composition);
        }

       

        private static ElementComposition ConditionNotMet(int index)
        {
            return new ElementComposition.Success(
                index,
                new JToken[0]);
        }

        private ElementComposition CompositionSuccess(
            int index,
            Composition composition)
        {
            return new ElementComposition.Success(
                index,
                new[] {composition.Value});
        }

        private ElementComposition CompositionFailure(
            int index,
            Composition composition)
        {
            return new ElementComposition.Failure(
                index,
                composition.Errors);
        }
    }
}