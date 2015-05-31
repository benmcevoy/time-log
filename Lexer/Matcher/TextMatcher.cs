namespace TimeLog.Lexer.Matcher
{
    public class TextMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            var comment = value.Trim();

            return new Token(TokenType.Text, lineNumber, startPosition, startPosition + comment.Length, comment);
        }
    }
}
