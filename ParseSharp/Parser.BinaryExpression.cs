using System;
using System.Linq;

namespace ParseSharp;

public static partial class Parser
{
    public static Parser<TExpression> BinaryExpression<TExpression>(
        Parser<TExpression> expressionParser,
        Parser<(string Value, ParserPosition Position)> operatorParser,
        Func<TExpression, string, TExpression, ParserPosition, TExpression> createExpressionFunc
    ) =>
        expressionParser.Bind(first =>
            ZeroOrMore(operatorParser.Bind(op =>
                expressionParser.Map(right =>
                    (op, right))))
            .Map(expressions => expressions.Aggregate(first, (left, opRight) =>
                createExpressionFunc(left, opRight.op.Value, opRight.right, opRight.op.Position))));
}