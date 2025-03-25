using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NucleusProject
{
    public class Values
    {
        static readonly string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\jeffb\Desktop\dev\csharp\NucleusProject\NucleusProject\App_Data\NucleusDatabase.mdf;Integrated Security=True";

        private static readonly string baseDir = @"C:\Users\jeffb\Desktop\dev\csharp\NucleusProject\NucleusProject\";

        public static string ConnectionString { get {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"(LocalDB)\MSSQLLocalDB";
                builder.AttachDBFilename = baseDir+ "App_Data\\NucleusDatabase.mdf";
                builder.IntegratedSecurity= true;

                return builder.ToString();
            } }

    }
}