using System;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.ByIndex.PickIndex
{
    public class IfElsePickIndexElementComposer: IConditionalPickIndexElementComposer
    {
        private readonly IConditionalPickIndexElementComposer _ifComposer;
        private readonly IConditionalPickIndexElementComposer _elseComposer;

        public IfElsePickIndexElementComposer(
            IConditionalPickIndexElementComposer ifComposer,
            IConditionalPickIndexElementComposer elseComposer = null)
        {
            _ifComposer = ifComposer ?? throw new ArgumentNullException(nameof(ifComposer));
            _elseComposer = elseComposer;
        }

        public Option<PickIndexElementComposition> Compose(
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
                ? Option<PickIndexElementComposition>.None
                : _elseComposer.Compose(
                    input,
                    parentPath,
                    index);
        }
    }
}