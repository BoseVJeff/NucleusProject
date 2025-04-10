using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace NucleusProject
{
    public enum ViewSpan
    {
        Day,
        Week,
        Month,
        Custom
    }
    public class Values
    {
        private static readonly string baseDir = @"~\";

        public static string ConnectionString { get {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"(LocalDB)\MSSQLLocalDB";
                builder.AttachDBFilename = HostingEnvironment.MapPath(Path.Combine(baseDir, @"App_Data\NucleusDatabase.mdf"));
                builder.IntegratedSecurity= true;

                return builder.ToString();
            } }

    }
}