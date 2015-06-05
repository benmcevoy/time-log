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
            Day currentDay = null;
            TimeEntry currentTime = null;

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
                            currentDay = new Day(lineNumber, (DateTime)token.Value);
                            log.Days.Add(currentDay);
                            break;

                        case TokenType.TimePeriod:
                            currentTime = new TimeEntry((Period) token.Value);
                            if (currentDay != null) currentDay.TimeEntries.Add(currentTime);
                            break;

                        case TokenType.ProjectName:
                            if (currentTime != null) currentTime.ProjectName = Convert.ToString(token.Value);
                            break;

                        case TokenType.ProjectComment:
                            if (currentTime != null) currentTime.AddComment(Convert.ToString(token.Value));
                            break;
                    }
                }
            }

            return log;
        }
    }
}
