using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class Report : System.Web.UI.Page
    {
        string path = @"~/favicon.ico";
        protected void Page_Load(object sender, EventArgs e)
        {
            int? studentId = Values.StudentId(Session, Request.Cookies);
            if (studentId == null)
            {
                Response.Redirect("~/");
            }
            Student student;
            if (Session["student"] == null)
            {
                // this is somehow unavailable while studentId is available
                // Reset it
                student = new Student((int)studentId);
                student.Sync();
                Session["student"] = student;
            }
            else
            {
                student = (Student)Session["student"];
            }
            ((Label)Master.FindControl("DisplayName")).Text = student.enrNo;

            // Setting values in the dropdown
            Semester semesters = new Semester((int)studentId);
            semesters.Sync();
            SemesterSelect.Items.Clear();
            foreach (SemesterData data in semesters.data)
            {
                ListItem item = new ListItem();
                item.Text = data.name;
                item.Value = data.id.ToString();
                SemesterSelect.Items.Add(item);
            }

            SemesterData currentSemester = SemesterData.GetSemesterDataForDateTimeOffset(DateTimeOffset.Now);

            if (!Page.IsPostBack)
            {
                // Default dropdown value is the current value
                ListItem itemInDropdown = SemesterSelect.Items.FindByValue(currentSemester.id.ToString());
                if (itemInDropdown != null)
                {
                    // Current semester is available in the dropdown
                    itemInDropdown.Selected = true;
                } else
                {
                    // Current semester is not available in the dropdown
                    // This may be because the current result has not been announced
                    // In that case, add the item to dropdown, make it active, and show that no result is available
                    // Also disable the print button
                    ListItem item = new ListItem();
                    item.Text = currentSemester.name;
                    item.Value = currentSemester.id.ToString();
                    item.Selected = true;
                    SemesterSelect.Items.Add(item);
                }
                //Session["semester"] = currentSemester;
            }
            else
            {
                // Dropdown should have value
                Debug.Print("New Value: " + Request.Form[SemesterSelect.UniqueID]);
                // From https://claude.ai/share/d37aa874-c7fc-4c21-b087-365bbc6db515
                SemesterSelect.Items.FindByValue(Request.Form[SemesterSelect.UniqueID]).Selected = true;
            }

            //SemesterData selectedSemester = (SemesterData)Session["semester"];
            SemesterData selectedSemester = new SemesterData(Convert.ToInt32(Request.Form[SemesterSelect.UniqueID]));
            selectedSemester.Sync();

            PopulateResults(student, selectedSemester);

            Grades grades = new Grades();
            grades.Sync();
            GradeExplanation.DataSource = grades.dataSet;
            GradeExplanation.DataBind();
            if (GradeExplanation.HeaderRow != null)
            {
                GradeExplanation.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

        }

        private void PopulateResults(Student student, SemesterData currentSemester)
        {
            SemesterGrades semesterGrades = new SemesterGrades(currentSemester.id, (int)student.id);
            semesterGrades.Sync();
            GV_Result.DataSource = semesterGrades.dataSet;
            GV_Result.DataBind();
            if (GV_Result.Rows.Count > 0)
            {
                GV_Result.HeaderRow.TableSection = TableRowSection.TableHeader;
                PrintReport.Enabled = true;
            } else
            {
                PrintReport.Enabled = false;
            }
                RegCredits.Text = semesterGrades.regCreditSum.ToString();
            Credits.Text = semesterGrades.creditSum.ToString();
            Points.Text = semesterGrades.pointSum.ToString();
            if (semesterGrades.creditSum != 0)
            {
                Sgpa.Text = String.Format("{0:0.00}", semesterGrades.pointSum / semesterGrades.creditSum);
            }
            else
            {
                Sgpa.Text = "0";
            }

            StudentEnr.Text = student.enrNo;
            StudentName.Text = student.name;
            StudentSemester.Text = currentSemester.name;
        }

        protected string ResolvePath(string path)
        {
            try
            {
                return Server.MapPath(path);
            } catch (HttpException)
            {
                // Happens with concrete paths
                return path;
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            //SemesterData data = new SemesterData(SemesterSelect.SelectedIndex);
            //data.Sync();
            //Session["semester"] = data;
            Debug.Print("["+e.GetType()+"][Btn]New Value: " + SemesterSelect.SelectedValue);
            Debug.Print("[" + e.GetType() + "][Btn]New Value: " + Request.Form[SemesterSelect.UniqueID]);
        }
    }

}