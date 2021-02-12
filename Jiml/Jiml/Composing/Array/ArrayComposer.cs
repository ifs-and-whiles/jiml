using System;
using System.Collections.Generic;
using System.Linq;
using Jiml.Composing.Results;
using Jiml.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Array
{
    public class ArrayComposer: IComposer
    {
        private readonly List<IConditionalElementComposer> _elementComposers;

        public ArrayComposer(
            IEnumerable<IConditionalElementComposer> elementComposers)
        {
            if (elementComposers == null) throw new ArgumentNullException(nameof(elementComposers));
            _elementComposers = elementComposers.ToList();
        }

        public Composition Compose(
            JObject input,
            Path parentPath)
        {
            return _elementComposers
                .Aggregate(
                    seed: new JArrayCompositionBuilder(),
                    (acc, elementComposer, index) =>
                    {
                        var elementOption = elementComposer.Compose(
                            input,
                            parentPath,
                            index);

                        return elementOption.Match(
                            element => element.IsSuccess
                                ? acc.AddElements(element.Values)
                                : acc.AddErrors(element.Errors),
                            () => acc);
                    })
                .Build();
        }
        
        private class JArrayCompositionBuilder
        {
            private readonly List<JToken> _elements =
                new List<JToken>();

            private readonly List<Jiml.Error> _errors =
                new List<Jiml.Error>();

           public JArrayCompositionBuilder AddElements(
               IEnumerable<JToken> elements)
           {
                _elements.AddRange(elements);

                return this;
           }

           public JArrayCompositionBuilder AddErrors(
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

               var jArray = _elements
                   .Aggregate(
                       seed: new JArray(),
                       (acc, element) =>
                       {
                           acc.Add(element);
                           return acc;
                       });

               return  new CorrectComposition(
                   jArray);
           }
        }
    }
}
