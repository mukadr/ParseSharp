using System;

namespace ParseSharp
{
    public class ParserException : Exception
    {
        public ParserException(string message) : base(message) { }
    }
}