using System;

namespace ParseSharp
{
    public partial class Parser<T>
    {
        internal Func<ParserInput, ParserResult<T>?> ParseFunc { get; private set; }

        internal Parser(Func<ParserInput, ParserResult<T>?> parseFunc)
        {
            ParseFunc = parseFunc;
        }

        public T ParseAllText(string text)
        {
            var result = ParseFunc(new ParserInput(text));
            if (result is null || !result.Input.IsEndOfInput)
            {
                throw new ParserException("Input text did not match.");
            }
            return result.Value;
        }

        public void Attach(Parser<T> parser)
        {
            ParseFunc = parser.ParseFunc;
        }
    }
}