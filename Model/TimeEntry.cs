using System;
using System.Collections.Generic;

namespace TimeLog.Model
{
    public class TimeEntry
    {
        public TimeEntry()
        {
            Comments = new List<string>();
        }

        public TimeEntry(Period period)
            : this()
        {
            StartDateTime = period.StartTime;
            EndDateTime = period.EndTime;
        }

        public void AddComment(string comment)
        {
            comment = comment.Trim();

            if (!Comments.Contains(comment))
            {
                Comments.Add(comment);
            }
        }

        public TimeSpan Duration { get { return EndDateTime.Subtract(StartDateTime); } }

        public double Minutes { get { return Duration.TotalMinutes; } }

        public DateTime StartDateTime { get; private set; }

        public DateTime EndDateTime { get; private set; }

        public string ProjectName { get; private set; }

        public List<string> Comments { get; private set; }

        public override string ToString()
        {
            // return a formatted entry 
            //
            // 3:00-3:15    ProjectName.
            // here are some comments
            //
            return string.Format("{0}-{1}{2}{3}{4}{5}", 
                StartDateTime.ToShortTimeString(), 
                EndDateTime.ToShortTimeString(), 
                "\t", 
                ProjectName, 
                "\r\n", 
                Comments);
        }
    }
}
