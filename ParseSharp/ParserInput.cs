using System;
using System.Linq;

namespace ParseSharp
{
    internal struct ParserInput
    {
        private readonly string _text;

        internal ParserPosition Position { get; private set; }

        internal bool IsEndOfInput => Position.Index >= _text.Length;

        internal ParserInput(string text, ParserPosition? position = null)
        {
            _text = text;
            Position = position ?? ParserPosition.Start;
        }

        internal ParserResult<string>? Match(char first, char last)
        {
            if (Position.Index < _text.Length &&
                _text[Position.Index] >= first &&
                _text[Position.Index] <= last)
            {
                var lexeme = _text[Position.Index].ToString();
                var text = new ParserInput(_text, Position.Advance(1, lexeme == "\n" ? 1 : 0));
                return new ParserResult<string>(lexeme, text);
            }
            return null;
        }

        internal ParserResult<string>? Match(string s, StringComparison comparisonType)
        {
            if (Position.Index < _text.Length - s.Length + 1)
            {
                var lexeme = _text.Substring(Position.Index, s.Length);
                if (lexeme.Equals(s, comparisonType))
                {
                    var lineCount = lexeme.Count(c => c == '\n');
                    var text = new ParserInput(_text, Position.Advance(s.Length, lineCount));
                    return new ParserResult<string>(lexeme, text);
                }
            }
            return null;
        }

        internal char? NextChar()
        {
            if (Position.Index < _text.Length)
            {
                var ch = _text[Position.Index];
                Position = Position.Advance(1, ch == '\n' ? 1 : 0);
                return ch;
            }
            return null;
        }
    }
}