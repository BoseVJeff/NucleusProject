using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class Attendance : System.Web.UI.Page
    {
        AttendanceData attendanceData;
        protected void Page_Load(object sender, EventArgs e)
        {
            int studentId = 3;

            attendanceData = new AttendanceData(studentId);
            attendanceData.Sync();

            //Response.Write(attendanceData.subjectAttendanceMap.ToString());
            AttendanceTable.DataSource= attendanceData.dataSet;
            AttendanceTable.DataBind();
            AttendanceTable.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}