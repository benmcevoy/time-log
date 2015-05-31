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
.*$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public Token Match(int lineNumber, int startPosition, string value)
        {
            return IsTimePeriod(value, out _timePeriod)
                ? new Token(TokenType.TimePeriod, lineNumber, startPosition, 100, _timePeriod) 
                : null;
        }

        private bool IsTimePeriod(string line, out Period result)
        {
            var l = line.Trim();
            result = null;

            var match = _timePeriodRegex.Match(l);

            if (!match.Success) return false;

            DateTime startTime;
            if (!DateTime.TryParse(match.Groups[1].Value, out startTime)) return false;

            DateTime endTime;
            if (!DateTime.TryParse(match.Groups[2].Value, out endTime)) return false;

            if (endTime < startTime) endTime = endTime.AddHours(12);

            startTime = DateTime.MinValue + startTime.TimeOfDay;
            endTime = DateTime.MinValue + endTime.TimeOfDay;

            result = new Period (startTime, endTime );
            return true;
        }
    }
}