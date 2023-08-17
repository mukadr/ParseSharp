using System.Collections.Generic;
using System.Text;

namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<string> OneOrMore(Parser<string> parser) =>
            new(input =>
            {
                var result = parser.ParseFunc(input);
                if (result is null)
                {
                    return null;
                }

                var sb = new StringBuilder();
                do
                {
                    sb.Append(result.Value);
                    input = result.Input;
                } while ((result = parser.ParseFunc(input)) is not null);

                return new ParserResult<string>(sb.ToString(), input);
            });

        public static Parser<List<T>> OneOrMore<T>(Parser<T> parser) =>
            new(input =>
            {
                var result = parser.ParseFunc(input);
                if (result is null)
                {
                    return null;
                }

                var list = new List<T>();
                do
                {
                    list.Add(result.Value);
                    input = result.Input;
                } while ((result = parser.ParseFunc(input)) is not null);

                return new ParserResult<List<T>>(list, input);
            });
    }
}