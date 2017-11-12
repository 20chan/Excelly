using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excelly.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Excelly.Execution.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestCategory("Parser")]
        [TestMethod()]
        public void ParseTest()
        {
            AssertExpression(Expression.Constant(0.5),
                Parser.Parse("0.5"));

            AssertExpression(Expression.Constant(1.0),
                Parser.Parse("(((((1)))))"));

            AssertExpression(Expression.UnaryPlus(Expression.Constant(1.0)),
                Parser.Parse("+1"));

            AssertExpression(Expression.Negate(Expression.UnaryPlus(Expression.UnaryPlus(Expression.Constant(0.5)))),
                Parser.Parse("-++0.5"));

            AssertExpression(Expression.Add(Expression.Constant(1.0), Expression.Constant(2.0)),
                Parser.Parse("1+2"));

            AssertExpression(Expression.Add(
                Expression.Constant(1.0),
                Expression.Add(
                    Expression.Constant(2.0),
                    Expression.Add(
                        Expression.Constant(3.0),
                        Expression.Constant(4.0)))),
                Parser.Parse("1+2+3+4"));

            AssertExpression(Expression.Multiply(Expression.Constant(1.0), Expression.Constant(2.0)),
                Parser.Parse("1*2"));

            AssertExpression(Expression.Add(
                Expression.Constant(1.0),
                Expression.Multiply(
                    Expression.Constant(2.0),
                    Expression.Constant(3.0))),
                Parser.Parse("1+2*3"));

            AssertExpression(Expression.Multiply(
                Expression.Add(
                    Expression.Constant(1.0),
                    Expression.Constant(2.0)),
                Expression.Constant(3.0)),
                Parser.Parse("(1+2)*3"));
        }

        public static void AssertExpression(Expression expected, Expression actual)
        {
            if (expected.NodeType != actual.NodeType)
                Assert.Fail();
            if (expected is ConstantExpression)
            {
                var expC = expected as ConstantExpression;
                var actC = actual as ConstantExpression;
                Assert.AreEqual(expC.Value, actC.Value);
            }
            else if (expected is UnaryExpression)
            {
                var expU = expected as UnaryExpression;
                var actU = actual as UnaryExpression;
                AssertExpression(expU.Operand, actU.Operand);
            }
            else if (expected is BinaryExpression)
            {
                var expB = expected as BinaryExpression;
                var actB = actual as BinaryExpression;
                AssertExpression(expB.Left, actB.Left);
                AssertExpression(expB.Right, actB.Right);
            }
        }
    }
}