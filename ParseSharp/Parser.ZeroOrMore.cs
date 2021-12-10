using System.Collections.Generic;

namespace ParseSharp
{
    public static partial class Parser
    {
        public static Parser<string> ZeroOrMore(Parser<string> parser)
            => OneOrMore(parser).Or(Constant<string>(string.Empty));

        public static Parser<List<T>> ZeroOrMore<T>(Parser<T> parser)
            => OneOrMore(parser).Or(Constant<List<T>>(new List<T>()));
    }
}