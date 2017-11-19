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
        private char Pop() => Code[_index++];

        private static bool IsWhiteSpace(char c)
            => c == ' ' || c == '\n' || c == '\r' || c == '\t';

        private static bool IsOperator(char c)
            => c == '+' || c == '-' || c == '*' || c == '/' || c == '^';

        private static bool IsParen(char c)
            => c == '(' || c == ')';

        public static IEnumerable<Token> Parse(string code)
            => new Lexer(code).Parse();

        public IEnumerable<Token> Parse()
        {
            while (!IsEof)
                yield return GetNextToken();
        }

        internal Token GetNextToken()
        {
            while (IsWhiteSpace(Current)) Pop();
            if (IsOperator(Current)) return GetOperatorToken();
            if (char.IsDigit(Current)) return GetNumberToken();
            if (IsParen(Current)) return GetParenToken();
            if (char.IsLetter(Current)) return GetParameter();
            if (Current == ',') return new Token(Pop().ToString(), TokenType.Comma);
            if (Current == ':') return new Token(Pop().ToString(), TokenType.Colon);

            throw new Exception("Must not happened");
        }

        private Token GetOperatorToken()
        {
            return new Token(Pop().ToString(), TokenType.Operator);
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
                else if (!char.IsDigit(Current)) break;

                Pop();
            }

            return new Token(Code.Substring(startIndex, _index - startIndex), TokenType.Number);
        }

        private Token GetParenToken()
        {
            var tok = Pop().ToString();
            return new Token(tok, tok == "(" ? TokenType.LParen : TokenType.RParen);
        }

        private Token GetParameter()
        {
            var startIndex = _index;

            while (!IsEof)
            {
                if (char.IsLetterOrDigit(Current) || Current == '_') Pop();
                else break;
            }

            return new Token(Code.Substring(startIndex, _index - startIndex), TokenType.Name);
        }
    }
}
