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
            if (Session["id"] == null)
            {
                Response.Redirect("~/");
            }
        }

        protected void downloadFile()
        {
            // Taken from https://stackoverflow.com/a/21417259
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attatchment;filename=report.pdf");
            Response.TransmitFile(this.path);
            Response.End();
        }
    }
}