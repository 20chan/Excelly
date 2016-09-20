using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Excelly
{
    public class Calculator
    {
        public Token[] ParseCode(string code)
        {
            TokenType stage = TokenType.NONE;
            int marker = 0;
            List<Token> result = new List<Token>();
            StringBuilder sb = new StringBuilder();

            for(int it = 0; it < code.Length; it++)
            {
                switch(stage)
                {
                    case TokenType.NONE:
                        {
                            switch(code[it])
                            {
                                case ' ':
                                    break;
                                case '0':
                                case '1':
                                case '2':
                                case '3':
                                case '4':
                                case '5':
                                case '6':
                                case '7':
                                case '8':
                                case '9':
                                case '.':
                                    {
                                        stage = TokenType.NUMBER;
                                        marker = it;

                                        if (it == code.Length - 1)
                                        {
                                            stage = TokenType.NONE;
                                            result.Add(new Token(TokenType.NUMBER, code[it].ToString()));
                                        }
                                        break;
                                    }
                                case '+':
                                    result.Add(new Token(TokenType.PLUS, "+"));
                                    break;
                                case '-':
                                    result.Add(new Token(TokenType.MINUS, "-"));
                                    break;
                                case '*':
                                    result.Add(new Token(TokenType.ASTERISK, "*"));
                                    break;
                                case '/':
                                    result.Add(new Token(TokenType.DIVIDE, "/"));
                                    break;
                                case '%':
                                    result.Add(new Token(TokenType.PERCENT, "%"));
                                    break;
                                case '(':
                                    result.Add(new Token(TokenType.LPAREN, "("));
                                    break;
                                case ')':
                                    result.Add(new Token(TokenType.RPAREN, ")"));
                                    break;
                                default:
                                    stage = TokenType.FUNCTION;
                                    marker = it;
                                    break;
                            }
                            break;
                        }
                    case TokenType.NUMBER:
                        {
                            if(Token.IsSplitChar(code[it]))
                            {
                                stage = TokenType.NONE;
                                result.Add(new Token(TokenType.NUMBER, subString(code, marker, it--)));
                            }
                            else if(!char.IsDigit(code[it]) && code[it] != '.')
                            {
                                stage = TokenType.NONE;
                                result.Add(new Token(TokenType.ERROR, subString(code, marker, it--)));
                            }
                            else if(it == code.Length - 1)
                            {
                                stage = TokenType.NONE;
                                result.Add(new Token(TokenType.NUMBER, subString(code, marker, it + 1)));
                            }
                            break;
                        }
                    case TokenType.FUNCTION:
                        {
                            if(Token.IsSplitChar(code[it]))
                            {
                                stage = TokenType.NONE;
                                string cur = subString(code, marker, it--);
                                result.Add(new Token(TokenType.FUNCTION, cur));
                            }
                            break;
                        }
                }
            }

            return result.ToArray();
        }

        private string subString(string str, int mark, int cur)
        {
            return str.Substring(mark, cur - mark);
        }
    }

    public struct Token
    {
        public static bool IsSplitChar(char c)
        {
            switch(c)
            {
                case ' ':
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '(':
                case ')':
                case ':': //아직 안함ㅋ
                    return true;
            }
            return false;
        }

        public TokenType Type;
        public string Value;

        public Token(TokenType type, string val)
        {
            Type = type;
            Value = val;
        }
    }

    public enum TokenType
    {
        NONE,
        NUMBER,
        CELL,
        FUNCTION,
        PLUS,
        MINUS,
        ASTERISK,
        PERCENT,
        DIVIDE,
        LPAREN,
        RPAREN,
        ERROR
    }
}
