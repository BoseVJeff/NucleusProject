using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NucleusProject
{
    abstract public class DbObject
    {
        abstract public void Sync(string connectionString);
    }

    public class Student : DbObject
    {
        public Student()
        {
            this.dataSet = new DataSet();
        }
        public DataSet dataSet;
        public override void Sync(string connectionString=null)
        {
            string connStr = connectionString;
            if (connStr == null)
            {
                connStr = Values.ConnectionString;
            }
            const string cmd = "SELECT * FROM Mst_Student";
            SqlConnection connection = new SqlConnection(connStr);
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmd, connection);
                sqlCommand.CommandText = cmd;
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                adapter.Fill(dataSet);
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
            return;
        }
    }
}