using System;
using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Numbers
{
    public abstract class NumberComposition
    {
        public JValue Value { get; }
        public Jiml.Error[] Errors { get; }
        public bool IsSuccess { get; }

        public decimal GetDecimal() => (decimal) Value;

        protected NumberComposition(decimal content)
        {
            Value = new JValue(content);

            IsSuccess = true;
        }

        protected NumberComposition(long content)
        {
            Value = new JValue(content);

            IsSuccess = true;
        }

        protected NumberComposition(Jiml.Error[] errors)
        {
            Errors = errors ?? throw new ArgumentNullException(nameof(errors));

            IsSuccess = false;
        }

        public class Success: NumberComposition
        {
            public Success(decimal content) : base(content)
            {
            }

            public Success(long content) : base(content)
            {
            }
        }

        public class Failure: NumberComposition
        {
            public Failure(Jiml.Error[] errors) : base(errors)
            {
            }
        }
    }
}