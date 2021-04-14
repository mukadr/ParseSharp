using static ParseSharp.ParserFactory;

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

            var integer = OneOrMore(Match('0', '9'));

            var intExpression = integer.Map((v, position) => new IntExpression(int.Parse(v), position));

            Parser =
                Optional(whitespace)
                .And(intExpression.Map<Ast>(expr => expr))
                .Skip(whitespace);
        }
    }
}