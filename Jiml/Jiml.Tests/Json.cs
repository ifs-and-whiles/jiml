using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jiml.Tests
{
    public class Json
    {
        public static JObject From(object @object)
        {
            var serializeObject = JsonConvert.SerializeObject(@object);
            return JObject.Parse(serializeObject);
        }
    }
}