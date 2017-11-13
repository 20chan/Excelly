using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excelly.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Excelly.Execution.Tests
{
    [TestClass()]
    public class EvaluateTests
    {
        [Registered("Add")]
        public static double Add(params double[] nums) => nums.Sum();

        [TestCategory("Evaluate")]
        [TestMethod()]
        public void EvaluateExpressionTest()
        {
            Registered.RegisterAll(typeof(EvaluateTests));

            var expr = Expression.Invoke(Expression.Parameter(typeof(Func<double[], double>), "Add"), 
                Expression.NewArrayInit(typeof(double), Expression.Constant(1.0, typeof(double)), Expression.Constant(2.0, typeof(double))));
            
            // Does not work!
            var comp = Expression.Lambda(expr, Expression.Parameter(typeof(Func<double[], double>), "Add")).Compile();
            var res = comp.DynamicInvoke(new Func<double[], double>(Add));

            Assert.AreEqual(3.0, Evaluate.EvaluateExpression(expr));
        }
    }
}