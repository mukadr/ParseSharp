using System;
using System.Collections.Generic;
using Xunit;
using static ParseSharp.ParserFactory;

namespace ParseSharp.Test
{
    public class SimpleTests
    {
        [Fact]
        public void Parser_Throws_OnMatchFailure()
        {
            Assert.Throws<ArgumentException>(() => Match('a').ParseAllText("x"));
            Assert.Throws<ArgumentException>(() => Match("a").ParseAllText("x"));
        }

        [Fact]
        public void Parser_Throws_WhenNotCompleted()
        {
            Assert.Throws<ArgumentException>(() => Match('a').ParseAllText("ab"));
            Assert.Throws<ArgumentException>(() => Match("a").ParseAllText("ab"));
        }

        [Fact]
        public void Match_Counts_LineNumber()
        {
            const string source = "\n\n\n\nx\n\n";

            Parser<ParserPosition> makeParser(Parser<string> newLineParser)
                => newLineParser.And(Match('x').Map((_, pos) => pos)).Skip(newLineParser);

            var matchChar = makeParser(ZeroOrMore(Match('\n')));
            var matchString = makeParser(ZeroOrMore(Match("\n")));
            var matchUntil = Until(Match('x')).Map((_, pos) => pos).Skip(ZeroOrMore(Match('\n')));

            Assert.Equal(5, matchChar.ParseAllText(source).Line);
            Assert.Equal(5, matchString.ParseAllText(source).Line);
            Assert.Equal(5, matchUntil.ParseAllText(source).Line);
        }

        [Fact]
        public void Match_Accepts_Character()
        {
            Match('a').ParseAllText("a");
        }

        [Fact]
        public void Match_Accepts_CharacterRange()
        {
            var parser = Match('a', 'z');

            for (var c = 'a'; c <= 'z'; c++)
            {
                parser.ParseAllText(c.ToString());
            }
        }

        [Fact]
        public void Match_Accepts_String()
        {
            Match("Hello").ParseAllText("Hello");
        }

        [Fact]
        public void Match_Accepts_StringIgnoreCase()
        {
            Match("HelloWorld", StringComparison.OrdinalIgnoreCase).ParseAllText("HELLOWORLD");
        }

        [Fact]
        public void Constant_Returns_Value()
        {
            Assert.Equal(3, Constant(3).ParseAllText(""));
        }

        [Fact]
        public void Bind_Executes_NextParser()
        {
            Match('0', '9').Bind(n => Match('h')).ParseAllText("5h");
        }

        [Fact]
        public void Map_Maps_ParsedValue()
        {
            var parser = Match('0', '9').Map(v => int.Parse(v));

            Assert.Equal(5, parser.ParseAllText("5"));
        }

        [Fact]
        public void Or_Accepts_FirstOrSecondParser()
        {
            var parser = Match("a").Or(Match("b"));

            Assert.Equal("a", parser.ParseAllText("a"));
            Assert.Equal("b", parser.ParseAllText("b"));
        }

        [Fact]
        public void And_Accepts_BothParsers()
        {
            var parser = Match("a").And(Match("b"));

            Assert.Throws<ArgumentException>(() => parser.ParseAllText("a"));
            Assert.Throws<ArgumentException>(() => parser.ParseAllText("b"));
            Assert.Equal("b", parser.ParseAllText("ab"));
        }

        [Fact]
        public void OneOrMore_Accepts_RepeatedInput()
        {
            var stringParser = OneOrMore(Match('0', '9'));
            var intParser = OneOrMore(Match('0', '9').Map(s => int.Parse(s)));

            Assert.Equal("2021", stringParser.ParseAllText("2021"));
            Assert.Equal(new List<int> { 2, 0, 2, 1 }, intParser.ParseAllText("2021"));
        }

        [Fact]
        public void OneOrMore_Rejects_MissingInput()
        {
            Assert.Throws<ArgumentException>(() => OneOrMore(Match('x')).ParseAllText(""));
        }

        [Fact]
        public void ZeroOrMore_Accepts_RepeatedInput()
        {
            var stringParser = ZeroOrMore(Match('0', '9'));
            var intParser = ZeroOrMore(Match('0', '9').Map(s => int.Parse(s)));

            Assert.Equal("2021", stringParser.ParseAllText("2021"));
            Assert.Equal(new List<int> { 2, 0, 2, 1 }, intParser.ParseAllText("2021"));
        }

        [Fact]
        public void ZeroOrMore_Accepts_MissingInput()
        {
            Assert.NotNull(ZeroOrMore(Match('x')).ParseAllText(""));
        }

        [Fact]
        public void Skip_Accepts_Input()
        {
            var parser = Match('a').Skip(Match('b')).Bind(a => Match('c').Map(c => a + c));

            Assert.Equal("ac", parser.ParseAllText("abc"));
        }

        [Fact]
        public void Optional_Accepts_Input()
        {
            var parser = Optional(Match('a'));

            Assert.Equal("a", parser.ParseAllText("a"));
        }

        [Fact]
        public void Optional_Accepts_MissingInput()
        {
            var parser = Optional(Match('a'));

            Assert.Null(parser.ParseAllText(""));
        }

        [Fact]
        public void Not_Rejects_Parser()
        {
            var parser = Not(Match('a')).Bind(_ => Match('b'));

            Assert.Equal("b", parser.ParseAllText("b"));
            Assert.Throws<ArgumentException>(() => parser.ParseAllText("ab"));
        }

        [Fact]
        public void Until_Accepts_WhenEndIsFound()
        {
            var result = Until(Match('a').Bind(a => Match('b').Bind(b => Match('c').Map(c => a + b + c)))).ParseAllText("123abc");

            Assert.Equal("123", result.Prefix);
            Assert.Equal("abc", result.End);
        }
    }
}