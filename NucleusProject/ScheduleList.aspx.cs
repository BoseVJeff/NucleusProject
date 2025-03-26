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
        protected void Page_Load(object sender, EventArgs e)
        {
            scheduleData = new ScheduleData();
            scheduleData.Sync();

            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            GV_Schedule.HeaderRow.TableSection=TableRowSection.TableHeader;
        }
    }
}