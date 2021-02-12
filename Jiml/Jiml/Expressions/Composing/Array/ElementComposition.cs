using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Array
{
    public class ElementComposition
    {
        public JToken[] Values { get; }
        public int Index { get; }
        public bool IsSuccess { get; }
        public Jiml.Error[] Errors { get; }


        protected ElementComposition(
            int index,
            JToken[] values)
        {
            Index = index;
            Values = values ?? throw new ArgumentNullException(nameof(values));
            IsSuccess = true;
        }

        protected ElementComposition(
            int index,
            params Jiml.Error[] errors)
        {
            Index = index;

            if (errors == null) throw new ArgumentNullException(nameof(errors));
            Errors = errors.ToArray();
        }

        public class Failure : ElementComposition
        {
            public Failure(
                int index,
                params Jiml.Error[] errors) : base(
                index,
                errors)
            {
            }
        }

        public class Success : ElementComposition
        {
            public Success(
                int index,
                IEnumerable<JToken> values) : base(
                index,
                values.ToArray())
            {
            }
        }
    }
}