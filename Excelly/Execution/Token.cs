namespace Excelly.Execution
{
    public struct Token
    {
        public string Code;
        public TokenType Type;

        public Token(string code, TokenType type)
        {
            Code = code;
            Type = type;
        }
    }

    public enum TokenType
    {
        EOF = -1,
        None = 0,
        Operator,
        Number,
        Paren,
    }
}
