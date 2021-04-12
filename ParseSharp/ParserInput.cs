using System;

namespace ParseSharp
{
    internal struct ParserInput
    {
        private readonly string _text;

        internal readonly ParserPosition _position;

        internal bool IsEndOfInput => _position.Index >= _text.Length;

        internal ParserInput(string text, ParserPosition? position = null)
        {
            _text = text;
            _position = position ?? ParserPosition.Start;
        }

        internal ParserResult<string>? Match(char first, char last)
        {
            if (_position.Index < _text.Length &&
                _text[_position.Index] >= first &&
                _text[_position.Index] <= last)
            {
                var lexeme = _text[_position.Index].ToString();
                var text = new ParserInput(_text, _position.Advance(1));
                return new ParserResult<string>(lexeme, text);
            }
            return null;
        }

        internal ParserResult<string>? Match(string s, StringComparison comparisonType)
        {
            if (_position.Index < _text.Length)
            {
                var index = _text.IndexOf(s, _position.Index, comparisonType);
                if (index >= 0)
                {
                    var lexeme = _text.Substring(_position.Index, s.Length);
                    var text = new ParserInput(_text, _position.Advance(s.Length));
                    return new ParserResult<string>(lexeme, text);
                }
            }
            return null;
        }

        internal ParserResult<string>? MatchUntil(string last, StringComparison comparisonType)
        {
            if (_position.Index < _text.Length)
            {
                var index = _text.IndexOf(last, _position.Index, comparisonType);
                if (index >= 0)
                {
                    var lexeme = _text.Substring(_position.Index, index - _position.Index);
                    var text = new ParserInput(_text, new ParserPosition(index + last.Length));
                    return new ParserResult<string>(lexeme, text);
                }
            }
            return null;
        }
    }
}