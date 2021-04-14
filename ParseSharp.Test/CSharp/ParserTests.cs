using Xunit;

namespace ParseSharp.Test.CSharp
{
    public class ParserTests : IClassFixture<ParserFixture>
    {
        private readonly Parser<Ast> _Parser;

        public ParserTests(ParserFixture fixture)
        {
            _Parser = fixture.Parser;
        }

        [Fact]
        public void Parser_Accepts_IntegerExpression()
        {
            var ast = _Parser.ParseAllText("\r\n\r\n\t 135 \r\n");

            var intExpression = Assert.IsType<IntExpression>(ast);

            Assert.Equal(135, intExpression.Value);
            Assert.Equal(3, intExpression.Position!.Value.Line);
        }

        [Fact]
        public void Parser_Accepts_BinaryExpression()
        {
            var ast = _Parser.ParseAllText("10 + 3 * 5");

            var bin = Assert.IsType<BinExpression>(ast);
            Assert.Equal("+", bin.Op);

            var left = Assert.IsType<IntExpression>(bin.Left);
            Assert.Equal(10, left.Value);

            var binRight = Assert.IsType<BinExpression>(bin.Right);
            Assert.Equal("*", binRight.Op);

            var middle = Assert.IsType<IntExpression>(binRight.Left);
            Assert.Equal(3, middle.Value);

            var right = Assert.IsType<IntExpression>(binRight.Right);
            Assert.Equal(5, right.Value);
        }
    }
}