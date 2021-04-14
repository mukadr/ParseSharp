namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<(string Value, ParserPosition position)> Token(string token)
            => Match(token).Map((value, position) => (value, position)).Skip(Whitespace);

        public static Parser<(T Value, ParserPosition position)> Token<T>(Parser<T> tokenParser)
            => tokenParser.Map((value, position) => (value, position)).Skip(Whitespace);
    }
}