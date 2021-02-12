using System;
using Jiml.Expressions.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Object.Property
{
    public class IfElsePropertyComposer : IConditionalPropertyComposer
    {
        private readonly IConditionalPropertyComposer _ifComposer;
        private readonly IConditionalPropertyComposer _elseComposer;

        public IfElsePropertyComposer(
            IConditionalPropertyComposer ifComposer,
            IConditionalPropertyComposer elseComposer = null)
        {
            _ifComposer = ifComposer ?? throw new ArgumentNullException(nameof(ifComposer));
            _elseComposer = elseComposer;
        }

        public Option<PropertyComposition> Compose(
            JObject input,
            Path parentPath)
        {
            var compositionOption = _ifComposer.Compose(
                input,
                parentPath);

            if (compositionOption.TryGet(out var composition))
            {
                return composition;
            }

            return _elseComposer == null
                ? Option<PropertyComposition>.None
                : _elseComposer.Compose(
                    input,
                    parentPath);
        }
    }
}