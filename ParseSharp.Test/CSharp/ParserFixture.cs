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

            var digit = Match('0', '9');
            var letter = Match('a', 'z').Or(Match('A', 'Z'));

            var number =
                Token(OneOrMore(digit))
                .Map<Expression>(token => new IntExpression(int.Parse(token.Value), token.position));

            var variable =
                Token(letter.Bind(l => ZeroOrMore(letter.Or(digit)).Map(ld => l + ld)))
                .Map<Expression>(token => new VarExpression(token.Value, token.position));

            var factor = number.Or(variable);

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