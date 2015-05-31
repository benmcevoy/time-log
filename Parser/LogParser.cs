using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TimeLog.Model;

namespace TimeLog.Parser
{
    public class LogParser : IParser
    {

        public const string TheIdealLine = "---------------------------------------------------------------------";
        public static string LogDateFormat = "dddd dd MMMM yyyy";

        
        // TODO: move to TextLexer?  this is the parser, but lexer needs to go first....
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

                // TODO: call the line lineLexer?
               // for each apply the lexers in order and move along the line
                
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
