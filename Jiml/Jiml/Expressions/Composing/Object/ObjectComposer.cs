using System.Collections.Generic;
using System.Linq;
using Jiml.Expressions.Composing.Object.Property;
using Jiml.Expressions.Composing.Results;
using Jiml.Expressions.Extracting;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Object
{
    public class ObjectComposer: IComposer
    {
        private readonly List<IConditionalPropertyComposer> _propertyComposers;

        public ObjectComposer(
            IEnumerable<IConditionalPropertyComposer> propertyComposers)
        {
            _propertyComposers = propertyComposers.ToList();
        }
        
        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            return _propertyComposers
                .Aggregate(
                    seed: new JObjectCompositionBuilder(),
                    (acc, propertyComposer) =>
                    {
                        var propertyOption = propertyComposer.Compose(
                            input,
                            parentPath);

                        return propertyOption.Match(
                            property => property.IsSuccess
                                ? acc.AddElement(property.Name, property.Value)
                                : acc.AddErrors(property.Errors),
                            () => acc);
                    })
                .Build();
        }

        private class JObjectCompositionBuilder
        {
            private readonly List<(string Name, JToken Value)> _properties =
                new List<(string Name, JToken Value)>();

            private readonly List<Jiml.Error> _errors =
                new List<Jiml.Error>();

            public JObjectCompositionBuilder AddElement(
                string name,
                JToken value)
            {
                _properties.Add((name, value));

                return this;
            }

            public JObjectCompositionBuilder AddErrors(
                IEnumerable<Jiml.Error> errors)
            {
                _errors.AddRange(errors);

                return this;
            }

            public Composition Build()
            {
                if (_errors.Any())
                {
                    return new IncorrectComposition(
                        _errors.ToArray());
                }

                var jObject = _properties
                    .Aggregate(
                        seed: new JObject(), 
                        (acc, property) =>
                        {
                            acc.Add(property.Name, property.Value);
                            return acc;
                        });

                return new CorrectComposition(
                    jObject);
            }
        }
    }
}
