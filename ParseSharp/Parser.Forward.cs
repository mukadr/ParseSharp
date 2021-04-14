namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<T> Forward<T>()
            => new Parser<T>(input => throw new ParserException("Forward parser should not be called."));
    }
}