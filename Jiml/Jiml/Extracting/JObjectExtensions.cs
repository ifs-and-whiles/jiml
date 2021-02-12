using System;
using System.Linq;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting
{
    public static class JObjectExtensions
    {
        public static Option<JProperty> TryGetProperty(
            this JObject jObject,
            string name)
        {
            return jObject
                .Properties()
                .FirstOrDefault(jProperty => string.Equals(
                    jProperty.Name,
                    name,
                    StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
