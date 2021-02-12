using System;

namespace Jiml
{
    public static partial class Jiml
    {
        public abstract class Error
        {
            public string Message { get; }
            public string Code { get; }
            public Error[] InnerErrors { get; }

            protected Error(
                string message, 
                string code, 
                Error[] innerErrors = null)
            {
                Message = message;
                Code = code;
                InnerErrors = innerErrors ?? new Error[0];
            }

            public bool Equals(Error other)
            {
                return Message == other.Message && Code == other.Code;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Error)obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Message, Code);
            }

            public override string ToString()
            {
                return $"{Code}: {Message}";
            }

            public class Generic: Error
            {
                public Generic(string message, string code) : base(message, code, new Error[0])
                {
                }
            }
        }
    }
}
