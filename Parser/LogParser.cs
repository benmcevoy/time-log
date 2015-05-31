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

                if (string.IsNullOrEmpty(line)) continue;

                Debug.WriteLine("Parsing Line {0}: {1}", lineNumber, line);

                var tokens = _lexer.Process(lineNumber, line);

                foreach (var token in tokens)
                {
                    switch (token.TokenType)
                    {
                        case TokenType.Text:
                            break;
                        case TokenType.Date:
                            day = new Day(lineNumber, (DateTime)token.Value);
                            break;
                        case TokenType.Line:
                            break;

                        case TokenType.TimePeriod:

                            break;
                        case TokenType.ProjectName:

                            break;
                        case TokenType.EndOfLine:

                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
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
