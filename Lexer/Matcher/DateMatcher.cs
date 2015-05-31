using System;
using System.Text.RegularExpressions;

namespace TimeLog.Lexer.Matcher
{
    public class DateMatcher : IMatcher
    {
        public Token Match(int lineNumber, int startPosition, string value)
        {
            DateTime result;

            var l = value.Trim();
            var match = _dateLine.Match(l);

            if (!match.Success) return null;
            if (!DateTime.TryParse(l, out result)) return null;
            
            return new Token(TokenType.Date, lineNumber, startPosition, value.Length, result);
        }

        private readonly Regex _dateLine = new Regex(
            @"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)
\s
(0?[1-9]|[12][0-9]|3[01])
\s
(January|February|March|April|May|June|July|August|September|October|November|December)
\s
(20[0-1][0-9])$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

    }
}