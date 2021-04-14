namespace ParseSharp
{
    public partial class Parser<T>
    {
        public Parser<U> And<U>(Parser<U> nextParser)
            => new Parser<U>(input =>
            {
                var result = ParseFunc(input);
                if (result is null)
                {
                    return null;
                }
                return nextParser.ParseFunc(result.Input);
            });
    }
}