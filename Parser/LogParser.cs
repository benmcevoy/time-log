using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TimeLog.Lexer;
using TimeLog.Model;

namespace TimeLog.Parser
{
    public class LogParser : IParser
    {
        private readonly LineLexer _lexer;

        public const string TheIdealLine = "---------------------------------------------------------------------";
        public static string LogDateFormat = "dddd dd MMMM yyyy";

        public LogParser(LineLexer lexer)
        {
            _lexer = lexer;
        }

        public Log Parse(IEnumerable<string> lines)
        {
            var lineList = lines as string[] ?? lines.ToArray();

            if (!lineList.Any()) return null;

            var log = new Log();
            var lineNumber = -1;
            Day day = null;
            TimeEntry timeEntry = null;

            foreach (var line in lineList)
            {
                lineNumber++;

                if (string.IsNullOrWhiteSpace(line)) continue;

                Debug.WriteLine("Parsing Line {0}: {1}", lineNumber, line);

                var tokens = _lexer.Process(lineNumber, line);

                foreach (var token in tokens)
                {
                    switch (token.TokenType)
                    {
                        case TokenType.Date:
                            day = new Day(lineNumber, (DateTime)token.Value);
                            log.Days.Add(day);
                            timeEntry = null;
                            break;

                        case TokenType.TimePeriod:
                            if (timeEntry != null) day.TimeEntries.Add(timeEntry);

                            timeEntry = new TimeEntry((Period)token.Value);
                            day.TimeEntries.Add(timeEntry);
                            break;

                        case TokenType.ProjectName:
                            if (timeEntry != null) timeEntry.ProjectName = (string)token.Value;
                            break;

                        case TokenType.ProjectComment:
                            if (timeEntry != null) timeEntry.AddComment((string)token.Value);
                            break;
                    }
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
    }
}
