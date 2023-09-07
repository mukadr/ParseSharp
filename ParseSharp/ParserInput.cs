using System;
using System.Linq;

namespace ParseSharp;

internal struct ParserInput
{
    private readonly string _text;

    internal ParserPosition Position { get; private set; }

    internal readonly bool IsEndOfInput => Position.Index >= _text.Length;

    internal ParserInput(string text, ParserPosition? position = null)
    {
        _text = text;
        Position = position ?? ParserPosition.Start;
    }

    internal readonly ParserResult<string>? Match(char first, char last)
    {
        if (Position.Index < _text.Length &&
            _text[Position.Index] >= first &&
            _text[Position.Index] <= last)
        {
            return CreateParserResult(_text[Position.Index].ToString());
        }
        return null;
    }

    internal readonly ParserResult<string>? Match(string s, StringComparison comparisonType)
    {
        if (Position.Index < _text.Length - s.Length + 1)
        {
            var lexeme = _text.Substring(Position.Index, s.Length);
            if (lexeme.Equals(s, comparisonType))
            {
                return CreateParserResult(lexeme);
            }
        }
        return null;
    }

    private readonly ParserResult<string> CreateParserResult(string lexeme)
    {
        var newLineCount = lexeme.Count(c => c == '\n');
        var input = new ParserInput(_text, Position.Advance(lexeme.Length, newLineCount));
        return new ParserResult<string>(lexeme, input);
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