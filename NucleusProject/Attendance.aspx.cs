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

            // Data table
            AttendanceRepeater.DataSource= attendanceData.dataSet;
            AttendanceRepeater.DataBind();
            AttendanceRepeater.EnableViewState = false;
        }
    }
}