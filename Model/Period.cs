using System;

namespace TimeLog.Model
{
    public class Period
    {
        public Period(DateTime startTime, DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }
    }
}
