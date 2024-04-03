using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace StersTransport.DataAccess
{
    public  class LabelsDa
    {
        public DataTable Get_Labels()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from Labels";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public bool update_labels(DataTable dtsource,out string errormessage)
        {
            bool comitted = false;
            errormessage = string.Empty;
            try
            {
                var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    using (SqlTransaction ts = conn.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmddelete = new SqlCommand();
                            cmddelete.Connection = conn;
                            cmddelete.Transaction = ts;
                            cmddelete.CommandText = "delete from Labels";
                            cmddelete.ExecuteNonQuery();

                            SqlCommand cmdinsert = new SqlCommand();
                            cmdinsert.Connection = conn;
                            cmdinsert.Transaction = ts;
                            foreach (DataRow dr in dtsource.Rows)
                            {
                                string Keyword = dr["Keyword"].ToString();
                                string Arabic = dr["Arabic"].ToString();
                                string Kurdish = dr["Kurdish"].ToString();

                                cmdinsert.CommandText = "Insert Into Labels (Keyword,Arabic,Kurdish) Values ('" + Keyword
                                    + "',N'" + Arabic + "',N'" + Kurdish + "')";
                                cmdinsert.ExecuteNonQuery();
                            }
   

                            ts.Commit();
                            comitted = true;

                        }
                        catch (Exception tsex)
                        {
                            errormessage = tsex.Message;
                            ts.Rollback();
                        }
                    }
                   
                    
                }
            }
            catch (Exception ex)
            { errormessage = ex.Message; }
            
            return comitted;
        }

    }
}
