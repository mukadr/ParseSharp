using System.Collections.Generic;
using System.Text;

namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<string> OneOrMore(Parser<string> parser)
            => new Parser<string>(input =>
            {
                var sb = new StringBuilder();

                var result = parser.ParseFunc(input);
                if (result is null)
                {
                    return null;
                }

                do
                {
                    sb.Append(result.Value);
                    input = result.Input;
                } while ((result = parser.ParseFunc(input)) is not null);

                return new ParserResult<string>(sb.ToString(), input);
            });

        public static Parser<IList<T>> OneOrMore<T>(Parser<T> parser)
            => new Parser<IList<T>>(input =>
            {
                var list = new List<T>();

                var result = parser.ParseFunc(input);
                if (result is null)
                {
                    return null;
                }

                do
                {
                    list.Add(result.Value);
                    input = result.Input;
                } while ((result = parser.ParseFunc(input)) is not null);

                return new ParserResult<IList<T>>(list, input);
            });
    }
}