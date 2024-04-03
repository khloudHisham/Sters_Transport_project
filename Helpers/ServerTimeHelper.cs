using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Helpers
{
    public static class ServerTimeHelper
    {
        public static DateTime? get_server_Time()
        {
            DateTime? dt = null;
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select getdate()";
                SqlDataReader rdr_00 = cmd.ExecuteReader();
                if (rdr_00.HasRows)
                {
                    rdr_00.Read();
                    dt = (DateTime)rdr_00[0];
                    rdr_00.Close();
                }
                else
                { rdr_00.Close(); }
            }

            return dt;
        }
    }
}
