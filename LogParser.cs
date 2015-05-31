using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TimeLog
{
    public class LogParser : IParser
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

                Debug.WriteLine("Parsing Line {0}: {1}", lineNumber, line);

                var date = MatchDateLineFirstLine(line);

                if (date != null)
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

                    day = new Day { Date = date.Value, LineNumber = lineNumber };
                    continue;
                }

                var secondDateLine = MatchDateLineSecondLine(line);
                if (!string.IsNullOrEmpty(secondDateLine))
                {
                    // could do something with it? nah...
                    continue;
                }

                var timePeriod = MatchTimePeriod(line, day == null ? DateTime.Today : day.Date);
                if (timePeriod != null)
                {
                    if (timeEntry != null)
                        day.TimeEntries.Add(timeEntry);

                    timeEntry = new TimeEntry() { StartDateTime = timePeriod.StartTime, EndDateTime = timePeriod.EndTime };
                    Debug.WriteLine("TimeEntry.Duration: " + timeEntry.Duration);
                }

                var project = MatchProject(line);
                if (!string.IsNullOrEmpty(project))
                {
                    timeEntry.ProjectName = project;
                }

                var comments = MatchComments(line);
                if (!string.IsNullOrEmpty(comments))
                {
                    // TODO: store comments as enumerable
                    // don't have duplicates per entry 
                    // add an AddCommnet method

                    if (timeEntry != null)
                    {
                        timeEntry.AddComment(comments);
                    }
                    continue;
                }
            }

            if (day == null) return log;

            if (timeEntry != null)
            {
                day.TimeEntries.Add(timeEntry);
            }
            log.Days.Add(day);

            return log;
        }

        private DateTime? MatchDateLineFirstLine(string line)
        {
            var l = line.Trim();
            DateTime result;

            var match = _dateLine.Match(l);

            if (!match.Success) return null;

            if (DateTime.TryParse(l, out result))
            {
                return result;
            }

            return null;
        }

        private string MatchDateLineSecondLine(string line)
        {
            if (line.All(l => l == '-') && line.Length > 5)
            {
                // return the ideal line!
                return TheIdealLine;
            }

            return string.Empty;
        }

        public Period MatchTimePeriod(string line, DateTime date)
        {
            var l = line.Trim();
            Period period = null;

            var match = _timePeriod.Match(l);

            if (!match.Success) return period;

            DateTime startTime;
            if (!DateTime.TryParse(match.Groups[1].Value, out startTime)) return period;

            DateTime endTime;
            if (!DateTime.TryParse(match.Groups[2].Value, out endTime)) return period;

            if (endTime < startTime) endTime = endTime.AddHours(12);

            startTime = date.Date + startTime.TimeOfDay;
            endTime = date.Date + endTime.TimeOfDay;

            period = new Period { StartTime = startTime, EndTime = endTime };

            return period;
        }

        public string MatchProject(string line)
        {
            var l = line.Trim();
            var match = _project.Match(l);

            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        public string MatchComments(string line)
        {
            //if (MatchDateLineFirstLine(line) != null)
            //    return string.Empty;

            //if (!string.IsNullOrEmpty(MatchDateLineSecondLine(line)))
            //    return string.Empty;

            if (string.IsNullOrEmpty(MatchProject(line))) return line.Trim();

            var l = line.Trim();
            var match = _project.Match(l);

            return match.Success ? match.Groups[2].Value.Trim() : string.Empty;
        }
    }
}
