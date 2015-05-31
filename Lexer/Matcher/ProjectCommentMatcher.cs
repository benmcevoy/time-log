using System;
using System.Linq;

namespace TimeLog.Lexer.Matcher
{
    public class ProjectCommentMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            if (startPosition == 0) return null;
            if (string.IsNullOrWhiteSpace(value)) return null;

            // rest of line if no full stop
            if (!value.Contains(".")) return new Token(TokenType.ProjectComment, lineNumber, startPosition, value.Length, value); 

            // otherwise first sentance
            var comment = value.Trim().Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(comment)) return null;

            return new Token(TokenType.ProjectComment, lineNumber, startPosition, comment.Length, comment);
        }
    }
}
