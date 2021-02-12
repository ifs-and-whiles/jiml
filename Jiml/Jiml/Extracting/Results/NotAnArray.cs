﻿using Jiml.Errors;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.Results
{
    public sealed class NotAnArray : Extraction
    {
        public NotAnArray(
            Path path,
            Path previousPath,
            JObject jObject) : base(
            path,
            new VariableWasNotAnArray(
                path,
                previousPath,
                jObject))
        {
        }

        public NotAnArray(
            Path path,
            Path previousPath,
            JValue jValue) : base(
            path,
            new VariableWasNotAnArray(
                path,
                previousPath,
                jValue))
        {
        }
    }
}