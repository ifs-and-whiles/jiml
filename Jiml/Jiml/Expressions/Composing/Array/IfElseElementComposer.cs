using System;
using Jiml.Expressions.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Array
{
    public class IfElseElementComposer : IConditionalElementComposer
    {
        private readonly IConditionalElementComposer _ifComposer;
        private readonly IConditionalElementComposer _elseComposer;

        public IfElseElementComposer(
            IConditionalElementComposer ifComposer,
            IConditionalElementComposer elseComposer = null)
        {
            _ifComposer = ifComposer ?? throw new ArgumentNullException(nameof(ifComposer));
            _elseComposer = elseComposer;
        }

        public Option<ElementComposition> Compose(
            JObject input, 
            Path parentPath,
            int index)
        {
            var compositionOption = _ifComposer.Compose(
                input,
                parentPath,
                index);
            
            if (compositionOption.TryGet(out var composition))
            {
                return composition;
            }

            return _elseComposer == null
                ? Option<ElementComposition>.None
                : _elseComposer.Compose(
                    input,
                    parentPath,
                    index);
        }
    }
}