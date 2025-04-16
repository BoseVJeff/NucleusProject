using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Data;
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
            // Highlight the report link in the navbar
            const string currentItemCss = "nav-link disabled text-white";
            const string otherItemsCss = "nav-link";
            ((HyperLink)Master.FindControl("ReportLink")).CssClass = currentItemCss;
            // Reset styling for other items
            ((HyperLink)Master.FindControl("ScheduleLink")).CssClass = otherItemsCss;
            ((HyperLink)Master.FindControl("AttendanceLink")).CssClass = otherItemsCss;

            int? studentId = Values.StudentId(Session, Request.Cookies);
            if (studentId == null)
            {
                Response.Redirect("~/?to=Report");
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
                }
                else
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
            //SemesterData selectedSemester = new SemesterData(Convert.ToInt32(Request.Form[SemesterSelect.UniqueID]));
            SemesterData selectedSemester = new SemesterData(Convert.ToInt32(SemesterSelect.SelectedValue));
            selectedSemester.Sync();

            PopulateResults(student, selectedSemester);

            CourseRepeater.ItemDataBound += CourseRepeater_ItemDataBound;
            List<Course> courses=LoadCourses(student,selectedSemester);
            CourseRepeater.DataSource = courses;
            CourseRepeater.DataBind();

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
            }
            else
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

            OverallGrades overallGrades = new OverallGrades((int)student.id);
            overallGrades.Sync();

            CumRegCredits.Text = overallGrades.regCreditSum.ToString();
            CumCredits.Text = overallGrades.creditSum.ToString();
            CumPoints.Text = overallGrades.pointSum.ToString();
            if (overallGrades.creditSum != 0)
            {
                Cgpa.Text = String.Format("{0:0.00}", overallGrades.pointSum / overallGrades.creditSum);
            }
        }

        private List<Course> LoadCourses(Student student, SemesterData semester)
        {
            InternalGrades internalGrades = new InternalGrades((int)student.id, semester.id);
            internalGrades.Sync();

            List<Course> courses = new List<Course>();

            // Starting from 1, as `0` is the unfiltered table
            for (int i = 1; i < internalGrades.subjects.Count; i++)
            {
                Course course = new Course {
                    CourseCode= (string)internalGrades.dataSet.Tables[i].Rows[0]["Code"],
                    CourseTitle= (string)internalGrades.dataSet.Tables[i].Rows[0]["Name"],
                    Exams=new List<Exam>()
                };
                foreach (DataRow eval in internalGrades.dataSet.Tables[i].Rows)
                {
                    Exam exam = new Exam();
                    exam.ExamName = (string)eval["Title"];
                    Decimal marks = (Decimal)eval["Marks"];
                    exam.ObtainedMarks = String.Format("{0:0.0}", marks);
                    int total = (int)eval["Max"];
                    exam.TotalMarks=String.Format("{0:0.0}",total);
                    DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((int)eval["Timestamp"]);
                    // TODO: Use client timezone
                    dateTimeOffset=dateTimeOffset.ToLocalTime();
                    exam.PublishedDate = dateTimeOffset.ToString("dd-MM-yyyy hh:mm:ss tt");
                    exam.PublishedBy = (string)eval["Faculty"];

                    decimal percentage = (marks * 100) / total;
                    exam.Percentage = String.Format("{0:0.0}", percentage);

                    course.Exams.Add(exam);
                }
                courses.Add(course);
            }

            return courses;
        }

        protected void CourseRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Course course = (Course)e.Item.DataItem;
                Repeater examRepeater = (Repeater)e.Item.FindControl("ExamRepeater");

                if (examRepeater != null)
                {
                    // Bind the nested ExamRepeater
                    examRepeater.DataSource = course.Exams;
                    examRepeater.DataBind();
                }
            }
        }

        public class Course
        {
            public string CourseCode { get; set; }
            public string CourseTitle { get; set; }
            public List<Exam> Exams { get; set; }
        }

        public class Exam
        {
            public string ExamName { get; set; }
            public string ObtainedMarks { get; set; }
            public string TotalMarks { get; set; }
            public string Percentage { get; set; }
            public string PublishedDate { get; set; }
            public string PublishedBy { get; set; }
        }
    }

}