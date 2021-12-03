using Xunit;

namespace ParseSharp.Test.CSharp
{
    public class ParserTests : IClassFixture<ParserFixture>
    {
        private readonly Parser<Ast> _parser;

        public ParserTests(ParserFixture fixture)
        {
            _parser = fixture.Parser;
        }

        [Fact]
        public void Parser_Accepts_IntegerExpression()
        {
            var ast = _parser.ParseAllText("\r\n\r\n\t 135 \r\n");

            var intExpression = Assert.IsType<IntExpression>(ast);

            Assert.Equal(135, intExpression.Value);
            Assert.Equal(3, intExpression.Position!.Value.Line);
        }

        [Fact]
        public void Parser_Accepts_VarExpression()
        {
            var ast = _parser.ParseAllText("\r\n\r\n\t D3adB33f \r\n");

            var varExpression = Assert.IsType<VarExpression>(ast);

            Assert.Equal("D3adB33f", varExpression.Name);
            Assert.Equal(3, varExpression.Position!.Value.Line);
        }

        [Fact]
        public void Parser_Accepts_BinaryExpression()
        {
            var ast = _parser.ParseAllText("x + 3 * y");

            var bin = Assert.IsType<BinExpression>(ast);
            Assert.Equal("+", bin.Op);

            var left = Assert.IsType<VarExpression>(bin.Left);
            Assert.Equal("x", left.Name);

            var binRight = Assert.IsType<BinExpression>(bin.Right);
            Assert.Equal("*", binRight.Op);

            var middle = Assert.IsType<IntExpression>(binRight.Left);
            Assert.Equal(3, middle.Value);

            var right = Assert.IsType<VarExpression>(binRight.Right);
            Assert.Equal("y", right.Name);
        }
    }
}