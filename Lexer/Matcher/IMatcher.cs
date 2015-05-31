namespace TimeLog.Lexer.Matcher
{
    public interface IMatcher<T>
    {
        Token<T> Match(int lineNumber, int startPosition, string value);
    }
}