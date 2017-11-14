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

        [TestCategory("Lexer")]
        [TestMethod()]
        public void ParseNameTest()
        {
            CollectionAssert.AreEqual(new Token[] { new Token("abcdef", TokenType.Name) },
                Lexer.Parse("abcdef").ToArray());

            CollectionAssert.AreEqual(new Token[] { new Token("ABC_123_deF", TokenType.Name) },
                Lexer.Parse("ABC_123_deF").ToArray());

            CollectionAssert.AreEqual(new Token[] {
                new Token("1", TokenType.Number),
                new Token("+", TokenType.Operator),
                new Token("A2", TokenType.Name),
            },
                Lexer.Parse("1 + A2").ToArray());

            CollectionAssert.AreEqual(new Token[] {
                new Token("a_1", TokenType.Name),
                new Token("-", TokenType.Operator),
                new Token("(", TokenType.Paren),
                new Token("-", TokenType.Operator),
                new Token("b_2", TokenType.Name),
                new Token(")", TokenType.Paren)
            },
                Lexer.Parse("a_1-(-b_2)").ToArray());
        }
    }
}