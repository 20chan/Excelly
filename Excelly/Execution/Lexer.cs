using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelly.Execution
{
    public class Lexer
    {
        public string Code { get; }
        private int _index = 0;

        public Lexer(string code)
        {
            Code = code;
        }

        private char Current => Code[_index];
        private bool IsEof => _index == Code.Length;

        private static bool IsWhiteSpace(char c)
            => c == ' ' || c == '\n' || c == '\r' || c == '\t';

        private static bool IsOperator(char c)
            => c == '+' || c == '-' || c == '*' || c == '/' || c == '^';

        private static bool IsParen(char c)
            => c == '(' || c == ')';

        public IEnumerable<Token> Parse()
        {
            while (!IsEof)
                yield return GetNextToken();
        }

        internal Token GetNextToken()
        {
            while (IsWhiteSpace(Current)) _index++;
            if (IsOperator(Current)) return GetOperatorToken();
            if (char.IsDigit(Current)) return GetNumberToken();
            if (IsParen(Current)) return GetParenToken();

            throw new Exception("Must not happened");
        }

        private Token GetOperatorToken()
        {
            return new Token(Code[_index++].ToString(), TokenType.Operator);
        }

        private Token GetNumberToken()
        {
            var startIndex = _index;
            var dotIncluded = false;

            while (!IsEof)
            {
                if (Current == '.')
                {
                    if (!dotIncluded)
                        dotIncluded = true;
                    else
                        throw new Exception("Number has two dot");
                }
                if (!char.IsDigit(Current)) break;

                _index++;
            }

            return new Token(Code.Substring(startIndex, _index - startIndex), TokenType.Number);
        }

        private Token GetParenToken()
        {
            return new Token(Code[_index++].ToString(), TokenType.Paren);
        }
    }
}
