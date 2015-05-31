using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeLog.Model
{
    public class Day
    {
        public Day()
        {
            TimeEntries = new List<TimeEntry>();
        }

        public int LineNumber { get; set; }

        public DateTime Date { get; set; }

        public List<TimeEntry> TimeEntries { get; set; }

        public double HoursToday { get { return TimeEntries.Select(t => t.Minutes).Sum() / 60.0; } }
    }
}
