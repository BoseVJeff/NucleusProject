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

 

            foreach (RepeaterItem item in AttendanceRepeater.Items)
            {
               
                // Set values
                DataRow AttendanceRow = attendanceData.dataSet.Tables[0].Rows[item.ItemIndex];
                int present = Convert.ToInt32(AttendanceRow["Present"]);
                int total = Convert.ToInt32(AttendanceRow["Total"]);
                int all = Convert.ToInt32(AttendanceRow["All"]);
                int classesLeft = total - present;

                double minAttRatio = (present * 100.0) / all;
                string minAttRatioString = String.Format("{0:0.0}",minAttRatio);
                double maxAttRatio = ((all - total + present) * 100.0) / all;
                string maxAttRatioString= String.Format("{0:0.0}",maxAttRatio);

                // Remember to leave a space at the end here
                string baseProgressClass = "progress-bar user-select-none ";

                // Set progress bar
                HtmlGenericControl minAttProgress = (HtmlGenericControl)item.FindControl("minattprogress");
                minAttProgress.Attributes.CssStyle.Add("width",minAttRatio.ToString()+"%");
                if(minAttRatio<50)
                {
                    // No possiblity of exam. Danger
                    minAttProgress.Attributes.Add("class", baseProgressClass + "bg-danger");
                } else if(minAttRatio>=50 && minAttRatio<80)
                {
                    // Grade drop. Warning
                    minAttProgress.Attributes.Add("class", baseProgressClass + "bg-warning");
                } else if(minAttRatio>=80)
                {
                    // No grade drop. Fine
                    minAttProgress.Attributes.Add("class", baseProgressClass + "bg-success");
                } else
                {
                    // Default styling
                    minAttProgress.Attributes.Add("class", baseProgressClass + "bg-secondary");
                }
                minAttProgress.Attributes.Add("aria-valuenow", minAttRatioString);
                minAttProgress.InnerText= minAttRatioString;

                HtmlGenericControl maxAttProgress = (HtmlGenericControl)item.FindControl("maxattprogress");
                maxAttProgress.Attributes.CssStyle.Add("width", (maxAttRatio-minAttRatio).ToString()+"%");
                maxAttProgress.Attributes.Add("aria-valuenow",maxAttRatioString);
                maxAttProgress.InnerText= maxAttRatioString;

                Chart AttendanceChart = (Chart)item.FindControl("AttendanceChart");

                if (total > 0)
                {
                    int i=AttendanceChart.Series["Series1"].Points.AddXY("Present",present);
                    AttendanceChart.Series["Series1"].Points[i].IsValueShownAsLabel = false;
                    if (classesLeft > 0)
                    {
                        AttendanceChart.Series["Series1"].Points.AddXY("",total - present);
                    }


                    
                    AttendanceChart.Palette = ChartColorPalette.None;
                    AttendanceChart.PaletteCustomColors = new Color[] { Color.FromArgb(25, 135, 84), Color.LightGray };
                    AttendanceChart.Series["Series1"].ChartType = SeriesChartType.Doughnut;
                    AttendanceChart.Series["Series1"]["DoughnutRadius"] = "35";
                    AttendanceChart.Series["Series1"].IsValueShownAsLabel = false;
                    ElementPosition position = new ElementPosition();
                    AttendanceChart.ChartAreas[0].InnerPlotPosition = position;

                    
                    AttendanceChart.BackColor = System.Drawing.Color.FromArgb(248, 249,250);
                    
                    AttendanceChart.ChartAreas[0].BackColor=System.Drawing.Color.FromArgb(248, 249, 250);
                    


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
                    AttendanceChart.PaletteCustomColors = new Color[] {Color.Gray };
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
    }
}
