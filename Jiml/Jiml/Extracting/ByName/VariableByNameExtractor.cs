using System;
using Jiml.Extracting.Results;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.ByName
{
    public class VariableByNameExtractor : IExtractor
    {
        private readonly string _variableName;

        public VariableByNameExtractor(
            string variableName)
        {
            _variableName = variableName;
        }

        public Extraction ExtractFrom(
            Context context)
        {
            var path = context
                .Path
                .AddSegment(_variableName);

            return context.Value switch
            {
                JArray jArray => new ParentNotAnObject(
                    path: path,
                    previousPath: context.Path,
                    jArray: jArray),

                JValue jValue => jValue.Value != null
                    ? new ParentNotAnObject(
                        path: path,
                        previousPath: context.Path,
                        jValue: jValue)
                    : (Extraction) new ParentNull(
                        path: path,
                        previousPath: context.Path),

                JObject jObject => TryExtractFromObject(
                    path,
                    jObject),

                _ => throw new InvalidOperationException(
                    $"Unexpected JToken implementation '{context.Value.GetType()}'")
            };
        }

        private Extraction TryExtractFromObject(
            Path path, 
            JObject jObject)
        {
            var jPropertyOption = jObject.TryGetProperty(
                _variableName);

            return jPropertyOption.Match<Extraction>(
                jProperty => new CorrectExtraction(
                    path,
                    jProperty.Value),
                () => new NotFound(
                    path));
        }

    }
}