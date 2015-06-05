using System;
using System.Text.RegularExpressions;
using TimeLog.Model;

namespace TimeLog.Lexer.Matcher
{
    public class TimePeriodMatcher : IMatcher
    {
        Period _timePeriod;
        private readonly Regex _timePeriodRegex = new Regex(
            @"^([012]?[0-9]:[0-5][0-9])
-
([012]?[0-9]:[0-5][0-9])
\s
.*
$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public Token Match(int lineNumber, int startPosition, string value)
        {
            var l = value;
            var match = _timePeriodRegex.Match(l);

            if (!match.Success) return null;

            DateTime startTime;
            var startMatch = match.Groups[1].Value;

            if (!DateTime.TryParse(startMatch, out startTime)) return null;

            DateTime endTime;
            var endMatch = match.Groups[2].Value;

            if (!DateTime.TryParse(endMatch, out endTime)) return null;

            if (endTime < startTime) endTime = endTime.AddHours(12);

            startTime = DateTime.MinValue + startTime.TimeOfDay;
            endTime = DateTime.MinValue + endTime.TimeOfDay;

            return new Token(TokenType.TimePeriod, lineNumber, startPosition, startMatch.Length + endMatch.Length + 1, new Period(startTime, endTime));
        }
    }
}