using System;
using System.Text.RegularExpressions;

namespace TimeLog.Lexer.Matcher
{
    public class DateMatcher : IMatcher<DateTime>
    {
        public Token<DateTime> Match(int lineNumber, int startPosition, string value)
        {
            DateTime date;

            return IsDateLineFirstLine(value, out date)
                ? new Token<DateTime>(TokenType.Date, lineNumber, startPosition, value.Length, date)
                : null;
        }

        private bool IsDateLineFirstLine(string line, out DateTime result)
        {
            result = new DateTime();

            var l = line.Trim();

            var match = _dateLine.Match(l);

            if (!match.Success) return false;

            return DateTime.TryParse(l, out result);
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