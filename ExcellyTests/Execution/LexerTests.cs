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
    public class LexerTests
    {
        [TestCategory("Lexer")]
        [TestMethod()]
        public void ParseNumberTest()
        {
            CollectionAssert.AreEqual(new Token[] { new Token("0", TokenType.Number) },
                Lexer.Parse("0").ToArray());
            CollectionAssert.AreEqual(new Token[] { new Token("0.5", TokenType.Number) },
                Lexer.Parse("0.5").ToArray());
            CollectionAssert.AreEqual(new Token[] { new Token("1.25", TokenType.Number) },
                Lexer.Parse("1.25").ToArray());
        }

        [TestCategory("Lexer")]
        [TestMethod()]
        public void ParseArithTest()
        {
            CollectionAssert.AreEqual(new Token[] {
                    new Token("1", TokenType.Number),
                    new Token("+", TokenType.Operator),
                    new Token("2", TokenType.Number)
                },
                Lexer.Parse("1 + 2").ToArray());
        }
    }
}