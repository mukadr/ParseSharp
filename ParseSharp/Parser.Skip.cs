namespace ParseSharp
{
    public partial class Parser<T>
    {
        public Parser<T> Skip<U>(Parser<U> next)
            => new Parser<T>(input =>
            {
                var result = ParseFunc(input);
                if (result is null)
                {
                    return result;
                }

                var consumed = next.ParseFunc(result.Input);
                if (consumed is null)
                {
                    return result;
                }

                return new ParserResult<T>(result.Value, consumed.Input);
            });
    }
}