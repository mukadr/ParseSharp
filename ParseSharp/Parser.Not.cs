namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<T?> Not<T>(Parser<T> parser) where T : class
            => new Parser<T?>(input =>
            {
                var result = parser.ParseFunc(input);
                if (result is null)
                {
                    return new ParserResult<T?>(null, input);
                }
                return null;
            });
    }
}