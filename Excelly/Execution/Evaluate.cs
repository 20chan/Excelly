using System;

namespace Excelly.Execution
{
    public class Evaluate
    {
        public static object Execute(Expr expr)
        {
            if (expr is BinaryExpr)
            {
                var exprB = expr as BinaryExpr;
                dynamic resL = Execute(exprB.Left);
                dynamic resR = Execute(exprB.Right);
                switch (exprB.NodeType)
                {
                    case ExprType.Add:
                        return resL + resR;
                    case ExprType.Sub:
                        return resL - resR;
                    case ExprType.Mul:
                        return resL * resR;
                    case ExprType.Div:
                        return resL / resR;
                    case ExprType.Pow:
                        return Math.Pow(resL, resR);
                    default:
                        throw new Exception("Should not happened");
                }
            }
            else if (expr is UnaryExpr)
            {
                dynamic res = Execute((expr as UnaryExpr).Content);
                return expr.NodeType == ExprType.UnaryPlus ? res : -res;
            }
            else if (expr is ConstantExpr)
            {
                return (expr as ConstantExpr).Value;
            }
            else throw new Exception("Should not happened");
        }
    }
}
