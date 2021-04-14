using System.Linq;
using static ParseSharp.Parser;

namespace ParseSharp.Test.CSharp
{
    public class ParserFixture
    {
        public Parser<Ast> Parser { get; }

        public ParserFixture()
        {
            var whitespace =
                OneOrMore(
                    Match(' ')
                    .Or(Match('\t'))
                    .Or(Match('\n'))
                    .Or(Match('\r')));

            Parser<(string Lexeme, ParserPosition Position)> OperatorParser(string op)
                => Match(op).Map((lexeme, position) => (lexeme, position)).Skip(whitespace);

            var minus = OperatorParser("-");
            var plus = OperatorParser("+");
            var star = OperatorParser("*");

            var integer = OneOrMore(Match('0', '9'));

            var intExpression =
                integer.Map<Expression>((lexeme, position) =>
                    new IntExpression(int.Parse(lexeme), position))
                .Skip(whitespace);

            Parser<Expression> BinExpressionParser(
                Parser<Expression> expression,
                Parser<(string Lexeme, ParserPosition Position)> @operator
            ) =>
                expression.Bind(first =>
                    ZeroOrMore(@operator.Bind(op =>
                        expression.Map(right =>
                            (op, right))))
                    .Map(expressions => expressions.Aggregate(first, (left, opRight) =>
                        new BinExpression(left, opRight.op.Lexeme, opRight.right, opRight.op.Position))));

            var timesExpression = Forward<Expression>();

            var plusExpression = BinExpressionParser(timesExpression, plus.Or(minus));

            timesExpression.Attach(BinExpressionParser(intExpression, star));

            Parser = Optional(whitespace).And(plusExpression).Map<Ast>(expr => expr);
        }
    }
}