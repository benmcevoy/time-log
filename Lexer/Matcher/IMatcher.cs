namespace TimeLog.Lexer.Matcher
{
    public interface IMatcher
    {
        Token Match(int lineNumber, int startPosition, string value);
    }
}