﻿using Jiml.Errors;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.Results
{
    public sealed class ParentNotAnObject : Extraction
    {
        public ParentNotAnObject(
            Path path,
            Path previousPath,
            JArray jArray) : base(
            path,
            new ParentVariableWasNotAnObject(
                path,
                previousPath,
                jArray))
        {
        }

        public ParentNotAnObject(
            Path path,
            Path previousPath,
            JValue jValue) : base(
            path,
            new ParentVariableWasNotAnObject(
                path,
                previousPath,
                jValue))
        {
        }
    }
}