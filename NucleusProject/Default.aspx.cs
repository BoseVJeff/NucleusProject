using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NucleusProject;

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
            Session["id"] = Convert.ToInt32(Username.Text);
            Response.Redirect("~/Attendance");
        }
    }
}