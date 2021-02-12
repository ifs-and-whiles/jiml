using System;
using System.Collections.Generic;

namespace Jiml.Antlr
{
    public class AntlrException : AggregateException
    {
        public AntlrException(string message, IEnumerable<Exception> innerExceptions) : base(message, innerExceptions)
        {
        }
    }
}