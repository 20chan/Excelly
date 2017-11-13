using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excelly.Execution
{
    public enum ExprType
    {
        Add,
        Sub,
        Mul,
        Div,
        Pow,
        UnaryPlus,
        UnaryMinus,
        Constant,
    }
    public abstract class Expr
    {
        public ExprType NodeType { get; private set; }
        public Expr(ExprType type)
        {
            NodeType = type;
        }

        public static Expr Add(Expr left, Expr right)
            => new BinaryExpr(ExprType.Add, left, right);

        public static Expr Subtract(Expr left, Expr right)
            => new BinaryExpr(ExprType.Sub, left, right);

        public static Expr Multiply(Expr left, Expr right)
            => new BinaryExpr(ExprType.Mul, left, right);

        public static Expr Divide(Expr left, Expr right)
            => new BinaryExpr(ExprType.Div, left, right);

        public static Expr Power(Expr left, Expr right)
            => new BinaryExpr(ExprType.Pow, left, right);

        public static Expr UnaryPlus(Expr content)
            => new UnaryExpr(ExprType.UnaryPlus, content);

        public static Expr Negate(Expr content)
            => new UnaryExpr(ExprType.UnaryMinus, content);

        public static Expr Constant(object value)
            => new ConstantExpr(value);
    }

    public class BinaryExpr : Expr
    {
        public Expr Left { get; }
        public Expr Right { get; }

        internal BinaryExpr(ExprType type, Expr left, Expr right) : base(type)
        {
            Left = left;
            Right = right;
        }
    }

    public class UnaryExpr : Expr
    {
        public Expr Content { get; }

        internal UnaryExpr(ExprType type, Expr content) : base(type)
        {
            Content = content;
        }
    }

    public class ConstantExpr : Expr
    {
        public object Value { get; }

        internal ConstantExpr(object value) : base(ExprType.Constant)
        {
            Value = value;
        }
    }
}
