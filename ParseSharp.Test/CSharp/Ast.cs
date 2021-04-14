namespace ParseSharp.Test.CSharp
{
    public abstract class Ast
    {
        public ParserPosition? Position { get; protected set; }
    }

    public abstract class Expression : Ast { }

    public class BinExpression : Expression
    {
        public Expression Left { get; }
        public string Op { get; }
        public Expression Right { get; }

        public BinExpression(Expression left, string op, Expression right, ParserPosition? position = null)
        {
            Left = left;
            Op = op.ToLower();
            Right = right;
            Position = position;
        }
    }

    public class IntExpression : Expression
    {
        public int Value { get; }

        public IntExpression(int value, ParserPosition? position = null)
        {
            Value = value;
            Position = position;
        }
    }
}