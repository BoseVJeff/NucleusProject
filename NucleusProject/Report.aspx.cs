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
            SemesterData selectedSemester = new SemesterData(Convert.ToInt32(Request.Form[SemesterSelect.UniqueID]));
            selectedSemester.Sync();

            PopulateResults(student, selectedSemester);

            CourseRepeater.ItemDataBound += CourseRepeater_ItemDataBound;
            List<Course> courses=LoadCourses();
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

        private List<Course> LoadCourses()
        {
            return new List<Course>
            {
                new Course
                {
                    CourseCode = "CMP305",
                    CourseTitle = "Fundamentals of Cloud Computing",
                    Exams = new List<Exam>
                    {
                        new Exam
                        {
                            ExamName = "Mid-Sem Continuous Evaluation",
                            ObtainedMarks = 8,
                            TotalMarks = 10,
                            Percentage = (8*100)/10,
                            PublishedDate = "07-11-2024 16:12:08",
                            PublishedBy = "Rupali Shinde"
                        },
                        new Exam
                        {
                            ExamName = "Assignment Continuous Evaluation",
                            ObtainedMarks = 13,
                            TotalMarks = 15,
                            Percentage = (13*100)/15,
                            PublishedDate = "07-11-2024 16:09:45",
                            PublishedBy = "Rupali Shinde"
                        },
                        new Exam
                        {
                            ExamName = "Viva Continuous Evaluation",
                            ObtainedMarks = 8,
                            TotalMarks = 10,
                            Percentage = (8*100)/10,
                            PublishedDate = "11-11-2024 14:26:52",
                            PublishedBy = "Rupali Shinde"
                        },
                        new Exam
                        {
                            ExamName = "Presentation Continuous Evaluation",
                            ObtainedMarks = 8,
                            TotalMarks = 10,
                            Percentage = (8*100)/10,
                            PublishedDate = "11-11-2024 14:26:16",
                            PublishedBy = "Rupali Shinde"
                        },
                        new Exam
                        {
                            ExamName = "AWS Certificate Continuous Evaluation",
                            ObtainedMarks = 15,
                            TotalMarks = 15,
                            Percentage = (15*100)/15,
                            PublishedDate = "07-11-2024 16:17:57",
                            PublishedBy = "Rupali Shinde"
                        }
                    }
                },

                new Course
                {
                    CourseCode = "CA210",
                    CourseTitle = "Software Engineering",
                    Exams = new List<Exam>
                    {
                        new Exam
                        {
                            ExamName = "Quiz Continuous Evaluation",
                            ObtainedMarks = 10,
                            TotalMarks = 15,
                            Percentage = (10*100)/15,
                            PublishedDate = "29-10-2024 10:04:38",
                            PublishedBy = "Shraddha Doshi"
                        },
                        new Exam
                        {
                            ExamName = "Viva Continuous Evaluation",
                            ObtainedMarks = 27,
                            TotalMarks = 30,
                            Percentage = (27*100)/30,
                            PublishedDate = "14-11-2024 12:01:26",
                            PublishedBy = "Shraddha Doshi"
                        },
                        new Exam
                        {
                            ExamName = "Class Participation Continuous Evaluation",
                            ObtainedMarks = 13,
                            TotalMarks = 15,
                            Percentage = (13*100)/15,
                            PublishedDate = "09-11-2024 13:45:37",
                            PublishedBy = "Shraddha Doshi"
                        }
                    }
                },

                new Course
                {
                    CourseCode = "CMP304",
                    CourseTitle = "Introduction to Web Designing and PHP",
                    Exams = new List<Exam>
                    {
                        new Exam
                        {
                            ExamName = "Assignment Continuous Evaluation",
                            ObtainedMarks = 14,
                            TotalMarks = 20,
                            Percentage = (14*100)/20,
                            PublishedDate = "14-11-2024 09:15:21",
                            PublishedBy = "Darshana Makani"
                        },
                        new Exam
                        {
                            ExamName = "CE/CP Continuous Evaluation",
                            ObtainedMarks = 3,
                            TotalMarks = 5,
                            Percentage = (3*100)/5,
                            PublishedDate = "14-11-2024 09:13:48",
                            PublishedBy = "Darshana Makani"
                        },
                        new Exam
                        {
                            ExamName = "Quiz Continuous Evaluation",
                            ObtainedMarks = 10,
                            TotalMarks = 15,
                            Percentage = (10*100)/15,
                            PublishedDate = "13-11-2024 17:03:32",
                            PublishedBy = "Darshana Makani"
                        },
                        new Exam
                        {
                            ExamName = "Viva Continuous Evaluation",
                            ObtainedMarks = 17,
                            TotalMarks = 20,
                            Percentage = (17*100)/20,
                            PublishedDate = "14-11-2024 11:54:44",
                            PublishedBy = "Darshana Makani"
                        }
                    }
                },

                new Course
                {
                    CourseCode = "CMP308",
                    CourseTitle = "Operating Systems",
                    Exams = new List<Exam>
                    {
                        new Exam
                        {
                            ExamName = "Assignment Continuous Evaluation",
                            ObtainedMarks = 15,
                            TotalMarks = 20,
                            Percentage = (15*100)/20,
                            PublishedDate = "11-11-2024 21:10:37",
                            PublishedBy = "Parth Kinjal Shah"
                        },
                        new Exam
                        {
                            ExamName = "Quiz Continuous Evaluation",
                            ObtainedMarks = 20,
                            TotalMarks = 20,
                            Percentage = (20*100)/20,
                            PublishedDate = "12-11-2024 21:57:51",
                            PublishedBy = "Parth Kinjal Shah"
                        },
                        new Exam
                        {
                            ExamName = "Viva Continuous Evaluation",
                            ObtainedMarks = 8,
                            TotalMarks = 20,
                            Percentage = (8*100)/20,
                            PublishedDate = "13-11-2024 13:34:10",
                            PublishedBy = "Parth Kinjal Shah"
                        }

                    }
                }
            };
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
            public int ObtainedMarks { get; set; }
            public int TotalMarks { get; set; }
            public double Percentage { get; set; }
            public string PublishedDate { get; set; }
            public string PublishedBy { get; set; }
        }
    }

}