using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Services;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class ScheduleList : System.Web.UI.Page
    {
        ScheduleData scheduleData;
        bool forceReload = false;

        void LoadAttendanceGrid(int id,TimeDuration duration)
        {
            // Set fromDate and toDate entries
            fromDate.Text = duration.start.LocalDateTime.Date.ToString("yyyy-MM-dd");
            toDate.Text = duration.end.LocalDateTime.ToString("yyyy-MM-dd");

            scheduleData = new ScheduleData(id);
            Console.WriteLine(duration);
            scheduleData.from = duration.start;
            scheduleData.to = duration.end;
            scheduleData.Sync();

            // Populate only if dataset has tables (expected - 1) and that table (idx = 0) has rows
            if(scheduleData.dataSet.Tables.Count>0 && scheduleData.dataSet.Tables[0].Rows.Count>0){
                GV_Schedule.DataSource = scheduleData.dataSet;
                GV_Schedule.DataBind();
                GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
                //GV_Schedule.Controls[0].Controls.Add
                //TableRow row=new TableRow();
                //TableCell cell = new TableCell();
                //cell.ColumnSpan = GV_Schedule.Columns.Count;
                //cell.Text = "";
            }
        }
        // TODO: Store defaults as class property
        void setButtonGroup(ViewSpan? span)
        {
            switch (span)
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
                case ViewSpan.Custom:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = true;
                    break;
                default:
                    // Default view is month
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = false;
                    break;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Session vars used: `id`, `view`, `startDate`, `endDate`

            // All event handlers can assume that `id` is available after this point
            if (Session["id"]==null)
            {
                Response.Redirect("~/");
            }
            // TODO: Actually use this value
            int? id = Values.StudentId(Session, Response.Cookies);
            if (!Page.IsPostBack)
            {
                // First load, check for preferences
                ViewSpan? span=(ViewSpan?)Session["view"];
                int studentId = (int)Session["id"]; // Null check earlier in the function, not needed here

                setButtonGroup(span);
                // Set the appropriate duration - setting default here
                TimeDuration duration=TimeDuration.AroundDateTime(DateTimeOffset.Now,ViewSpan.Month);
                setButtonGroup(ViewSpan.Month);
                if(span==ViewSpan.Custom) // TODO: Maybe more explicit behaviour?
                {
                    if (Session["startDate"] != null && Session["endDate"] != null)
                    {
                        // TODO: Fix workaround
                        // See the TODO in `FilterBtn_Click` for more details
                        duration = new TimeDuration((DateTime)Session["startDate"], ((DateTime)Session["endDate"]).AddDays(1));
                        setButtonGroup(ViewSpan.Custom);
                    } else
                    {
                        // TODO: Maybe log?
                        Debug.WriteLine("[Incomplete data!]"+Session["startDate"]+ " -> " + Session["endDate"]);
                        // Potentially partial data
                        // Reset session vars to known good state
                        Session["startDate"] = null;
                        Session["endDate"] = null;
                        Session["view"] = ViewSpan.Month;
                    }


                } else if(span!=null)
                {
                    // Defined, but not custom
                    duration = TimeDuration.AroundDateTime(DateTime.Now, (ViewSpan)span);
                    setButtonGroup((ViewSpan)span);
                }

                // Fill in the grid
                LoadAttendanceGrid(studentId, duration);
            }
        }

        // Month view button
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            // Store value in session
            Session["view"] = ViewSpan.Month;
            // Set duration and load appropriate gridview
            // TODO: Set this to be around the start date instead
            TimeDuration duration = TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Month);
            //Debug.WriteLine(duration);
            LoadAttendanceGrid((int)Session["id"], duration);
            // Set the button config
            setButtonGroup(ViewSpan.Month);
        }

        // Week view Button
        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            // Store value in session
            Session["view"] = ViewSpan.Week;
            // Set duration and load appropriate gridview
            // TODO: Set this to be around the start date instead
            TimeDuration duration = TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Week);
            //Debug.WriteLine(duration);
            LoadAttendanceGrid((int)Session["id"], duration);
            // Set the button config
            setButtonGroup(ViewSpan.Week);
        }

        // Day view button
        protected void Unnamed_Click2(object sender, EventArgs e)
        {
            // Store value in session
            Session["view"] = ViewSpan.Day;
            // Set duration and load appropriate gridview
            // TODO: Set this to be around the start date instead
            LoadAttendanceGrid((int)Session["id"], TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Day));
            // Set the button config
            setButtonGroup(ViewSpan.Day);
        }

        protected void FilterBtn_Click(object sender, EventArgs e)
        {
            DateTime fromDateValue;
            DateTime toDateValue;

            if (DateTime.TryParse(fromDate.Text, out fromDateValue) && DateTime.TryParse(toDate.Text, out toDateValue))
            {
                //Set start date to be start of day and end date to be the end of day in the local time
                //fromDateValue =new DateTimeOffset

                // TODO: Use the below line to get the client timezone and set date/time accordingly
                // From https://claude.ai/share/5f5f892f-da66-4f4f-bad2-be73ec54850a
                //Debug.WriteLine("Hidden input: "+tzData.Value);

                Session["view"] = ViewSpan.Custom;
                Session["startDate"]= fromDateValue;
                Session["endDate"]= toDateValue;

                // For now, working around by adding one day to the endDate
                // This ensures that the end date selected by the user is included, as expected
                LoadAttendanceGrid((int)Session["id"],new TimeDuration(fromDateValue, toDateValue.AddDays(1)));
                setButtonGroup(ViewSpan.Custom);
            }
            else
            {
                // Invalid value - Default to month view
                Session["view"]=ViewSpan.Month;
                LoadAttendanceGrid((int)Session["id"],TimeDuration.AroundDateTime(DateTime.Now,ViewSpan.Month));
                setButtonGroup(ViewSpan.Month);
               
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