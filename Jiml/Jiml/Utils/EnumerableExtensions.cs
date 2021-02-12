using System;
using System.Collections.Generic;

namespace Jiml.Utils
{
    public static class EnumerableExtensions
    {
        public static TAccumulate Aggregate<TSource, TAccumulate>(
            this IEnumerable<TSource> source, 
            TAccumulate seed, 
            Func<TAccumulate, TSource, int, TAccumulate> func)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (func == null) throw new ArgumentNullException(nameof(func));

            var index = 0;
            var result = seed;
            foreach (var element in source)
            {
                result = func(result, element, index);
                index++;
            }

            return result;
        }
    }
}
