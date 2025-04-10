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

        void LoadAttendanceGrid(int id, TimeDuration duration)
        {
            // Set fromDate and toDate entries
            fromDate.Text = duration.start.Date.ToString("yyyy-MM-dd");
            toDate.Text = duration.end.Date.ToString("yyyy-MM-dd");

            // Set the span
            DateRangeLabel.Text = duration.start.Date.ToString("dd MMM, yyyy") + " - " + duration.end.Date.ToString("dd MMM, yyyy");

            scheduleData = new ScheduleData(id);
            Console.WriteLine(duration);
            scheduleData.from = duration.start;
            scheduleData.to = duration.end;
            scheduleData.Sync();

            // Populate only if dataset has tables (expected - 1) and that table (idx = 0) has rows
            if (scheduleData.dataSet.Tables.Count > 0 && scheduleData.dataSet.Tables[0].Rows.Count > 0)
            {
                GV_Schedule.Visible = true;
                NoDataLabel.Visible = false;

                GV_Schedule.DataSource = scheduleData.dataSet;
                GV_Schedule.DataBind();
                GV_Schedule.HeaderRow.TableSection = TableRowSection.TableHeader;
                //GV_Schedule.Controls[0].Controls.Add
                //TableRow row=new TableRow();
                //TableCell cell = new TableCell();
                //cell.ColumnSpan = GV_Schedule.Columns.Count;
                //cell.Text = "";
            }
            else
            {
                // Dataset has no rows
                GV_Schedule.Visible = false;
                NoDataLabel.Visible = true;
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
                    PreviousBtn.Disabled = false;
                    NextBtn.Disabled = false;
                    break;
                case ViewSpan.Week:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = false;
                    MonthBtn.Enabled = true;
                    PreviousBtn.Disabled = false;
                    NextBtn.Disabled = false;
                    break;
                case ViewSpan.Month:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = false;
                    PreviousBtn.Disabled = false;
                    NextBtn.Disabled = false;
                    break;
                case ViewSpan.Custom:
                    DayBtn.Enabled = true;
                    WeekBtn.Enabled = true;
                    MonthBtn.Enabled = true;
                    // Disable back/forward for custom ranges
                    // TODO: Maybe go back/forward by the length of the current date range?
                    PreviousBtn.Disabled = false;
                    NextBtn.Disabled = false;
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
            if (Session["id"] == null)
            {
                Response.Redirect("~/");
            }
            // TODO: Actually use this value
            int? id = Values.StudentId(Session, Response.Cookies);
            if (!Page.IsPostBack)
            {
                // First load, check for preferences
                ViewSpan? span = (ViewSpan?)Session["view"];
                int studentId = (int)Session["id"]; // Null check earlier in the function, not needed here

                setButtonGroup(span);
                // Set the appropriate duration - setting default here
                TimeDuration duration = TimeDuration.AroundDateTime(DateTimeOffset.Now, ViewSpan.Month);
                Session["startDate"] = duration.start.DateTime;
                Session["endDate"] = duration.end.DateTime;
                Session["view"] = ViewSpan.Month;
                setButtonGroup(ViewSpan.Month);
                if (span == ViewSpan.Custom) // TODO: Maybe more explicit behaviour?
                {
                    if (Session["startDate"] != null && Session["endDate"] != null)
                    {
                        // TODO: Fix workaround
                        // See the TODO in `FilterBtn_Click` for more details
                        duration = new TimeDuration((DateTime)Session["startDate"], ((DateTime)Session["endDate"]).AddDays(1).AddSeconds(-1));
                        setButtonGroup(ViewSpan.Custom);
                    }
                    else
                    {
                        // TODO: Maybe log?
                        Debug.WriteLine("[Incomplete data!]" + Session["startDate"] + " -> " + Session["endDate"]);
                        // Potentially partial data
                        // Reset session vars to known good state
                        Session["startDate"] = null;
                        Session["endDate"] = null;
                        Session["view"] = ViewSpan.Month;
                    }


                }
                else if (span != null)
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
            // TODO: Set this to be around the start date instead
            TimeDuration duration = TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Month);
            // Store value in session
            Session["view"] = ViewSpan.Month;
            Session["startDate"] = duration.start;
            Session["endDate"] = duration.end;
            // Set duration and load appropriate gridview
            //Debug.WriteLine(duration);
            LoadAttendanceGrid((int)Session["id"], duration);
            // Set the button config
            setButtonGroup(ViewSpan.Month);
        }

        // Week view Button
        protected void Unnamed_Click1(object sender, EventArgs e)
        {
            // TODO: Set this to be around the start date instead
            TimeDuration duration = TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Week);
            // Store value in session
            Session["view"] = ViewSpan.Week;
            Session["startDate"] = duration.start.DateTime;
            Session["endDate"] = duration.end.DateTime;
            // Set duration and load appropriate gridview
            //Debug.WriteLine(duration);
            LoadAttendanceGrid((int)Session["id"], duration);
            // Set the button config
            setButtonGroup(ViewSpan.Week);
        }

        // Day view button
        protected void Unnamed_Click2(object sender, EventArgs e)
        {
            // TODO: Set this to be around the start date instead
            TimeDuration duration = TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Day);
            // Store value in session
            Session["view"] = ViewSpan.Day;
            Session["startDate"] = duration.start.DateTime;
            Session["endDate"] = duration.end.DateTime;
            // Set duration and load appropriate gridview
            // TODO: Set this to be around the start date instead
            LoadAttendanceGrid((int)Session["id"], duration);
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
                Session["startDate"] = fromDateValue;
                Session["endDate"] = toDateValue;

                // For now, working around by adding one day to the endDate
                // This ensures that the end date selected by the user is included, as expected
                LoadAttendanceGrid((int)Session["id"], new TimeDuration(fromDateValue, toDateValue.AddDays(1).AddSeconds(-1)));
                setButtonGroup(ViewSpan.Custom);
            }
            else
            {
                // Invalid value - Default to month view
                Session["view"] = ViewSpan.Month;
                LoadAttendanceGrid((int)Session["id"], TimeDuration.AroundDateTime(DateTime.Now, ViewSpan.Month));
                setButtonGroup(ViewSpan.Month);

            }
        }

        protected void PreviousBtn_Click(object sender, EventArgs e)
        {
            DateTimeOffset start = (DateTime)Session["startDate"];
            DateTimeOffset end = (DateTime)Session["endDate"];
            int id = (int)Session["id"];
            TimeDuration timeDuration=TimeDuration.AroundDateTime(DateTimeOffset.Now,ViewSpan.Day);
            //Debug.WriteLine(Session["view"]);

            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    timeDuration = new TimeDuration(start.AddDays(-1), end.AddDays(-1));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Week:
                    // Assuming 7 days in a week
                    timeDuration = new TimeDuration(start.AddDays(-7), end.AddDays(-7));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Month:
                    // Every month is different in size so duration needs to be recomputed every time
                    DateTimeOffset newRef = start.AddMonths(-1);
                    timeDuration = TimeDuration.AroundDateTime(newRef, ViewSpan.Month);
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Custom:
                    TimeSpan timeSpan = end - start;
                    timeDuration = new TimeDuration(start.Add(-timeSpan), end.Add(-timeSpan));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
            }
            Session["startDate"] = timeDuration.start.DateTime;
            Session["endDate"] = timeDuration.end.DateTime;
        }

        protected void NextBtn_Click(object sender, EventArgs e)
        {
            DateTimeOffset start = (DateTime)Session["startDate"];
            DateTimeOffset end = (DateTime)Session["endDate"];
            int id = (int)Session["id"];
            TimeDuration timeDuration = TimeDuration.AroundDateTime(DateTimeOffset.Now, ViewSpan.Day);
            //Debug.WriteLine(Session["view"]);

            switch ((ViewSpan)Session["view"])
            {
                case ViewSpan.Day:
                    timeDuration = new TimeDuration(start.AddDays(1), end.AddDays(1));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Week:
                    // Assuming 7 days in a week
                    timeDuration = new TimeDuration(start.AddDays(7), end.AddDays(7));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Month:
                    // Every month is different in size so duration needs to be recomputed every time
                    DateTimeOffset newRef = start.AddMonths(1);
                    timeDuration = TimeDuration.AroundDateTime(newRef,ViewSpan.Month);
                    LoadAttendanceGrid(id, timeDuration);
                    break;
                case ViewSpan.Custom:
                    TimeSpan timeSpan = end - start;
                    timeDuration = new TimeDuration(start.Add(timeSpan), end.Add(timeSpan));
                    LoadAttendanceGrid(id, timeDuration);
                    break;
            }
            Session["startDate"] = timeDuration.start.DateTime;
            Session["endDate"] = timeDuration.end.DateTime;
        }
    }
}