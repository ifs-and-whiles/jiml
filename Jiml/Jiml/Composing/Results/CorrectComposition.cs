using Newtonsoft.Json.Linq;

namespace Jiml.Composing.Results
{
    public class CorrectComposition : Composition
    {
        public CorrectComposition(JToken content) : base(content)
        {
        }
    }
}
