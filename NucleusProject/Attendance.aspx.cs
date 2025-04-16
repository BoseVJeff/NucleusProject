using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using System.Web.UI.HtmlControls;
using AjaxControlToolkit;
using System.Drawing;
using System.Configuration;
using System.Diagnostics;

namespace NucleusProject
{
    public partial class Attendance : System.Web.UI.Page
    {
        AttendanceData attendanceData;

        const string baseProgressClass = "progress-bar user-select-none";

        private void setBarGraph(double size, HtmlGenericControl barGraph, string cssClass, string value)
        {
            if (barGraph == null)
            {
                return;
            }

            barGraph.Attributes.CssStyle.Add("width", size.ToString() + "%");
            barGraph.Attributes.Add("class", baseProgressClass + " " + cssClass);
            barGraph.Attributes.Add("title", value);

            // Only show the value if the bar size is large enough to fit it
            if (size >= 10)
            {
                barGraph.InnerText = value;
            }
            else
            {
                barGraph.InnerText = ""; // Hide value if bar is too small
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            int? studentId = Values.StudentId(Session, Request.Cookies);
            if (studentId == null)
            {
                Response.Redirect("~/");
            }

            Student student;
            if (Session["student"]!=null)
            {
                student = (Student)Session["student"];
            } else
            {
                student = new Student((int)studentId);
                student.Sync();
                Session["student"] = student;
            }

            ((Label)Master.FindControl("DisplayName")).Text = student.enrNo;

            Semester semester = new Semester((int)studentId);
            semester.Sync();

            SemesterSelect.Items.Clear();

            foreach(SemesterData data in semester.data) {
                ListItem item=new ListItem();
                item.Text = data.name;
                item.Value = data.id.ToString();
                SemesterSelect.Items.Add(item);
            }

            if(!IsPostBack)
            {
                SemesterData currentSemester = SemesterData.GetSemesterDataForDateTimeOffset(DateTimeOffset.Now);
                ListItem cuurentSemItem = SemesterSelect.Items.FindByValue(currentSemester.id.ToString());
                if (cuurentSemItem != null)
                {
                    cuurentSemItem.Selected = true;
                    Session["semesterId"] = Convert.ToInt32(SemesterSelect.SelectedValue);
                    FillAttendanceData(studentId, (int)Session["semesterId"]);
                } else
                {
                    // Maybe add the item in?
                    ListItem item = new ListItem();
                    item.Text = currentSemester.name;
                    item.Value = currentSemester.id.ToString();
                    item.Selected = true;
                    SemesterSelect.Items.Add(item);
                    FillAttendanceData(studentId, currentSemester.id);
                }

                Debug.WriteLine("[Semester Dropdown SelectedValue] " + SemesterSelect.SelectedValue);
            } else
            {
                int selectedValue = Convert.ToInt32(Request.Form[SemesterSelect.UniqueID]);
                SemesterSelect.Items.FindByValue(selectedValue.ToString()).Selected= true;
                FillAttendanceData(studentId, selectedValue);
            }
        }

        private void FillAttendanceData(int? studentId,int semesterId)
        {
            attendanceData = new AttendanceData((int)studentId,semesterId);
            attendanceData.Sync();

            // Data table
            AttendanceRepeater.DataSource = attendanceData.dataSet;
            AttendanceRepeater.DataBind();
            AttendanceRepeater.EnableViewState = false;

            foreach (RepeaterItem item in AttendanceRepeater.Items)
            {

                // Set values
                DataRow AttendanceRow = attendanceData.dataSet.Tables[0].Rows[item.ItemIndex];
                int present = Convert.ToInt32(AttendanceRow["Present"]);
                int total = Convert.ToInt32(AttendanceRow["Total"]);
                int all = Convert.ToInt32(AttendanceRow["All"]);
                int classesLeft = total - present;

                if (total == 0 || all == 0)
                {
                    break;
                }

                double attRatio = (present * 100) / total;
                double minAttRatio = (present * 100.0) / all;
                string minAttRatioString = String.Format("{0:0.0}", minAttRatio);
                double maxAttRatio = ((all - total + present) * 100.0) / all;
                string maxAttRatioString = String.Format("{0:0.0}", maxAttRatio);

                // Sizes of each bar
                // Note that each value is a percentage (ie. out of 100%)

                // Bar thickness in %
                int currentBarThickness = 2;

                // 0 - <min attendance> -> Guaranteed value
                double barOneSize = minAttRatio - 0;
                // <min attendance> - <current attendance> - 0.5% -> Possible drop
                double barTwoSize = attRatio - minAttRatio - (currentBarThickness / 2);
                // <current attendance> - 0.5% - <current attendance> + 0.5% -> Current attendance, +/- 0.5% (ie. Width is always 1%)
                double barThreeSize = currentBarThickness;
                // <current attendance> + 0.5% - <max attendance> -> Possible Gain
                double barFourSize = maxAttRatio - minAttRatio - (currentBarThickness / 2);
                // <max attendance> - 100% -> Impossible value
                double barFiveSize = 100 - maxAttRatio;

                // Get progress bars
                // TODO: Add aria labels and accessiblity titles
                HtmlGenericControl barOne = (HtmlGenericControl)item.FindControl("barone");
                HtmlGenericControl barTwo = (HtmlGenericControl)item.FindControl("bartwo");
                HtmlGenericControl barThree = (HtmlGenericControl)item.FindControl("barthree");
                HtmlGenericControl barFour = (HtmlGenericControl)item.FindControl("barfour");
                HtmlGenericControl barFive = (HtmlGenericControl)item.FindControl("barfive");

                // Set progress bars
                setBarGraph(barOneSize, barOne, "bg-success", $"{minAttRatioString}%"); // Show value
                setBarGraph(barTwoSize, barTwo, "bg-secondary", ""); // No value
                setBarGraph(barThreeSize, barThree, "bg-dark", $"{attRatio:0.0}"); // No value
                setBarGraph(barFourSize, barFour, "bg-secondary", ""); // No value
                setBarGraph(barFiveSize, barFive, "bg-danger", $"{maxAttRatioString}%"); // Show value


                Chart AttendanceChart = (Chart)item.FindControl("AttendanceChart");

                if (total > 0)
                {
                    int i = AttendanceChart.Series["Series1"].Points.AddXY("Present", present);
                    AttendanceChart.Series["Series1"].Points[i].IsValueShownAsLabel = false;
                    if (classesLeft > 0)
                    {
                        AttendanceChart.Series["Series1"].Points.AddXY("", total - present);
                    }



                    AttendanceChart.Palette = ChartColorPalette.None;
                    AttendanceChart.PaletteCustomColors = new Color[] { Color.FromArgb(25, 135, 84), Color.LightGray };

                    // Doughnut docs: https://learn.microsoft.com/en-us/previous-versions/visualstudio/visual-studio-2012/dd456717(v=vs.110)
                    // Includes list of custom properties
                    AttendanceChart.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                    AttendanceChart.Series["Series1"]["DoughnutRadius"] = "35";
                    AttendanceChart.Series["Series1"]["PieStartAngle"] = "270";
                    AttendanceChart.Series["Series1"]["Clockwise"] = "False";
                    AttendanceChart.Series["Series1"].IsValueShownAsLabel = false;
                    ElementPosition position = new ElementPosition();
                    AttendanceChart.ChartAreas[0].InnerPlotPosition = position;


                    AttendanceChart.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

                    AttendanceChart.ChartAreas[0].BackColor = System.Drawing.Color.FromArgb(248, 249, 250);



                    //Retrieve the value of getCurrentRatio() for the current item
                    string currentRatio = getCurrentRatio(AttendanceRow);
                    //Add text annotation to the center of the chart
                    TextAnnotation annotation = new TextAnnotation();
                    annotation.Text = currentRatio;
                    //Center Horizontally
                    annotation.X = 30;
                    //Center Vertically
                    annotation.Y = 42.5;
                    annotation.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
                    annotation.ForeColor = System.Drawing.Color.Black;
                    annotation.Alignment = ContentAlignment.MiddleCenter;
                    annotation.AnchorX = 50;
                    annotation.AnchorY = 50;
                    AttendanceChart.Annotations.Add(annotation);
                }
                else
                {
                    int i = AttendanceChart.Series["Series1"].Points.AddXY("dummy", 100);
                    AttendanceChart.Series["Series1"].Points[i].IsValueShownAsLabel = false;


                    AttendanceChart.Palette = ChartColorPalette.None;
                    AttendanceChart.PaletteCustomColors = new Color[] { Color.Gray };
                    AttendanceChart.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                    AttendanceChart.Series["Series1"]["DoughnutRadius"] = "35";
                    AttendanceChart.Series["Series1"].IsValueShownAsLabel = false;
                    ElementPosition position = new ElementPosition();
                    AttendanceChart.ChartAreas[0].InnerPlotPosition = position;


                    AttendanceChart.BackColor = System.Drawing.Color.FromArgb(248, 249, 250);

                    AttendanceChart.ChartAreas[0].BackColor = System.Drawing.Color.FromArgb(248, 249, 250);



                    //Retrieve the value of getCurrentRatio() for the current item
                    string currentRatio = "0%";
                    //Add text annotation to the center of the chart
                    TextAnnotation annotation = new TextAnnotation();
                    annotation.Text = currentRatio;
                    //Center Horizontally
                    annotation.X = 35;
                    //Center Vertically
                    annotation.Y = 42.5;
                    annotation.Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
                    annotation.ForeColor = System.Drawing.Color.Black;
                    annotation.Alignment = ContentAlignment.MiddleCenter;
                    annotation.AnchorX = 50;
                    annotation.AnchorY = 50;
                    AttendanceChart.Annotations.Add(annotation);
                }
            }
        }

        protected string getCurrentRatio(DataRow row)
        {
            object present = row["Present"];
            object total = row["Total"];
            
            if(present is System.DBNull || total is System.DBNull)
            {
                //Some data is unavailable
                return "";
            } else
            {
                //Both fields are present.
                int pre = (int)present;
                int tot = (int)total;

                if (tot == 0)
                {
                    //No classes have been marked present/absent
                    return "";
                }

                int ratio = (int)((pre * 100) / tot);
                //Round to two decimal palces
                return ratio.ToString() + "%";
            }
        }

        protected void SemesterSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Assuming that user is logged in and session var is set
            FillAttendanceData((int)Session["id"], (int)Session["semesterId"]);
        }
    }
}
