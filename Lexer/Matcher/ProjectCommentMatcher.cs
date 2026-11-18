using System;
using System.Linq;

namespace TimeLog.Lexer.Matcher
{
    public class ProjectCommentMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            if (startPosition == 0) return Token.Unknown;
            if (string.IsNullOrWhiteSpace(value)) return Token.Unknown;

            // rest of line if no full stop
            if (!value.Contains(".")) return new Token(TokenType.ProjectComment, lineNumber, startPosition, value.Length, value); 

            // otherwise first sentance
            var comment = value.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(comment)) return Token.Unknown;

            return new Token(TokenType.ProjectComment, lineNumber, startPosition, comment.Length, comment.Trim());
        }
    }
}
