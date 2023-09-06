namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<string> Whitespace { get; } =
            Match(' ').Or(Match('\t')).Or(Match('\r')).Or(Match('\n'));

        public static Parser<string> SkipWhitespace { get; set; } = ZeroOrMore(Whitespace);
    }
}