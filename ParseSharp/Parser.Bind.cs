using System;

namespace ParseSharp
{
    public partial class Parser<T>
    {
        public Parser<U> Bind<U>(Func<T, Parser<U>> nextParser) =>
            new(input =>
            {
                var result = ParseFunc(input);
                if (result is null)
                {
                    return null;
                }
                return nextParser(result.Value).ParseFunc(result.Input);
            });

        public Parser<U> Bind<U>(Func<T, ParserPosition, Parser<U>> nextParser) =>
            new(input =>
            {
                var result = ParseFunc(input);
                if (result is null)
                {
                    return null;
                }
                return nextParser(result.Value, result.Input.Position).ParseFunc(result.Input);
            });
    }
}