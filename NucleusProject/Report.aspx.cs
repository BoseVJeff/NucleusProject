using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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

            Student student = (Student)Session["student"];
            // TODO: Populate from dropdown
            SemesterData currentSemester = new SemesterData(1);
            currentSemester.Sync();

            ((Label)Master.FindControl("DisplayName")).Text = student.enrNo;

            //SemesterGrades semesterGrades = new SemesterGrades((int)studentId, SemesterData.GetSemesterDataForDateTimeOffset(DateTimeOffset.Now).id);
            SemesterGrades semesterGrades = new SemesterGrades(1, (int)studentId);
            semesterGrades.Sync();
            GV_Result.DataSource = semesterGrades.dataSet;
            GV_Result.DataBind();
            if (GV_Result.Rows.Count > 0)
            {
                GV_Result.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            RegCredits.Text = semesterGrades.regCreditSum.ToString();
            Credits.Text = semesterGrades.creditSum.ToString();
            Points.Text = semesterGrades.pointSum.ToString();
            Sgpa.Text = String.Format("{0:0.00}", semesterGrades.pointSum / semesterGrades.creditSum);

            StudentEnr.Text = student.enrNo;
            StudentName.Text = student.name;
            StudentSemester.Text = currentSemester.name;

            Grades grades = new Grades();
            grades.Sync();
            GradeExplanation.DataSource = grades.dataSet;
            GradeExplanation.DataBind();
            if (GradeExplanation.HeaderRow != null)
            {
                GradeExplanation.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

            if (!IsPostBack)
            {
                // Register ItemDataBound event
                CourseRepeater.ItemDataBound += CourseRepeater_ItemDataBound;

                // Load courses and bind to CourseRepeater
                List<Course> courses = LoadCourses();
                if (courses != null && courses.Count > 0)
                {
                    CourseRepeater.DataSource = courses;
                    CourseRepeater.DataBind();
                }
                else
                {
                    Console.WriteLine("No courses available to bind.");
                }
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

        protected string ResolvePath(string path)
        {
            try
            {
                return Server.MapPath(path);
            }
            catch (HttpException)
            {
                // Happens with concrete paths
                return path;
            }
        }

        protected void DownloadFile()
        {

            // Adapted from https://stackoverflow.com/a/21417259
            string file = ResolvePath(this.path);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attatchment;filename=report.pdf");
            Response.AppendHeader("Content-Length", fileInfo.Length.ToString());
            Response.TransmitFile(file);
            // Probably unecessary, keeping it anyway
            Response.Flush();
            Response.End();
        }

    }

}