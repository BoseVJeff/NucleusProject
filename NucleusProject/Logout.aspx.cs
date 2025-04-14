using System;
using System.Web;

namespace NucleusProject
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Unset session variable
            Session["id"] = null;
            // Delete cookie
            Response.Cookies["id"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Remove("id");
            // Replace it with a dummy cookie
            HttpCookie cookie = new HttpCookie("id");
            cookie.Value = "";
            cookie.Expires = DateTime.Now.AddMonths(-1);
            Response.Cookies.Add(cookie);
            // Redirect user to Login page
            Response.Redirect("~/");
            // End session
            Session.Clear();
            Session.Abandon();
        }
    }
}