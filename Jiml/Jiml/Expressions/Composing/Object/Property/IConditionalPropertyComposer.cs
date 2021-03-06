﻿using Jiml.Expressions.Extracting;
using Jiml.Utils;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Composing.Object.Property
{
    public interface IConditionalPropertyComposer
    {
        Option<PropertyComposition> Compose(
            JObject input,
            Path parentPath);
    }
}