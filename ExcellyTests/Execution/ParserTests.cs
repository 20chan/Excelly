using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excelly.Execution;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Excelly.Execution.Tests
{
    [TestClass()]
    public class ParserTests
    {
        [TestCategory("Parser")]
        [TestMethod()]
        public void ParseTest()
        {
            AssertExpr(Expr.Constant(0.5),
                Parser.Parse("0.5"));

            AssertExpr(Expr.Constant(1.0),
                Parser.Parse("(((((1)))))"));

            AssertExpr(Expr.UnaryPlus(Expr.Constant(1.0)),
                Parser.Parse("+1"));

            AssertExpr(Expr.Negate(Expr.UnaryPlus(Expr.UnaryPlus(Expr.Constant(0.5)))),
                Parser.Parse("-++0.5"));

            AssertExpr(Expr.Add(Expr.Constant(1.0), Expr.Constant(2.0)),
                Parser.Parse("1+2"));

            AssertExpr(Expr.Add(
                Expr.Constant(1.0),
                Expr.Add(
                    Expr.Constant(2.0),
                    Expr.Add(
                        Expr.Constant(3.0),
                        Expr.Constant(4.0)))),
                Parser.Parse("1+2+3+4"));

            AssertExpr(Expr.Multiply(Expr.Constant(1.0), Expr.Constant(2.0)),
                Parser.Parse("1*2"));

            AssertExpr(Expr.Add(
                Expr.Constant(1.0),
                Expr.Multiply(
                    Expr.Constant(2.0),
                    Expr.Constant(3.0))),
                Parser.Parse("1+2*3"));

            AssertExpr(Expr.Multiply(
                Expr.Add(
                    Expr.Constant(1.0),
                    Expr.Constant(2.0)),
                Expr.Constant(3.0)),
                Parser.Parse("(1+2)*3"));

            AssertExpr(Expr.Function("do", new Expr[] { }),
                Parser.Parse("do()"));

            AssertExpr(Expr.Add(
                Expr.Constant(1.0),
                Expr.Function("PI", new Expr[] { })),
                Parser.Parse("1 + PI()"));

            AssertExpr(Expr.Function("print", new Expr[] {
                Expr.Function("plus", new Expr[] {
                    Expr.Constant(1.0),
                    Expr.Constant(1.0)
                    })
                }),
                Parser.Parse("print(plus(1, 1))"));
        }

        [TestCategory("Parser")]
        [TestMethod()]
        public void ParseErrTest()
        {
            try
            {
                AssertExpr(Expr.Constant(1.0),
                    Parser.Parse("1"));

                Assert.Fail();
            }
            catch (AssertFailedException) { }

            try
            {
                AssertExpr(Expr.Add(
                    Expr.Constant(1.0),
                    Expr.Constant(2.0)),
                    Parser.Parse("2+1"));

                Assert.Fail();
            }
            catch (AssertFailedException) { }
        }

        public static void AssertExpr(Expr expected, Expr actual)
        {
            if (expected.NodeType != actual.NodeType)
                Assert.Fail();
            if (expected is ConstantExpr)
            {
                var expC = expected as ConstantExpr;
                var actC = actual as ConstantExpr;
                Assert.AreEqual(expC.Value, actC.Value);
            }
            else if (expected is UnaryExpr)
            {
                var expU = expected as UnaryExpr;
                var actU = actual as UnaryExpr;
                AssertExpr(expU.Content, actU.Content);
            }
            else if (expected is BinaryExpr)
            {
                var expB = expected as BinaryExpr;
                var actB = actual as BinaryExpr;
                AssertExpr(expB.Left, actB.Left);
                AssertExpr(expB.Right, actB.Right);
            }
        }
    }
}