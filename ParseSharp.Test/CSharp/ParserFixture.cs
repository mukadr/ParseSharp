using static ParseSharp.Parser;

namespace ParseSharp.Test.CSharp
{
    public class ParserFixture
    {
        public Parser<Ast> Parser { get; }

        public ParserFixture()
        {
            var plus = Token("+");
            var minus = Token("-");
            var star = Token("*");
            var div = Token("/");

            var factor = Token(OneOrMore(Match('0', '9'))).Map<Expression>(token =>
                new IntExpression(int.Parse(token.Value), token.position));

            var timesExpression = Forward<Expression>();

            var plusExpression = BinaryExpression(
                timesExpression,
                plus.Or(minus),
                (left, op, right, position) => new BinExpression(left, op, right, position)
            );

            timesExpression.Attach(BinaryExpression(
                factor,
                star.Or(div),
                (left, op, right, position) => new BinExpression(left, op, right, position))
            );

            Parser = Optional(Whitespace).And(plusExpression).Map<Ast>(expr => expr);
        }
    }
}