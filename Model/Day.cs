using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeLog.Model
{
    public class Day
    {
        public Day(int lineNumber, DateTime date)
        {
            LineNumber = lineNumber;
            Date = date;
            TimeEntries = new List<TimeEntry>();
        }

        public void AddTimeEntry(TimeEntry timeEntry)
        {
            TimeEntries.Add(timeEntry);
        }

        public int LineNumber { get; private set; }

        public DateTime Date { get; private set; }

        public List<TimeEntry> TimeEntries { get; private set; }

        public double HoursToday { get { return TimeEntries.Select(t => t.Minutes).Sum() / 60.0; } }
    }
}
