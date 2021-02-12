using System;
using System.Linq;

namespace Jiml.Expressions.Extracting.ByIndex.PickIndex
{
    public class PickIndexElementComposition
    {
        public int[] Values { get; }
        public int Index { get; }
        public bool IsSuccess { get; }
        public Jiml.Error[] Errors { get; }


        protected PickIndexElementComposition(
            int index,
            int[] values)
        {
            Index = index;
            Values = values ?? throw new ArgumentNullException(nameof(values));
            IsSuccess = true;
        }

        protected PickIndexElementComposition(
            int index,
            params Jiml.Error[] errors)
        {
            Index = index;

            if (errors == null) throw new ArgumentNullException(nameof(errors));
            Errors = errors.ToArray();
        }

        public class Failure : PickIndexElementComposition
        {
            public Failure(
                int index,
                params Jiml.Error[] errors) : base(
                index,
                errors)
            {
            }
        }

        public class Success : PickIndexElementComposition
        {
            public Success(
                int index,
                int[] values) : base(
                index,
                values)
            {
            }
        }
    }
}