using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NucleusProject
{
    class Grades : DbObject
    {
        public DataSet dataSet;
        public Grades() {
            this.dataSet = new DataSet();
        }
        public override void Sync(string connectionString = null)
        {
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            const string cmd = @"SELECT Id, Name, Explanation, Points FROM E_Grade";
            SqlConnection conn=new SqlConnection(connStr);
            try {
                SqlCommand sqlCommand=new SqlCommand(cmd, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            } finally {
                if (conn.State != ConnectionState.Open) {
                    conn.Close(); 
                } 
            }
        }
    }
    class AttendanceCheck
    {
        public int presentCnt=0;
        public int totalCnt=-1;
        public int allClassCnt = -1;
        public string code = "";

        public AttendanceCheck() { }
    }
    class CourseDetails
    {
        public string code;
        public string school;
        public string schoolShort;
        public CourseDetails() { }
    }
    abstract public class DbObject
    {
        abstract public void Sync(string connectionString=null);
    }

    class AttendanceData:DbObject
    {
        public DataSet dataSet;

        int studentId;
        int presentCount=0;
        int totalCount = 0;
        public Dictionary<string,AttendanceCheck> subjectAttendanceMap = new Dictionary<string,AttendanceCheck>();

        public AttendanceData(int student) {
            this.studentId = student;
            dataSet = new DataSet();
        }

        public override void Sync(string connectionString = null)
        {
            // TODO: Add support for semester choice
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            // Get number of classes 
            const string cmd = @"SELECT Mst_Course.""Name"" AS ""Course"", COUNT(Map_Trn_Schedule_Student_Attendance.""Id"") AS Cnt FROM Trn_Schedule JOIN Mst_Course ON Trn_Schedule.""Course""=Mst_Course.Id LEFT JOIN Map_Trn_Schedule_Student_Attendance ON Map_Trn_Schedule_Student_Attendance.""Schedule"" = Trn_Schedule.""Id"" AND Map_Trn_Schedule_Student_Attendance.""Student""=@student LEFT JOIN E_Attendance ON Map_Trn_Schedule_Student_Attendance.""Attendance"" = E_Attendance.""Id"" GROUP BY Mst_Course.""Name""";
            // Get number of classes student was present in
            const string cmd2 = @"SELECT Mst_Course.""Name"" AS ""Course"", COUNT(Map_Trn_Schedule_Student_Attendance.""Id"") AS Cnt FROM Trn_Schedule JOIN Mst_Course ON Trn_Schedule.""Course""=Mst_Course.Id LEFT JOIN Map_Trn_Schedule_Student_Attendance ON Map_Trn_Schedule_Student_Attendance.""Schedule"" = Trn_Schedule.""Id"" AND Map_Trn_Schedule_Student_Attendance.""Student""=@student LEFT JOIN E_Attendance ON Map_Trn_Schedule_Student_Attendance.""Attendance"" = E_Attendance.""Id"" WHERE Map_Trn_Schedule_Student_Attendance.""Attendance""=1 GROUP BY Mst_Course.""Name""";
            // Get 
            const string cmd3 = @"SELECT Mst_Course.""Name"" AS ""Course"", COUNT(Trn_Schedule.""Id"") AS Cnt FROM Trn_Schedule JOIN Mst_Course ON Trn_Schedule.""Course""=Mst_Course.Id GROUP BY Mst_Course.""Name""";
            // Get code for each course
            const string cmd4 = @"SELECT Mst_Course.""Name"" AS ""Name"", Mst_Course.""Code"" AS ""Code"", Mst_School.""Name"" AS ""School"", Mst_School.""Short_Name"" AS ""SchoolShort"" FROM Mst_Course JOIN Mst_School ON Mst_Course.""School""=Mst_School.""Id"";";
            Dictionary<string,CourseDetails> courseCodeMap= new Dictionary<string,CourseDetails>();
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);
                sqlCommand.Parameters.Add("@student", SqlDbType.BigInt);
                sqlCommand.Parameters["@student"].Value = this.studentId;

                sqlCommand.CommandText = cmd;

                connection.Open();

                SqlDataReader reader= sqlCommand.ExecuteReader();
                Console.WriteLine("CMD 1:");
                while (reader.Read())
                {
                    AttendanceCheck check = new AttendanceCheck();
                    check.totalCnt = reader.GetInt32(1);
                    this.subjectAttendanceMap.Add(reader.GetString(0), check);
                }
                reader.Close();
                // At this point, all subjects should have an entry in the Dictionary

                sqlCommand.CommandText= cmd2;

                reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    this.subjectAttendanceMap[reader.GetString(0)].presentCnt=reader.GetInt32(1);
                }
                reader.Close();

                sqlCommand.CommandText= cmd3;

                reader=sqlCommand.ExecuteReader();
                while (reader.Read()) {
                    this.subjectAttendanceMap[reader.GetString(0)].allClassCnt=reader.GetInt32(1);
                }
                reader.Close();

                sqlCommand.CommandText = cmd4;

                reader=sqlCommand.ExecuteReader();
                while (reader.Read()) {
                    CourseDetails details = new CourseDetails();
                    details.code= reader.GetString(1);
                    details.school=reader.GetString(2);
                    details.schoolShort=reader.GetString(3);
                    courseCodeMap.Add(reader.GetString(0), details);
                }
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            // Convert dictionary to datatable
            dataSet.Clear();
            DataTable dt = new DataTable();
            dataSet.AcceptChanges();

            // Get back to a known state, as Sync is expected to clean and refetch al data
            dt.Reset();

            // Setup columns
            dt.Columns.Add("Course",typeof(String));
            dt.Columns.Add("Present", typeof(int));
            dt.Columns.Add("Total",typeof(int));
            dt.Columns.Add("All",typeof(int));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("School", typeof(string));
            dt.Columns.Add("SchoolShort", typeof(string));

            // Add data to DataTable
            foreach (var item in this.subjectAttendanceMap.ToArray())
            {
                DataRow row= dt.NewRow();
                row["Course"] = item.Key;
                row["Present"] = item.Value.presentCnt;
                row["Total"]=item.Value.totalCnt;
                row["All"] = item.Value.allClassCnt;
                CourseDetails tmp;
                if(courseCodeMap.TryGetValue(item.Key, out tmp))
                {
                    row["Code"] = tmp.code;
                    row["School"] = tmp.school;
                    row["SchoolShort"] = tmp.schoolShort;
                }
                
                dt.Rows.Add(row);
            }
            dataSet.Tables.Add(dt);
            dataSet.AcceptChanges();

            return;
        }
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

        int studentId;

        public DateTimeOffset from=DateTimeOffset.MinValue;

        public DateTimeOffset to=DateTimeOffset.MaxValue;

        public ScheduleData(int studentId)
        {
            this.dataSet = new DataSet();
            this.studentId= studentId;
        }

        // Set the range to be the entire current month.
        // Extra work done to set the hours correctly.
        public void setCurrentMonth() {
            this.to = DateTimeOffset.Now;
            //this.to = DateTimeOffset.FromUnixTimeSeconds(1743206399); // Saturday, March 29, 2025 5:29:59 AM GMT+05:30
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
            const string cmd = @"SELECT Mst_Course.""Name"" AS ""Course"", Mst_Class.""Name"" AS ""Class"", E_Days.""Name"" AS ""Day"", Mst_Faculty.""Name"" AS ""Faculty"", Mst_Faculty.""Email"" AS ""Email"", Mst_Faculty.""Phone"" AS ""Phone"", ""Start"", ""End"", E_Attendance.""Name"" AS ""Attendance"" FROM Trn_Schedule JOIN E_Days ON Trn_Schedule.""Day""=E_Days.Id JOIN Mst_Course ON Trn_Schedule.""Course""=Mst_Course.Id JOIN Mst_Class ON Trn_Schedule.""Class""=Mst_Class.""Id"" JOIN Mst_Faculty ON Trn_Schedule.""Faculty""=Mst_Faculty.""Id"" LEFT JOIN Map_Trn_Schedule_Student_Attendance ON Map_Trn_Schedule_Student_Attendance.""Schedule"" = Trn_Schedule.""Id"" AND Map_Trn_Schedule_Student_Attendance.""Student""=@student LEFT JOIN E_Attendance ON Map_Trn_Schedule_Student_Attendance.""Attendance"" = E_Attendance.""Id"" WHERE Trn_Schedule.""Start"">=@start AND Trn_Schedule.""End""<=@end ORDER BY Trn_Schedule.""Start"" ASC";
            //const string cmd = "SELECT * FROM Mst_Course";
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);

                sqlCommand.Parameters.Add("@start",SqlDbType.BigInt);
                sqlCommand.Parameters.Add("@end", SqlDbType.BigInt);
                sqlCommand.Parameters.Add("@student", SqlDbType.BigInt);
                sqlCommand.Parameters["@start"].Value=this.from.ToUnixTimeSeconds();
                sqlCommand.Parameters["@end"].Value=this.to.ToUnixTimeSeconds();
                sqlCommand.Parameters["@student"].Value = this.studentId;

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