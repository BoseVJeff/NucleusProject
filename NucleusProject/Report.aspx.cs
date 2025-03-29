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
            // TODO: Enable this once actual report generation and download is figured out
            //if (Session["id"] == null)
            //{
                //Response.Redirect("~/");
            //}
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

        protected void DownloadFile()
        {

            // Adapted from https://stackoverflow.com/a/21417259
            string file = ResolvePath(this.path);
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(file);
            Response.ContentType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "attatchment;filename=report.pdf");
            Response.AppendHeader("Content-Length",fileInfo.Length.ToString());
            Response.TransmitFile(file);
            // Probably unecessary, keeping it anyway
            Response.Flush();
            Response.End();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            DownloadFile();
        }
    }
}