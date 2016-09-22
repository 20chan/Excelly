using System.Collections.Generic;
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
                                case '[':
                                    result.Add(new Token(TokenType.LSBRACKET, "["));
                                    break;
                                case ']':
                                    result.Add(new Token(TokenType.RSBRACKET, "]"));
                                    break;
                                case ',':
                                    result.Add(new Token(TokenType.COMMA, ","));
                                    break;
                                default:
                                    if (it != code.Length - 1)
                                        if (char.IsLetter(code[it]))
                                            if (char.IsDigit(code[it + 1]) || 
                                                (Token.IsSplitChar(code[it + 1]) && code[it + 1] != '(' && code[it + 1] != ')'))
                                            {
                                                stage = TokenType.CELL;
                                                marker = it;
                                                break;
                                            }

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
                    case TokenType.CELL:
                        {
                            if(Token.IsSplitChar(code[it]))
                            {
                                stage = TokenType.NONE;
                                result.Add(new Token(TokenType.CELL, subString(code, marker, it--)));
                            }

                            break;
                        }
                }
            }

            return result.ToArray();
        }

        private Stack<Token> ToPostFix(Token[] toks)
        {
            Stack<Token> output = new Stack<Token>();
            Stack<Token> op = new Stack<Token>();
            for(int i = 0; i < toks.Length; i++)
            {
                switch (toks[i].Type)
                {
                    case TokenType.PLUS:
                    case TokenType.MINUS:
                        op.Push(toks[i]);
                        break;
                    case TokenType.ASTERISK:
                    case TokenType.DIVIDE:
                        Token top = op.Peek();
                        if(top.Type == TokenType.ASTERISK || top.Type == TokenType.DIVIDE || top.Type == TokenType.PERCENT)
                        {

                        }
                        else if(top.Type == TokenType.PLUS || top.Type == TokenType.MINUS)
                        {

                        }

                        output.Push(op.Pop());
                        op.Push(toks[i]);
                        break;
                    case TokenType.LPAREN:
                        op.Push(toks[i]);
                        break;
                    case TokenType.FUNCTION:
                        int mark = i; //remember function token
                        if (toks[++i].Type != TokenType.LPAREN)
                            throw new System.Exception();

                        List<Token> args = new List<Token>();
                        while (toks[i].Type != TokenType.RPAREN)
                        {
                            while(toks[i].Type != TokenType.COMMA)
                            {
                                List<Token> arg = new List<Token>();
                                arg.Add(toks[i++]);
                                args.Add(Evaluate(ToPostFix(arg.ToArray())));
                            }
                        }

                        foreach (Token t in args)
                            output.Push(t);
                        op.Push(toks[mark]);
                        break;
                }
            }

        }

        private Token Evaluate(Stack<Token> postToks)
        {

            Token tok;
            while(postToks.Count > 0)
            {
                tok = postToks.Pop();
                switch (tok.Type)
                {
                    case TokenType.PLUS:
                        break;
                }
            }

            throw new System.NotImplementedException();
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
                case ',':
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
        LSBRACKET,
        RSBRACKET,
        COMMA,
        ERROR
    }
}
