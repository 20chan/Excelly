using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Excelly.Execution
{
    public class Parser
    {
        private Stack<Token> _tokens;
        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = new Stack<Token>(tokens);
        }

        public Parser(string code) : this(new Lexer(code).Parse())
        {

        }

        public Token Top => _tokens.Peek();
        public Token Pop() => _tokens.Pop();

        public Expression Parse()
        {
            return ParseArith();
        }

        private Expression ParseArith()
        {
            var lexpr = ParseTerm();
            if (Top.Code == "+" || Top.Code == "-")
            {
                var op = Pop();
                var rexpr = ParseArith();
                if (op.Code == "+")
                    return Expression.Add(lexpr, rexpr);
                else
                    return Expression.Subtract(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expression ParseTerm()
        {
            var lexpr = ParseAtom();
            if (Top.Code == "*" || Top.Code == "/")
            {
                var op = Pop();
                var rexpr = ParseFactor();
                if (op.Code == "*")
                    return Expression.Multiply(lexpr, rexpr);
                else
                    return Expression.Divide(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expression ParseFactor()
        {
            if (Top.Code == "+")
                return Expression.UnaryPlus(ParsePower());
            else if (Top.Code == "-")
                return Expression.Negate(ParsePower());
            else
                return ParsePower();
        }

        private Expression ParsePower()
        {
            var lexpr = ParseAtom();
            if (Top.Code == "^")
            {
                Pop();
                var rexpr = ParsePower();
                return Expression.Power(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expression ParseAtom()
        {
            if (Top.Code == "(")
            {
                Pop();
                var expr = Parse();
                if (Top.Code != ")")
                    throw new Exception("Paren not match");
                return expr;
            }
            else return Expression.Constant(Top);
        }
    }
}
