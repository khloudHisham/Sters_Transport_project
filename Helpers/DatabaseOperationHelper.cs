using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Helpers
{
    public class DatabaseOperationHelper
    {
        public long getMaxLongValueFromTable(string tablename,string columnname)
        {
            long max = 0;
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Select ISNULL(Max(" + columnname + "),0) as MaxVal from " + tablename + "";
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    max=(long)rdr["MaxVal"];
                    rdr.Close();
                }
                else
                {
                    rdr.Close();
                }
            }
           
            return max;
        }
    }
}
