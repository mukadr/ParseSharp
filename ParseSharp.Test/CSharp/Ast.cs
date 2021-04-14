namespace ParseSharp.Test.CSharp
{
    public abstract class Ast
    {
        public ParserPosition? Position { get; }

        protected Ast(ParserPosition? position)
        {
            Position = position;
        }
    }

    public class IntExpression : Ast
    {
        public int Value { get; }

        public IntExpression(int value, ParserPosition? position = null)
            : base(position)
        {
            Value = value;
        }
    }
}