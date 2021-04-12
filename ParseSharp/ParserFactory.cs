using System;
using System.Collections.Generic;

namespace ParseSharp
{
    public static class ParserFactory
    {
        public static Parser<T> Constant<T>(T value)
            => new Parser<T>(input => new ParserResult<T>(value, input));

        public static Parser<string> Match(char c)
            => new Parser<string>(input => input.Match(c, c));

        public static Parser<string> Match(char first, char last)
            => new Parser<string>(input => input.Match(first, last));

        public static Parser<string> Match(string s, StringComparison comparisonType = StringComparison.Ordinal)
            => new Parser<string>(input => input.Match(s, comparisonType));

        public static Parser<string> MatchUntil(string s, StringComparison comparisonType = StringComparison.Ordinal)
            => new Parser<string>(input => input.MatchUntil(s, comparisonType));

        public static Parser<IList<T>> OneOrMore<T>(Parser<T> parser)
            => new Parser<IList<T>>(input =>
            {
                var list = new List<T>();

                var result = parser.Parse(input);
                if (result is null)
                {
                    return null;
                }

                while (true)
                {
                    list.Add(result.Value.Value);
                    input = result.Value.Input;
                    result = parser.Parse(input);
                    if (result is null)
                    {
                        break;
                    }
                }

                return new ParserResult<IList<T>>(list, input);
            });
    }
}