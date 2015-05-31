using System.Collections.Generic;
using System.Linq;

namespace TimeLog
{
    public class KeywordExtractor
    {
        public IEnumerable<string> Extract(Log log)
        {
            return log.Days.SelectMany(d => d.TimeEntries.Select(t => t.ProjectName)).Distinct();
        }
    }
}
