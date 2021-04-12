using System;
using Xunit;
using static ParseSharp.ParserFactory;

namespace ParseSharp.Test
{
    public class SimpleParsingTests
    {
        [Fact]
        public void Parser_Throws_OnMatchFailure()
        {
            Assert.Throws<ArgumentException>(() => Match('a').ParseToEnd("x"));
            Assert.Throws<ArgumentException>(() => Match('a', 'f').ParseToEnd("x"));
            Assert.Throws<ArgumentException>(() => Match("a").ParseToEnd("x"));
            Assert.Throws<ArgumentException>(() => MatchUntil("a").ParseToEnd("x"));
        }

        [Fact]
        public void Parser_Throws_WhenNotCompleted()
        {
            Assert.Throws<ArgumentException>(() => Match('a').ParseToEnd("ab"));
            Assert.Throws<ArgumentException>(() => Match('a', 'f').ParseToEnd("ab"));
            Assert.Throws<ArgumentException>(() => Match("a").ParseToEnd("ab"));
            Assert.Throws<ArgumentException>(() => MatchUntil("a").ParseToEnd("ab"));
        }

        [Fact]
        public void Match_Accepts_Character()
        {
            Match('a').ParseToEnd("a");
        }

        [Fact]
        public void Match_Accepts_CharacterRange()
        {
            var parser = Match('a', 'z');

            for (var c = 'a'; c <= 'z'; c++)
            {
                parser.ParseToEnd(c.ToString());
            }
        }

        [Fact]
        public void Match_Accepts_String()
        {
            Match("Hello").ParseToEnd("Hello");
        }

        [Fact]
        public void Match_Accepts_StringIgnoreCase()
        {
            Match("HelloWorld", StringComparison.OrdinalIgnoreCase).ParseToEnd("HELLOWORLD");
        }

        [Fact]
        public void MatchUntil_Accepts_UpToTerminator()
        {
            Assert.Equal("123", MatchUntil("abc").ParseToEnd("123abc"));
        }

        [Fact]
        public void MatchUntil_Accepts_UpToTerminatorIgnoreCase()
        {
            Assert.Equal("123", MatchUntil("abc", StringComparison.OrdinalIgnoreCase).ParseToEnd("123ABC"));
        }

        [Fact]
        public void Constant_Returns_Value()
        {
            Assert.Equal(3, Constant(3).ParseToEnd(""));
        }

        [Fact]
        public void Bind_Executes_NextParser()
        {
            Match('0', '9').Bind(n => Match('h')).ParseToEnd("5h");
        }

        [Fact]
        public void Map_Maps_ParsedValue()
        {
            var parser = Match('0', '9').Map(v => int.Parse(v));

            Assert.Equal(5, parser.ParseToEnd("5"));
        }

        [Fact]
        public void Or_Accepts_FirstOrSecondParser()
        {
            var parser = Match("a").Or(Match("b"));

            parser.ParseToEnd("a");
            parser.ParseToEnd("b");
        }

        [Fact]
        public void And_Accepts_BothParsers()
        {
            var parser = Match("a").And(Match("b"));

            parser.ParseToEnd("ab");
        }

        [Fact]
        public void OneOrMore_Accepts_RepeatedInput()
        {
            var parser = OneOrMore(Match('0', '9'));

            parser.ParseToEnd("2021");
        }
    }
}