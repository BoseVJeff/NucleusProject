using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class CourseList : System.Web.UI.Page
    {
        CourseData courseData;
        protected void Page_Load(object sender, EventArgs e)
        {
            this.courseData = new CourseData();
            courseData.Sync();

            CourseGrid.DataSource = courseData.dataSet;
            CourseGrid.DataBind();
            CourseGrid.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}