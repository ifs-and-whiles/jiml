using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Jiml
{
    public static partial class Jiml
    {
        public class Result
        {
            public JToken Value { get; }

            public Error[] Errors { get; }

            public bool IsSuccess { get; set; }

            public Result(JToken value)
            {
                Value = value;
                IsSuccess = true;
            }

            public Result(IEnumerable<Error> errors)
            {
                Errors = errors.ToArray();
                IsSuccess = false;
            }

            public static implicit operator Result(JToken value) => new Result(value);

            public static implicit operator Result(Error[] errors) => new Result(errors);

            public static implicit operator Result(Error error) => new Result(new[] { error });

            protected bool Equals(Result other)
            {
                return Value?.ToString() == other.Value?.ToString() && ErrorsEquals(Errors, other.Errors);
            }

            private static bool ErrorsEquals(Error[] errors, Error[] others)
            {
                if (errors == null && others == null) return true;
                if (errors == null) return false;
                if (others == null) return false;
                return errors.SequenceEqual(others);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Result)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Value, Errors);
            }
        }
    }
}