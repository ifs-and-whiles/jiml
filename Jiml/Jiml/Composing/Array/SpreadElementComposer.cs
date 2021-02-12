using System;
using Jiml.Composing.Results;
using Jiml.Errors;
using Jiml.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Array
{
    public class SpreadElementComposer : IElementComposer
    {
        private readonly IComposer _composerToSpread;

        public SpreadElementComposer(
            IComposer composerToSpread)
        {
            _composerToSpread = composerToSpread ?? throw new ArgumentNullException(nameof(composerToSpread));
        }

        public ElementComposition Compose(
            JObject input,
            Path parentPath,
            int index)
        {
            var composition = _composerToSpread.Compose(
                input,
                parentPath);

            if (!composition.IsSuccess)
                return InnerCompositionFailed(
                    parentPath, 
                    index, 
                    composition);

            if (composition.Value is JArray jArray)
                return new ElementComposition.Success(
                    index,
                    jArray);

            return new ElementComposition.Success(
                index,
                new[] {composition.Value});
        }
        
        private static ElementComposition InnerCompositionFailed(
            Path parentPath, 
            int index, 
            Composition composition)
        {
            return new ElementComposition.Failure(
                index,
                new SpreadArrayElementCompositionFailed(
                    compositionPath: parentPath,
                    index: index,
                    innerErrors: composition.Errors));
        }
    }
}