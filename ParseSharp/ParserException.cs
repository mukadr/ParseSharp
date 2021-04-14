using System;

namespace ParseSharp
{
    public class ParserException : Exception
    {
        internal ParserException(string message) : base(message) { }
    }
}