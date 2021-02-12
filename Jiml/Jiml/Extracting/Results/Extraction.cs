using System;
using Newtonsoft.Json.Linq;

namespace Jiml.Extracting.Results
{
    public abstract class Extraction
    {
        public Path Path { get; }

        public JToken Value { get; }
        public Jiml.Error Error { get; }
        public bool IsSuccess { get; }

        protected Extraction(Path path, JToken content)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Value = content ?? throw new ArgumentNullException(nameof(content));

            IsSuccess = true;
        }

        protected Extraction(Path path, Jiml.Error error)
        {
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Error = error ?? throw new ArgumentNullException(nameof(error));

            IsSuccess = false;
        }

        public class Failure: Extraction
        {
            public Failure(
                Path path, 
                Jiml.Error error) : base(
                path, 
                error)
            {
            }
        }
    }
}