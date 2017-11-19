using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

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

        public static Expr Parse(string code)
            => new Parser(code).Parse();

        public Expr Parse()
        {
            return ParseArith();
        }

        private Expr ParseArith()
        {
            var lexpr = ParseTerm();
            if (!IsEmpty && (Top.Code == "+" || Top.Code == "-"))
            {
                var op = Pop();
                var rexpr = ParseArith();
                if (op.Code == "+")
                    return Expr.Add(lexpr, rexpr);
                else
                    return Expr.Subtract(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expr ParseTerm()
        {
            var lexpr = ParseFactor();
            if (!IsEmpty && (Top.Code == "*" || Top.Code == "/"))
            {
                var op = Pop();
                var rexpr = ParseTerm();
                if (op.Code == "*")
                    return Expr.Multiply(lexpr, rexpr);
                else
                    return Expr.Divide(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expr ParseFactor()
        {
            if (Top.Code == "+")
            {
                Pop();
                return Expr.UnaryPlus(ParseFactor());
            }
            else if (Top.Code == "-")
            {
                Pop();
                return Expr.Negate(ParseFactor());
            }
            else
                return ParsePower();
        }

        private Expr ParsePower()
        {
            var lexpr = ParseAtom();
            if (!IsEmpty && Top.Code == "^")
            {
                Pop();
                var rexpr = ParsePower();
                return Expr.Power(lexpr, rexpr);
            }
            else return lexpr;
        }

        private Expr ParseAtom()
        {
            if (Top.Type == TokenType.LParen)
            {
                Pop();
                var expr = Parse();
                if (IsEmpty || Top.Type != TokenType.RParen)
                    throw new Exception("Paren not match");
                Pop();
                return expr;
            }
            else
            {
                if (Top.Type == TokenType.Number)
                    return Expr.Constant(Convert.ToDouble(Pop().Code));
                else if (Top.Type == TokenType.Name)
                {
                    var name = Pop().Code;
                    if (Top.Type == TokenType.LParen)
                    {
                        Pop(); // (
                        var parameters = ParseParameters();
                        Pop(); // )
                        return Expr.Function(name, parameters);
                    }
                    else
                        return Expr.ParameterExpr(name);
                }
                else throw new Exception("Unexpected Type");
            }
        }

        private Expr[] ParseParameters()
        {
            if (Top.Code == ")") return new Expr[] { };

            var buffer = new List<Expr>();
            buffer.Add(Parse());

            while (Top.Code != ")")
            {
                if (Pop().Type == TokenType.Comma)
                buffer.Add(Parse());
            }

            return buffer.ToArray();
        }
    }
}
