using System;

namespace ParseSharp
{
    public class Parser<T>
    {
        internal Func<ParserInput, ParserResult<T>?> Parse { get; }

        internal Parser(Func<ParserInput, ParserResult<T>?> parseFunc)
        {
            Parse = parseFunc;
        }

        public T ParseToEnd(string text)
        {
            var result = Parse(new ParserInput(text));
            if (result is null || !result.Input.IsEndOfInput)
            {
                throw new ArgumentException("Input text did not match.");
            }
            return result.Value;
        }

        public Parser<U> Bind<U>(Func<T, Parser<U>> nextParser)
            => new Parser<U>(input =>
            {
                var result = Parse(input);
                if (result is null)
                {
                    return null;
                }
                return nextParser(result.Value).Parse(result.Input);
            });

        public Parser<U> Map<U>(Func<T, U> mapFunc)
            => Bind(v => ParserFactory.Constant<U>(mapFunc(v)));

        public Parser<T> Or(Parser<T> nextParser)
            => new Parser<T>(input =>
            {
                var result = Parse(input);
                if (result is null)
                {
                    return nextParser.Parse(input);
                }
                return result;
            });

        public Parser<T> And(Parser<T> nextParser)
            => new Parser<T>(input =>
            {
                var result = Parse(input);
                if (result is null)
                {
                    return null;
                }
                return nextParser.Parse(result.Input);
            });
    }
}