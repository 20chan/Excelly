using System.Linq;
using System.Linq.Expressions;

namespace Excelly.Execution
{
    public class Evaluate
    {
        public static object EvaluateExpression(Expression expr)
        {
            return Expression.Lambda(expr, Registered.RegisteredVariable.Select(d => Expression.Parameter(d.Value.GetType(), d.Key)))
                .Compile().DynamicInvoke(Registered.RegisteredVariable.Select(d => d.Value).ToArray());
        }
    }
}
