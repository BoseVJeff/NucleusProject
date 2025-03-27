using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NucleusProject;

namespace NucleusProject
{
    public partial class AttendanceDaily : System.Web.UI.Page
    {
        StudentData student;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.student = new StudentData();
            student.Sync();

            // Populate Gridview
            GridView1.DataSource = student;
            GridView1.DataBind();
            GridView1.HeaderRow.TableSection=TableRowSection.TableHeader;
        }
    }
}