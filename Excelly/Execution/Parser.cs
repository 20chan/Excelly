using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Excelly.Execution
{
    public class Parser
    {
        private Queue<Token> _tokens;
        public Parser(IEnumerable<Token> tokens)
        {
            _tokens = new Queue<Token>(tokens);
        }

        public Parser(string code) : this(new Lexer(code).Parse())
        {

        }

        public bool IsEmpty => _tokens.Count == 0;
        public Token Top => _tokens.Peek();
        public Token Pop() => _tokens.Dequeue();

        public static Expression Parse(string code)
            => new Parser(code).Parse();

        public Expression Parse()
        {
            return ParseArith();
        }

        private Expression ParseArith()
        {
            var lexpr = ParseTerm();
            if (!IsEmpty && (Top.Code == "+" || Top.Code == "-"))
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
            var lexpr = ParseFactor();
            if (!IsEmpty && (Top.Code == "*" || Top.Code == "/"))
            {
                var op = Pop();
                var rexpr = ParseTerm();
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
            {
                Pop();
                return Expression.UnaryPlus(ParseFactor());
            }
            else if (Top.Code == "-")
            {
                Pop();
                return Expression.Negate(ParseFactor());
            }
            else
                return ParsePower();
        }

        private Expression ParsePower()
        {
            var lexpr = ParseAtom();
            if (!IsEmpty && Top.Code == "^")
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
                if (IsEmpty || Top.Code != ")")
                    throw new Exception("Paren not match");
                Pop();
                return expr;
            }
            else
            {
                if (Top.Type == TokenType.Number)
                    return Expression.Constant(Convert.ToDouble(Pop().Code));
                else
                    throw new Exception("Not number");
            }
        }
    }
}
