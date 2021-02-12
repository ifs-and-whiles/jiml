using System;
using Jiml.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Object.Property
{
    public class PropertyComposer
    {
        public string Name { get; }
        private readonly IComposer _innerComposer;

        public PropertyComposer(
            string name,
            IComposer innerComposer)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _innerComposer = innerComposer ?? throw new ArgumentNullException(nameof(innerComposer));
        }

        public Option<PropertyComposition> Compose(
            JObject input,
            Path parentPath)
        {
            var composition = _innerComposer.Compose(
                input,
                parentPath.AddSegment(Name));

            if (composition.IsSuccess)
                return new PropertyComposition.Success(
                    Name,
                    composition.Value);

            return new PropertyComposition.Failure(
                Name,
                composition.Errors);
        }
    }
}