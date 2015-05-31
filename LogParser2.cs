using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TimeLog
{
    public class LogParser2 : IParser
    {
        public static string LogDateFormat = "dddd dd MMMM yyyy";
        public static string TheIdealLine = "---------------------------------------------------------------------";

        private readonly Regex _dateLine = new Regex(
@"^(Monday|Tuesday|Wednesday|Thursday|Friday|Saturday|Sunday)
\s
(0?[1-9]|[12][0-9]|3[01])
\s
(January|February|March|April|May|June|July|August|September|October|November|December)
\s
(20[0-1][0-9])$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private readonly Regex _timePeriod = new Regex(
@"^([012]?[0-9]:[0-5][0-9])
-
([012]?[0-9]:[0-5][0-9])
\s
.*$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        private readonly Regex _project = new Regex(
@"^[012]?[0-9]:[0-5][0-9]
-
[012]?[0-9]:[0-5][0-9]
[\s|\t]*
(.*?)\.(.*)$", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

        public Log Parse(IEnumerable<string> lines)
        {
            if (!lines.Any())
                return null;

            var log = new Log();

            var lineNumber = -1;
            Day day = null;
            TimeEntry timeEntry = null;

            foreach (var line in lines)
            {
                lineNumber++;

                if (string.IsNullOrEmpty(line))
                    continue;

                Debug.WriteLine(string.Format("Parsing Line {0}: {1}", lineNumber, line));

                DateTime date;
                string secondDateLine; 
                Period timePeriod;
                string project;
                string comment;

                if (IsDateLineFirstLine(line, out date))
                {
                    if (day != null)
                    {
                        if (timeEntry != null)
                        {
                            day.TimeEntries.Add(timeEntry);
                            timeEntry = null;
                        }
                        log.Days.Add(day);
                    }

                    day = new Day() { Date = date, LineNumber = lineNumber };
                    continue;
                }
               
                if (IsDateLineSecondLine(line, out secondDateLine))
                {
                    continue;
                }

                if (IsTimePeriod(line, day == null ? DateTime.Today : day.Date, out timePeriod))
                {
                    if (timeEntry != null)
                        day.TimeEntries.Add(timeEntry);

                    timeEntry = new TimeEntry(timePeriod);
                    Debug.WriteLine("TimeEntry.Duration: " + timeEntry.Duration);
                }

                if (IsProject(line, out project))
                {
                    timeEntry.ProjectName = project;
                }

                if (IsComments(line, out comment))
                {
                    if (timeEntry != null)
                    {
                        timeEntry.AddComment(comment);
                    }
                    continue;
                }
            }

            if (day != null)
            {
                if (timeEntry != null)
                {
                    day.TimeEntries.Add(timeEntry);
                }
                log.Days.Add(day);
            }

            return log;
        }

        private bool IsDateLineFirstLine(string line, out DateTime result)
        {
            result = new DateTime();

            var l = line.Trim();

            var match = _dateLine.Match(l);

            if (match.Success)
            {
                if (DateTime.TryParse(l, out result))
                {
                    return true;
                }
            }
             
            return false;
        }

        private bool IsDateLineSecondLine(string line, out string  result)
        {
            result = TheIdealLine;
            return (line.All(l => l == '-') && line.Length > 5);
        }

        public bool IsTimePeriod(string line, DateTime date, out Period result)
        {
            var l = line.Trim();
            result = null;

            var match = _timePeriod.Match(l);

            if (match.Success)
            {
                DateTime startTime;
                if (DateTime.TryParse(match.Groups[1].Value, out startTime))
                {
                    DateTime endTime;
                    if (DateTime.TryParse(match.Groups[2].Value, out endTime))
                    {
                        if (endTime < startTime)
                            endTime = endTime.AddHours(12);

                        startTime = date.Date + startTime.TimeOfDay;
                        endTime = date.Date + endTime.TimeOfDay;

                        result = new Period() { StartTime = startTime, EndTime = endTime };
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsProject(string line, out string result)
        {
            result = string.Empty;
            var l = line.Trim();
            var match = _project.Match(l);

            if (match.Success)
            {
                result = match.Groups[1].Value;
                return true;
            }

            return false;
        }

        public bool IsComments(string line, out string result)
        {
            result = string.Empty;
            var temp = string.Empty;
            // check for trailing comments after a project name
            if (IsProject(line, out temp))
            {
                var l = line.Trim();
                var match = _project.Match(l);

                if (match.Success)
                {
                    result = match.Groups[2].Value.Trim();
                    return true;
                }

                return false;
            }

            result = line.Trim();

            return string.IsNullOrEmpty(result);
        }

    }
}
