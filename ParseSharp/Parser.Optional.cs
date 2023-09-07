namespace ParseSharp;

public static partial class Parser
{
    public static Parser<T?> Optional<T>(Parser<T> parser) where T : class =>
        parser.Map(value => (T?)value).Or(Constant<T?>(null));
}