using System;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Reset styling for other items
            const string otherItemsCss = "nav-link";
            ((HyperLink)Master.FindControl("AttendanceLink")).CssClass = otherItemsCss;
            ((HyperLink)Master.FindControl("ScheduleLink")).CssClass = otherItemsCss;
            ((HyperLink)Master.FindControl("ReportLink")).CssClass = otherItemsCss;

            int? studentId = Values.StudentId(Session, Request.Cookies);

            if (studentId!=null)
            {
                Session["studentId"] = (int)studentId;
                Response.Redirect("~/Attendance");
                //((HyperLink)Master.FindControl("LogoutLink")).Visible = true;
                // Hide the user display section
                ((HtmlGenericControl)Master.FindControl("userdisplay")).Visible = true;
            } else
            {
                // User is logged out
                // Hide the logout button
                //((HyperLink)Master.FindControl("LogoutLink")).Visible = false;
                // Hide the user display section
                ((HtmlGenericControl)Master.FindControl("userdisplay")).Visible = false;
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            string username=Username.Text;
            string password=Password.Text;
            Student student=new Student(username,password);
            student.Sync();
            if(student.id==null)
            {
                Incorrect.Visible = true;
            } else
            {
                Session["student"] = student;
                Session["id"] = student.id;
                if (remember.Checked) {
                    HttpCookie cookie = new HttpCookie("id");
                    cookie.Value = student.id.ToString();
                    cookie.Expires = DateTime.Now.AddMonths(4);
                    Response.Cookies.Add(cookie);
                }
                Debug.WriteLine("[" + DateTime.Now + "][Enrollment No.] " + student.enrNo);
                ((Label)Master.FindControl("DisplayName")).Text = student.enrNo;
                Debug.WriteLine("[" + DateTime.Now + "][Label] " + ((Label)Master.FindControl("DisplayName")).Text);
                // TODO: Redirect from url param
                string toUrl = Request.QueryString["to"];
                if (toUrl != null)
                {
                    Response.Redirect("~/" + toUrl);
                }
                else
                {
                    Response.Redirect("~/Attendance");
                }
            }
        }
    }
}