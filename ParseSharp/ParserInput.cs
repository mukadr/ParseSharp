using System;
using System.Linq;

namespace ParseSharp
{
    internal struct ParserInput
    {
        private readonly string _text;

        internal readonly ParserPosition Position { get; }

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
            if (Position.Index < _text.Length)
            {
                var index = _text.IndexOf(s, Position.Index, comparisonType);
                if (index >= 0)
                {
                    var lexeme = _text.Substring(Position.Index, s.Length);
                    var lineCount = lexeme.Count(c => c == '\n');
                    var text = new ParserInput(_text, Position.Advance(s.Length, lineCount));
                    return new ParserResult<string>(lexeme, text);
                }
            }
            return null;
        }

        internal ParserResult<string>? MatchUntil(string s, StringComparison comparisonType)
        {
            if (Position.Index < _text.Length)
            {
                var index = _text.IndexOf(s, Position.Index, comparisonType);
                if (index >= 0)
                {
                    var lexeme = _text.Substring(Position.Index, index - Position.Index);
                    var lineCount = lexeme.Count(c => c == '\n');
                    var text = new ParserInput(_text, new ParserPosition(index + s.Length, lineCount));
                    return new ParserResult<string>(lexeme, text);
                }
            }
            return null;
        }
    }
}