namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<T> Constant<T>(T value)
            => new Parser<T>(input => new ParserResult<T>(value, input));
    }
}