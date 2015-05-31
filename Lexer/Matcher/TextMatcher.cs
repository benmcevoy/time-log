namespace TimeLog.Lexer.Matcher
{
    public class TextMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            var result = value.Trim();

            return new Token(TokenType.Text, lineNumber, startPosition, result.Length, result);
        }
    }
}
