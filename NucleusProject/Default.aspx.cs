using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NucleusProject
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["id"]!=null)
            {
                Response.Redirect("~/Attendance");
                ((HyperLink)Master.FindControl("LogoutLink")).Visible = true;
            } else
            {
                // Hide the logout button
                ((HyperLink)Master.FindControl("LogoutLink")).Visible = false;
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
                // TODO: Reqdirect from url param
                Response.Redirect("~/Attendance");
            }
        }
    }
}