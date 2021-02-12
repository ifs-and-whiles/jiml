using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.Results
{
    public sealed class CorrectExtraction : Extraction
    {
        public CorrectExtraction(
            Path path, 
            JToken content) : base(
            path, 
            content)
        {
        }
    }
}