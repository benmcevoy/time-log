namespace TimeLog.Lexer.Matcher
{
    public class TextMatcher : IMatcher<string>
    {
        public Token<string> Match(int lineNumber, int startPosition, string value)
        {
            var comment = value.Trim();

            return new Token<string>(TokenType.Text, lineNumber, startPosition, startPosition + comment.Length, comment);
        }
    }
}
