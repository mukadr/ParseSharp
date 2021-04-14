using System.Text;

namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<(string Prefix, T End)> Until<T>(Parser<T> parser)
            => new Parser<(string Prefix, T End)>(input =>
            {
                var sb = new StringBuilder();

                ParserResult<T>? result;
                while ((result = parser.ParseFunc(input)) is null)
                {
                    var ch = input.NextChar();
                    if (ch is null)
                    {
                        return null;
                    }
                    sb.Append(ch);
                }

                return new ParserResult<(string Prefix, T End)>((sb.ToString(), result.Value), result.Input);
            });
    }
}