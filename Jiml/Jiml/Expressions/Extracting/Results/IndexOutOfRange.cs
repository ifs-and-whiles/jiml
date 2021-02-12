using System.Collections.Generic;
using System.Linq;
using Jiml.Expressions.Errors;
using Newtonsoft.Json.Linq;

namespace Jiml.Expressions.Extracting.Results
{
    public sealed class IndexOutOfRange : Extraction
    {
        public IndexOutOfRange(
            Path path,
            Path previousPath,
            JArray jArray,
            IEnumerable<int> wrongIndexes) : base(
            path,
            new IndexWasOutOfRange(
                path,
                previousPath,
                jArray.Count,
                wrongIndexes.ToArray()))
        {
        }
    }
}