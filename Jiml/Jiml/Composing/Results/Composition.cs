using System;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Results
{
    public abstract class Composition
    {
        public JToken Value { get; }
        public Jiml.Error[] Errors { get; }
        public bool IsSuccess { get; }

        protected Composition(JToken content)
        {
            Value = content ?? throw new ArgumentNullException(nameof(content));

            IsSuccess = true;
        }

        protected Composition(Jiml.Error[] errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));

            IsSuccess = false;
        }
    }
}
