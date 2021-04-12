namespace ParseSharp
{
    internal struct ParserResult<T>
    {
        internal T Value { get; }

        internal ParserInput Input { get; }

        internal ParserResult(T value, ParserInput input)
        {
            Value = value;
            Input = input;
        }
    }
}