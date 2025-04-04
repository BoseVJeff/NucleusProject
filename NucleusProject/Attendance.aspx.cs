using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Data;

namespace NucleusProject
{
    public partial class Attendance : System.Web.UI.Page
    {
        AttendanceData attendanceData;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"] == null)
            {
                Response.Redirect("~/");
            }
            int studentId = (int) Session["id"];

            attendanceData = new AttendanceData(studentId);
            attendanceData.Sync();

            // Data table
            AttendanceRepeater.DataSource= attendanceData.dataSet;
            AttendanceRepeater.DataBind();
            AttendanceRepeater.EnableViewState = false;

            //// Add data points to the chart
            //Chart1.Series["Series1"].Points.AddXY("Category 1", 76);
            //Chart1.Series["Series1"].Points.AddXY("Category 2", 56);
            //Chart1.Series["Series1"].Points.AddXY("Category 3", 88);
            //Chart1.Series["Series1"].Points.AddXY("Category 4", 65);

            //// Set chart appearance
            //Chart1.Series["Series1"].Color = System.Drawing.Color.SteelBlue;
            //Chart1.Series["Series1"].ChartType = SeriesChartType.Doughnut;

            foreach (RepeaterItem item in AttendanceRepeater.Items)
            {
                //Label LblCode = (Label)item.FindControl("CourseCode"); // Find chart

                //Response.Write(item.ItemIndex + ": " + LblCode.Text + attendanceData.dataSet.Tables[0].Rows[item.ItemIndex]["Course"] + "<br>");
                // Set values
                DataRow AttendanceRow = attendanceData.dataSet.Tables[0].Rows[item.ItemIndex];
                int present = Convert.ToInt32(AttendanceRow["Present"]);
                int total = Convert.ToInt32(AttendanceRow["Total"]);
                int all = Convert.ToInt32(AttendanceRow["All"]);
                int absent = total - present;

                Chart AttendanceChart = (Chart)item.FindControl("AttendanceChart");

                if (total > 0)
                {
                    int i=AttendanceChart.Series["Series1"].Points.AddXY("Present",present);
                    AttendanceChart.Series["Series1"].Points[i].IsValueShownAsLabel = false;
                    if (absent > 0)
                    {
                        AttendanceChart.Series["Series1"].Points.AddXY("",total - present);
                    }

                    AttendanceChart.Legends[0].BackColor= System.Drawing.Color.FromArgb(248, 249, 250);

                    AttendanceChart.Series["Series1"].Color = System.Drawing.Color.SteelBlue;
                    AttendanceChart.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                    AttendanceChart.Series["Series1"].IsValueShownAsLabel = false;
                    ElementPosition position = new ElementPosition();
                    AttendanceChart.ChartAreas[0].InnerPlotPosition = position;
                    
                    //Title title=new Title   
                    //AttendanceChart.Titles
                        //Legend legend = new Legend();
                    //AttendanceChart.Legends.Add(legend);
                    AttendanceChart.BackColor = System.Drawing.Color.FromArgb(248,249,250);
                    AttendanceChart.ChartAreas[0].BackColor=System.Drawing.Color.FromArgb(248, 249, 250);
                } else
                {
                    AttendanceChart.Visible= false;
                }
            }
        }
    }
}