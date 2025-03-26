using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NucleusProject
{
    abstract public class DbObject
    {
        abstract public void Sync(string connectionString=null);
    }

    /// <summary>
    /// Gets data about the class schedule.
    /// 
    /// <para>
    /// Note that you MUST call <c>Sync()</c> on the object, as the <c>dataSet</c> is not populated without it.
    /// </para>
    /// 
    /// <para>
    /// By default, it collects all entries from <c>DateTimeOffset.MinValue</c> to <c>DateTimeOffset.MaxValue</c>.
    /// These can be set using <c>from</c> and <c>to</c> respectively, preferably before calling <c>Sync()</c>.
    /// </para>
    /// <para>
    /// Additionally, this class provides three utility classes (<c>setCurrentDay()</c>,<c>setCurrentMonth()</c>, & <c>setCurrentWeek()</c> to set these values directly.
    /// These values are then set such that the data fetched is from the current day/month/week respectively.
    /// </para>
    /// </summary>
    public class ScheduleData : DbObject
    {
        public DataSet dataSet;

        public DateTimeOffset from=DateTimeOffset.MinValue;

        public DateTimeOffset to=DateTimeOffset.MaxValue;

        public ScheduleData()
        {
            this.dataSet = new DataSet();
        }

        // Set the range to be the entire current month.
        // Extra work done to set the hours correctly.
        public void setCurrentMonth() {
            this.to = DateTimeOffset.Now;
            // End of last day
            this.to = to.AddHours(-to.Hour).AddHours(24);
            // Last day of month
            this.to=this.to.AddDays(-this.to.Day).AddDays(DateTime.DaysInMonth(to.Year,to.Month));
            this.from = this.to.AddMonths(-1);
            // Start of first day
            //this.from = this.from.AddHours(-24);
        }
        // Set the range to be the entire current week
        public void setCurrentWeek()
        {
            this.to=DateTimeOffset.Now;
            // Extension of trick from https://stackoverflow.com/a/3215924
            this.to = to.AddHours(-to.Hour).AddHours(24);
            this.to = to.AddDays(-(int)this.to.DayOfWeek).AddDays((int)DayOfWeek.Saturday);
            this.from = this.to.AddDays(-7);
        }
        public void setCurrentDay()
        {
            this.to = DateTimeOffset.Now;
            // Assuming 24 hours in a day
            this.to=to.AddHours(-to.Hour).AddHours(24);
            this.from= this.to.AddDays(-1);
        }
        public override void Sync(string connectionString=null)
        {
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            const string cmd = @"SELECT Mst_Course.""Name"" AS ""Course"", Mst_Class.""Name"" AS ""Class"", E_Days.""Name"" AS ""Day"", E_Class_Status.""Name"" AS ""Status"", Mst_Faculty.""Name"" AS ""Faculty"", Mst_Faculty.""Email"" AS ""Email"", Mst_Faculty.""Phone"" AS ""Phone"", ""Start"", ""End"" FROM Trn_Schedule JOIN E_Class_Status ON Trn_Schedule.""Status""=E_Class_Status.Id JOIN E_Days ON Trn_Schedule.""Day""=E_Days.Id JOIN Mst_Course ON Trn_Schedule.""Course""=Mst_Course.Id JOIN Mst_Class ON Trn_Schedule.""Class""=Mst_Class.""Id"" JOIN Mst_Faculty ON Trn_Schedule.""Faculty""=Mst_Faculty.""Id"" WHERE Trn_Schedule.""Start"">=@start AND Trn_Schedule.""End""<=@end ORDER BY Trn_Schedule.""Start"" ASC";
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);
                sqlCommand.Parameters.Add("@start",SqlDbType.BigInt);
                sqlCommand.Parameters.Add("@end", SqlDbType.BigInt);
                sqlCommand.Parameters["@start"].Value=this.from.ToUnixTimeSeconds();
                sqlCommand.Parameters["@end"].Value=this.to.ToUnixTimeSeconds();

                sqlCommand.CommandText = cmd;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return;
        }
    }

    public class CourseData : DbObject
    {
        public DataSet dataSet;
        public CourseData() {
            this.dataSet = new DataSet();
        }
        public override void Sync(string connectionString=null)
        {
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            const string cmd = "SELECT * FROM Mst_Course";
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);
                sqlCommand.CommandText = cmd;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return;
        }
    }

    public class StudentData : DbObject
    {
        public StudentData()
        {
            this.dataSet = new DataSet();
        }
        public DataSet dataSet;
        public override void Sync(string connectionString=null)
        {
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            const string cmd = "SELECT * FROM Mst_Student";
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);
                sqlCommand.CommandText = cmd;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return;
        }
    }
}