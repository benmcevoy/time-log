using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace TimeLog
{
    public class Log
    {
        private readonly double _dayLength = 8;

        public Log()
        {
            Days = new List<Day>();
            try
            {
                _dayLength = Convert.ToDouble(ConfigurationManager.AppSettings["dayLength"]);
            }
            catch (Exception)
            {
                _dayLength = 8;
            }
        }

        public string GetStatistics(DateTime forDate)
        {
            const string report = "{0}\r\n{1}\r\n{2}";

            return string.Format(report, GetDay(forDate), GetWeekly(forDate), GetTimesheetReport(forDate));
        }

        private string GetDay(DateTime forDate)
        {
            var selectedDay = (from t in Days where t.Date == forDate select t).FirstOrDefault();

            if (selectedDay == null)
            {
                return forDate.ToString(LogParser.LogDateFormat) + " - no entry found";
            }

            SelectedLineNumber = selectedDay.LineNumber;

            var hours = selectedDay.HoursToday;

            var projects = from e in selectedDay.TimeEntries
                           group e by e.ProjectName into g
                           select new
                           {
                               ProjectName = g.Key,
                               TotalDuration = g.Sum(e => e.Duration.TotalHours),
                               Comments = g.Select(c => c.Comments.Aggregate(new StringBuilder(),
                        (a, b) => a.Append("\r\n" + b),
                        a => a.ToString().Replace("\r\n", "")))
                           };

            var projectList = "";
            var projectDetails = "";

            foreach (var project in projects)
            {
                projectList += string.Format("{0} - {1} hours\r\n", project.ProjectName, project.TotalDuration);

                projectDetails += string.Format("{0} - {1} hours\r\n-----------------\r\n{2}\r\n\r\n",
                    project.ProjectName,
                    project.TotalDuration,
                    project.Comments.Aggregate(new StringBuilder(),
                        (a, b) => a.Append("\r\n" + b),
                        a => a.Remove(0, 2).ToString()));
            }

            var result =
@"{0} 
{1} hours

Projects
-----------------
{2}

Details
-----------------
{3}
";

            return string.Format(result, forDate.ToString(LogParser.LogDateFormat), hours, projectList, projectDetails);
        }

        private string GetWeekly(DateTime forDate)
        {
            var daysThisWeek = GetDaysThisWeek(forDate);

            var thisWeek = daysThisWeek as Day[] ?? daysThisWeek.ToArray();
            var hoursThisWeek = (thisWeek.Sum(d => d.HoursToday));

            var projects = from e in thisWeek
                           from d in e.TimeEntries
                           group d by d.ProjectName into g
                           select new
                           {
                               ProjectName = g.Key,
                               TotalDuration = g.Sum(e => e.Duration.TotalHours),
                               Comments = g.Select(c => c.Comments.Aggregate(new StringBuilder(),
                        (a, b) => a.Append("\r\n" + b),
                        a => a.ToString().Replace("\r\n", "")))
                           };

            var projectList = "";
            //var projectDetails = "";

            foreach (var project in projects)
            {
                projectList += string.Format("{0} - {1} hours\r\n", project.ProjectName, project.TotalDuration);

                //projectDetails += string.Format("{0} - {1} hours\r\n-----------------\r\n{2}\r\n\r\n",
                //    project.ProjectName,
                //    project.TotalDuration,
                //    project.Comments.Aggregate(new StringBuilder(),
                //        (a, b) => a.Append("\r\n" + b),
                //        a => a.Remove(0, 2).ToString()));
            }

            return string.Format(@"
This week {0} hours

Projects
-----------------
{1}
 ",
    hoursThisWeek,
    projectList);
        }

        private string GetTimesheetReport(DateTime forDate)
        {
            var daysThisWeek = GetDaysThisWeek(forDate);

            var timesheet = daysThisWeek.Aggregate("",
                (current, day) =>
                    current + string.Format("{0}\t{1}-{2}\t{3}\t({4})\r\n",
                    day.Date.ToString("ddd"),
                    day.TimeEntries.Any() ? day.TimeEntries.First().StartDateTime.ToString("HH:mm") : "",
                    day.TimeEntries.Any() ? day.TimeEntries.Last().EndDateTime.ToString("HH:mm") : "",
                    day.HoursToday,
                    (GetDayDuration(day)) - day.HoursToday));

            return string.Format(@"
Timesheet
-----------------
{0}
", timesheet);
        }

        private IEnumerable<Day> GetDaysThisWeek(DateTime forDate)
        {

            var startOfWeek = StartOfWeek(forDate, DayOfWeek.Monday);
           

            return (from t in Days
                    where t.Date >= startOfWeek
                          && t.Date < startOfWeek.AddDays(7)
                    select t);
        }

        private double GetDayDuration(Day day)
        {
            if (!day.TimeEntries.Any()) return 0;

            var start = day.TimeEntries.First().StartDateTime;
            var end = day.TimeEntries.Last().EndDateTime;
            if (end.Hour < 10) end = end.AddHours(12);

            return (end - start).Duration().Hours;
        }

        internal double GetRemainToday()
        {
            return (from t in Days where t.Date == DateTime.Today select _dayLength - t.HoursToday).FirstOrDefault();
        }

        private DateTime GetDayOfTheWeek(DayOfWeek dayofWeek, DateTime fordate)
        {
            var diff = dayofWeek - fordate.DayOfWeek;
            return fordate.AddDays(diff).Date;
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public List<Day> Days { get; set; }

        public int SelectedLineNumber { get; set; }
    }
}
