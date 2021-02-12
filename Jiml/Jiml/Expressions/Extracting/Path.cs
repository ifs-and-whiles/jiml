using System;

namespace Jiml.Expressions.Extracting
{
    public class Path
    {
        public string Value { get; }

        private Path(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static Path From(string segment) => new Path(segment);
        public static Path Empty => new Path("");
        public static Path Root => new Path("<root>");

        public bool IsEmpty => string.IsNullOrWhiteSpace(Value);

        public Path AddSegment(string segment)
        {
            return IsEmpty 
                ? new Path(segment) 
                : new Path($"{Value}.{segment}");
        }

        public Path Append(string appendix)
        {
            return new Path($"{Value}{appendix}");
        }
        
        public static implicit operator string(Path path) => path.Value;

        public override string ToString() => Value;
    }
}