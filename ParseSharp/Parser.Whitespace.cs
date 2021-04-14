namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<string> Whitespace { get; }
            = ZeroOrMore(Match(' ').Or(Match('\t')).Or(Match('\r')).Or(Match('\n')));
    }
}