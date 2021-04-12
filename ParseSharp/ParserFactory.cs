using System;
using System.Collections.Generic;
using System.Text;

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

        public static Parser<string> OneOrMore(Parser<string> parser)
            => new Parser<string>(input =>
            {
                var sb = new StringBuilder();

                var result = parser.Parse(input);
                if (result is null)
                {
                    return null;
                }

                while (true)
                {
                    sb.Append(result.Value);
                    input = result.Input;
                    result = parser.Parse(input);
                    if (result is null)
                    {
                        break;
                    }
                }

                return new ParserResult<string>(sb.ToString(), input);
            });

        public static Parser<string> ZeroOrMore(Parser<string> parser)
            => OneOrMore(parser).Or(Constant<string>(string.Empty));

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
                    list.Add(result.Value);
                    input = result.Input;
                    result = parser.Parse(input);
                    if (result is null)
                    {
                        break;
                    }
                }

                return new ParserResult<IList<T>>(list, input);
            });

        public static Parser<IList<T>> ZeroOrMore<T>(Parser<T> parser)
            => OneOrMore(parser).Or(Constant<IList<T>>(new List<T>()));

        public static Parser<T?> Optional<T>(Parser<T> parser) where T : class
            => parser.Map(value => (T?)value).Or(Constant<T?>(null));

        public static Parser<T?> Not<T>(Parser<T> parser) where T : class
            => new Parser<T?>(input =>
            {
                var result = parser.Parse(input);
                if (result is null)
                {
                    return new ParserResult<T?>(null, input);
                }
                return null;
            });

        public static Parser<string> Until<T>(Parser<T> parser)
            => new Parser<string>(input =>
            {
                var sb = new StringBuilder();

                ParserResult<T>? result;
                while (true)
                {
                    result = parser.Parse(input);
                    if (result is not null)
                    {
                        break;
                    }

                    var ch = input.NextChar();
                    if (ch is null)
                    {
                        return null;
                    }
                    sb.Append(ch);
                }

                return new ParserResult<string>(sb.ToString(), result.Input);
            });
    }
}