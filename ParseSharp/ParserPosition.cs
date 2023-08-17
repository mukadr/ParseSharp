namespace ParseSharp
{
    public struct ParserPosition
    {
        public static readonly ParserPosition Start = new(0, 1);

        public int Index { get; private set; }

        public int Line { get; private set; }

        public ParserPosition(int index, int line)
        {
            Index = index;
            Line = line;
        }

        public readonly ParserPosition Advance(int charCount, int newLineCount) => new(Index + charCount, Line + newLineCount);
    }
}