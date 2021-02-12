using System;
using Jiml.Expressions.Extracting.Results;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.ByIndex
{
    public abstract class JArrayExtractor : IExtractor
    {
        protected abstract Path GetPath(
            Context context);

        protected abstract Extraction ExtractFromJArray(
            JArrayContext jArrayContext);

        public Extraction ExtractFrom(Context context)
        {
            switch (context.Value)
            {
                case JArray jArray:
                    return ExtractFromJArray(
                        new JArrayContext(
                            context.Input,
                            jArray,
                            context.Path));

                case JValue jValue:
                    return new NotAnArray(
                        GetPath(context),
                        context.Path,
                        jValue);

                case JObject jObject:
                    return new NotAnArray(
                        GetPath(context),
                        context.Path,
                        jObject);

                default:
                    throw new InvalidOperationException(
                        $"Unexpected JToken implementation '{context.Value.GetType()}'");
            }
        }

        protected class JArrayContext
        {
            public JObject Input { get; }
            public JArray Value { get; }
            public Path Path { get; }

            public JArrayContext(
                JObject input,
                JArray jArray, 
                Path path)
            {
                Value = jArray ?? throw new ArgumentNullException(nameof(jArray));
                Path = path ?? throw new ArgumentNullException(nameof(path));
                Input = input ?? throw new ArgumentNullException(nameof(input));
            }
        }
    }
}