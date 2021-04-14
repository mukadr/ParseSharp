using System;

namespace ParseSharp
{
    public partial class Parser<T>
    {
        public Parser<U> Map<U>(Func<T, U> map)
            => Bind(v => Parser.Constant<U>(map(v)));

        public Parser<U> Map<U>(Func<T, ParserPosition, U> map)
            => Bind((v, pos) => Parser.Constant<U>(map(v, pos)));
    }
}