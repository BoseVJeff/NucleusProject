using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public enum ViewSpan
    {
        Day,
        Week,
        Month
    }
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
            if (Session["view"]==null)
            {
                // Default view
                Session["view"]=ViewSpan.Month;
            }
            // Get data
            scheduleData = new ScheduleData((int)Session["id"]);
            // Disable the selected button and set the corresponding view
            switch ((ViewSpan) Session["view"])
            {
                case ViewSpan.Day:
                    DayBtn.Enabled = false;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = true;
                    scheduleData.setCurrentDay();
                    break;
                case ViewSpan.Week:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = false;
                    MonthBtn.Enabled = true;
                    scheduleData.setCurrentWeek();
                    break;
                case ViewSpan.Month:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = false;
                    scheduleData.setCurrentMonth();
                    break;
            }
            scheduleData.Sync();

            if(scheduleData.dataSet.Tables.Count==0 || scheduleData.dataSet.Tables[0].Rows.Count==0) {
                // Dataset has no tables or dataset table has no rows

            }

            // Link data to Gridview
            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
            //if (GV_Schedule.HeaderRow != null)
            //{
            //    GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Session["view"] = ViewSpan.Month;
            Page_Load(sender,e);
        }

        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            Session["view"] = ViewSpan.Week;
            Page_Load(sender, e);
        }

        protected void Unnamed_Click2(object sender, EventArgs e)
        {
            Session["view"] = ViewSpan.Day;
            Page_Load(sender, e);
        }
    }
}