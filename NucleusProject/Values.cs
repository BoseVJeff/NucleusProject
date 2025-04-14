using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.SessionState;

namespace NucleusProject
{
    public enum ViewSpan
    {
        Day,
        Week,
        Month,
        Custom
    }
    public class TimeDuration
    {
        public DateTimeOffset start;
        public DateTimeOffset end;

        public TimeDuration(DateTimeOffset startDateTimeOffset, DateTimeOffset endDateTimeOffset)
        {
            this.start = startDateTimeOffset;
            this.end = endDateTimeOffset;
        }

        public static TimeDuration AroundDateTime(DateTimeOffset? dateTimeOffset,ViewSpan span) {
            // Setting a sensible default
            DateTimeOffset date = dateTimeOffset ?? DateTimeOffset.Now;

            // Holders for eventual values
            DateTimeOffset startDate;
            DateTimeOffset endDate;

            // Set start and end dates based on the span
            switch (span)
            {
                case ViewSpan.Day:
                    // Start at first minute of the current day
                    startDate= new DateTimeOffset(year: date.Year, month: date.Month, day: date.Day, hour: 0, minute: 0,second: 0, offset: date.Offset);
                    // End at the last minute of the current day
                    endDate= new DateTimeOffset(year: date.Year, month: date.Month, day: date.Day, hour: 23, minute: 59, second: 59, offset: date.Offset);
                    break;
                case ViewSpan.Week:
                    // Week starts on Sunday
                    // TODO: Customisable start of week? Will require additional tests to verify functionality

                    // Start at first minute of the first day of the current week
                    startDate = date.AddDays(-((int)date.DayOfWeek) + ((int)DayOfWeek.Sunday));
                    startDate=new DateTimeOffset(startDate.Year,startDate.Month,startDate.Day,0,0,0,startDate.Offset);
                    // End at the last minute of the last day of the current week
                    endDate = date.AddDays(-(int)date.DayOfWeek + (int)DayOfWeek.Saturday);
                    endDate=new DateTimeOffset(endDate.Year,endDate.Month,endDate.Day,23,59,59,endDate.Offset);
                    break;
                case ViewSpan.Month:
                    // Start at first day of the current month
                    startDate = new DateTimeOffset(year: date.Year, month: date.Month, day: 1, hour: 0, minute: 0, second: 0, offset: date.Offset);
                    // End at the last day of the current month
                    endDate = new DateTimeOffset(year: date.Year, month: date.Month, day: DateTime.DaysInMonth(date.Year,date.Month), hour: 23, minute: 59, second: 59, offset: date.Offset);
                    break;
                default:
                    // For unknown spans
                    startDate = DateTime.Now;
                    endDate = DateTime.Now;
                    break;
            }

            return new TimeDuration(startDate, endDate);
        }

        public override string ToString()
        {
            return start.ToString() + " - "+ end.ToString();
        }
    }
    public class Values
    {
        private static readonly string baseDir = @"~\";

        public static string ConnectionString { get {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"(LocalDB)\MSSQLLocalDB";
                builder.AttachDBFilename = HostingEnvironment.MapPath(Path.Combine(baseDir, @"App_Data\NucleusDatabase.mdf"));
                builder.IntegratedSecurity= true;

                return builder.ToString();
            } }

        public static int? StudentId(HttpSessionState session, HttpCookieCollection cookies)
        {
            // Check session for value first, then cookies, else return null
            // Seaching for key `id`

            if (session["id"] != null)
            {
                return (int)session["id"];
            } else
            {
                // No current session, maybe because the user has not logged in
                // Fallthrough to cookie id
            }

            if (cookies != null)
            {
                if (cookies["id"] != null)
                {
                    int outId;
                    if (Int32.TryParse(cookies["id"].Value, out outId))
                    {
                        return outId;
                    }
                    else
                    {
                        // Failed to parse cookie
                        // TODO: Maybe log here?
                        return null;
                    }
                }
                else
                {
                    // No `id` cookie found
                    return null;
                }
            } else
            {
                // No cookies
                return null;
            }
        }

    }
}