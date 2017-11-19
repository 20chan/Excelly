using Microsoft.VisualStudio.TestTools.UnitTesting;
using Excelly.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excelly.Execution.Tests
{
    [TestClass()]
    public class EvaluateTests
    {
        [TestCategory("Evaluate")]
        [TestMethod()]
        public void ExecuteTest()
        {
            Assert.AreEqual(32.5,
                Parser.Parse("32.5").Eval());

            Assert.AreEqual(3.0,
                Parser.Parse("1+2").Eval());

            Assert.AreEqual(1024.0,
                Parser.Parse("(1+0.5*2)^10").Eval());
        }

        [Registered("Sum")]
        static double Sum(object[] parameters) => parameters.Cast<double>().Sum();

        [TestCategory("Evaluate")]
        [TestMethod()]
        public void ExecuteFunctionTest()
        {
            Registered.RegisterAll(typeof(EvaluateTests));
            Assert.AreEqual(3.0,
                Parser.Parse("Sum(1, 2)").Eval());
        }

        [TestCategory("Evaluate")]
        [TestMethod()]
        public void ExecuteErrTest()
        {
            Assert.AreNotEqual(2.0,
                Parser.Parse("1").Eval());
        }
    }
}