using System;
using System.Collections.Generic;

namespace Jiml
{
    public class AntlrException : AggregateException
    {
        public AntlrException(string message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions)
        {
        }
    }
}