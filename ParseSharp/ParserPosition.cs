namespace ParseSharp
{
    public struct ParserPosition
    {
        public static readonly ParserPosition Start = new ParserPosition(0);

        public int Index { get; private set; }

        public ParserPosition(int index)
        {
            Index = index;
        }

        public ParserPosition Advance(int count) => new ParserPosition(Index + count);
    }
}