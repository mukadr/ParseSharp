namespace ParseSharp
{
    public partial class Parser<T>
    {
        public Parser<T> Or(Parser<T> nextParser)
            => new Parser<T>(input =>
            {
                var result = ParseFunc(input);
                if (result is null)
                {
                    return nextParser.ParseFunc(input);
                }
                return result;
            });
    }
}