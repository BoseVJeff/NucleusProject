using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Services;
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
                NoDataLabel.Visible = true;
            }
            else
            {
                NoDataLabel.Visible = false;
            }

            // Link data to Gridview
            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
            //if (GV_Schedule.HeaderRow != null)
            //{
            //    GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
            //}
            UpdateDateRangeLabel();
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

        protected void FilterBtn_Click(object sender, EventArgs e)
        {
            DateTime fromDateValue;
            DateTime toDateValue;

            if (DateTime.TryParse(fromDate.Text, out fromDateValue) && DateTime.TryParse(toDate.Text, out toDateValue))
            {

                // Clear the current data
                scheduleData = new ScheduleData((int)Session["id"]);

                // Set the new date range
                scheduleData.from = new DateTimeOffset(fromDateValue);
                scheduleData.to = new DateTimeOffset(toDateValue);

                // Sync the data
                scheduleData.Sync();

                if (scheduleData.dataSet.Tables.Count == 0 || scheduleData.dataSet.Tables[0].Rows.Count == 0)
                {
                    // Dataset has no tables or dataset table has no rows
                    NoDataLabel.Visible = true;
                }
                else
                {
                    NoDataLabel.Visible = false;
                }

                // Update the GridView with the new data
                GV_Schedule.DataSource = scheduleData.dataSet;
                GV_Schedule.DataBind();

                // Set the session view to Month and update button stattes
                Session["view"] = ViewSpan.Month;
                Session["view"] = ViewSpan.Week;
                Session["view"] = ViewSpan.Day;
                UpdateButtonStates();
            }
            else
            {
               // Handle invali date input
            }
        }

        private void UpdateButtonStates()
        {
            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    DayBtn.Enabled = false;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = true;
                    break;

                case ViewSpan.Week:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = false;
                    MonthBtn.Enabled = true;
                    break;

                case ViewSpan.Month:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = false;
                    break;
            }
        }

        protected void PreviousBtn_Click(object sender, EventArgs e)
        {
            // logic to navigate to the previous month or week
            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    scheduleData.from = scheduleData.from.AddDays(-1);
                    scheduleData.to = scheduleData.to.AddDays(-1);
                    break;

                case ViewSpan.Week:
                    scheduleData.from = scheduleData.from.AddDays(-7);
                    scheduleData.to = scheduleData.to.AddDays(-7);
                    break;

                case ViewSpan.Month:
                    scheduleData.from = scheduleData.from.AddMonths(-1);
                    scheduleData.from = scheduleData.to.AddMonths(-1);
                    break;
            }

            scheduleData.Sync();
            if(scheduleData.dataSet.Tables.Count == 0 || scheduleData.dataSet.Tables[0].Rows.Count == 0)
            {
                // Dataset has no tables or dataset table has no rows
                NoDataLabel.Visible = true;
            }
            else
            {
                NoDataLabel.Visible = false;
            }
            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            UpdateDateRangeLabel();
        }

        protected void NextBtn_Click(object sender, EventArgs e)
        {
            // logic to navigate to the next month or week or day
            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    scheduleData.from = scheduleData.from.AddDays(1);
                    scheduleData.to = scheduleData.to.AddDays(1).AddTicks(-1);
                    break;

                case ViewSpan.Week:
                    scheduleData.from = scheduleData.from.AddDays(7);
                    scheduleData.to = scheduleData.to.AddDays(7).AddTicks(-7);
                    break;

                case ViewSpan.Month:
                    scheduleData.from = scheduleData.from.AddMonths(1);
                    scheduleData.to = scheduleData.to.AddMonths(1).AddTicks(-1);
                    break;

            }

            
            scheduleData.Sync();
            if (scheduleData.dataSet.Tables.Count == 0 || scheduleData.dataSet.Tables[0].Rows.Count == 0)
            {
                // Dataset has no tables or dataset table has no rows
                NoDataLabel.Visible = true;
            }
            else
            {
                NoDataLabel.Visible = false;
            }
            GV_Schedule.DataSource = scheduleData.dataSet;
            GV_Schedule.DataBind();
            UpdateDateRangeLabel();
        }

        private void UpdateDateRangeLabel()
        {
            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    DateRangeLabel.Text = $"{scheduleData.from: MMMM dd, yyyy}";
                    break;

                case ViewSpan.Week:
                    DateRangeLabel.Text = $"{scheduleData.from: MMMM dd, yyyy} - {scheduleData.to: MMMM dd, yyyy}";
                    break;

                case ViewSpan.Month:
                    DateRangeLabel.Text = $"{scheduleData.from: MMMM dd, yyyy} - {scheduleData.to: MMMM dd, yyyy}";
                    break;
            }

            //DateTime firstDayOfMonth = new DateTime(scheduleData.from.Year, scheduleData.from.Month, 1);
            //DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //DateRangeLabel.Text = $"{firstDayOfMonth: MMMM dd, yyyy} - {lastDayOfMonth: MMMM dd, yyyy}";
        }
    }
}