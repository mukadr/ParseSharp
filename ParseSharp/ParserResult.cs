namespace ParseSharp
{
    internal class ParserResult<T>
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