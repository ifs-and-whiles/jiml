using System;
using Jiml.Expressions.Composing.Numbers;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.ByIndex.RangeIndex
{
    public class RangeIndex
    {
        private readonly INumberComposer _from;
        private readonly INumberComposer _to;

        public RangeIndex(
            INumberComposer from,
            INumberComposer to)
        {
            _from = @from;
            _to = to;
        }

        public RangeIndexComposition Compose(
            JObject input,
            Path parentPath)
        {
            var from = TryExtractIndexValue(
                _from,
                input,
                parentPath);
            
            var to = TryExtractIndexValue(
                _to,
                input,
                parentPath);

            return new RangeIndexComposition(
                from,
                to);
        }

        private static int? TryExtractIndexValue(
            INumberComposer composer,
            JObject input,
            Path parentPath)
        {
            if (composer == null) 
                return null;

            var composition = composer.Compose(
                input,
                parentPath);

            if(!composition.IsSuccess)
                throw new NotImplementedException("handle possible problems");

            if (composition.GetDecimal().IsInteger(out var value))
                return value;

            throw new NotImplementedException("handle floating point value problem");
        }
    }

    public class RangeIndexComposition
    {
        public int? From { get; }
        public int? To { get; }

        public RangeIndexComposition(
            int? from, 
            int? to)
        {
            From = @from;
            To = to;
        }

        public override string ToString()
        {
            return $"{From?.ToString() ?? ""}:{To?.ToString() ?? ""}";
        }
    }
}
