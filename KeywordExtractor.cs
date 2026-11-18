using TimeLog.Model;

namespace TimeLog
{
    public class KeywordExtractor
    {
        public IEnumerable<string> Extract(Log? log)
        {
            if (log is null) return [];
            return log.Days.SelectMany(d => d.TimeEntries.Select(t => t.ProjectName)).Distinct();
        }
    }
}
