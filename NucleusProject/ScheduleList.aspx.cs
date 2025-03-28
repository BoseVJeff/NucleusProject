using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class ScheduleList : System.Web.UI.Page
    {
        ScheduleData scheduleData;
        bool forceReload = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"]==null)
            {
                Response.Redirect("~/");
            }
            // Get data
            scheduleData = new ScheduleData((int) Session["id"]);
            scheduleData.setCurrentWeek();
            //Response.Write(scheduleData.from);
            scheduleData.Sync();

            // Link data to Gridview
            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
    }
}