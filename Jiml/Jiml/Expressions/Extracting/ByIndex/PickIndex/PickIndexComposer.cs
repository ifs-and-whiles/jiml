using System;
using System.Collections.Generic;
using System.Linq;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.ByIndex.PickIndex
{
    public class PickIndexComposer
    {
        private readonly IConditionalPickIndexElementComposer[] _elements;

        public PickIndexComposer(
            IEnumerable<IConditionalPickIndexElementComposer> elements)
        {
            if (elements == null) 
                throw new ArgumentNullException(nameof(elements));

            _elements = elements.ToArray();
        }

        public int[] Compose(
            JObject input,
            Path parentPath)
        {
            var values = _elements
                .Aggregate(
                    seed: new List<int>(),
                    func: (acc, element, index) =>
                    {
                        var pickIndexElementOption = element.Compose(
                            input,
                            parentPath,
                            index);

                        var result =  pickIndexElementOption.Match(
                            pickIndexElement =>
                            {
                                if (!pickIndexElement.IsSuccess)
                                    throw new NotImplementedException("handle possible problems");

                                acc.AddRange(pickIndexElement.Values);

                                return acc;
                            },
                            () => acc);

                        return result;
                    });

            return values.ToArray();
        }
    }
}