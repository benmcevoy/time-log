namespace TimeLog.Lexer
{
    public class Token<T>
    {
        public Token(TokenType tokenType, int lineNumber,  int start, int length, T value)
        {
            TokenType = tokenType;
            LineNumber = lineNumber;
            Start = start;
            Length = length;
            Value = value;
        }

        public TokenType TokenType { get; private set; }
        public int LineNumber { get; private set; }
        public int Start { get; private set; }
        public int Length { get; private set; }
        public T Value { get; private set; }
    }
}