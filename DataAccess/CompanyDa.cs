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
    public class CompanyDa
    {
        public void get_companyData()
        {
            // base static data...
            GlobalData.CompanyData.ArabicBaseName = "ستيرس";
            GlobalData.CompanyData.EnglishBaseName = "Sters";




            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from Company_General_Settings";
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                    GlobalData.CompanyData.ArabicName = rdr["ArabicName"].ToString();
                    GlobalData.CompanyData.ArabicDiscription = rdr["ArabicDiscription"].ToString();
                    GlobalData.CompanyData.EnglishName = rdr["EnglishName"].ToString();
                    GlobalData.CompanyData.EnglishName2 = rdr["EnglishName2"].ToString();
                    GlobalData.CompanyData.EnglishDiscription = rdr["EnglishDiscription"].ToString();
                    GlobalData.CompanyData.EnglishDiscription2 = rdr["EnglishDiscription2"].ToString();
                    if (rdr["Logo1_EU"] != DBNull.Value)
                    { GlobalData.CompanyData.Logo1_EU = (byte[])rdr["Logo1_EU"]; }
                    else { GlobalData.CompanyData.Logo1_EU = null; }

                    if (rdr["Logo2_Sters"] != DBNull.Value)
                    {
                        GlobalData.CompanyData.Logo2_Sters = (byte[])rdr["Logo2_Sters"];
                    }
                    else
                    {
                        GlobalData.CompanyData.Logo2_Sters = null;
                    }

                    if (rdr["Logo2_Stars"] != DBNull.Value)
                    {
                        GlobalData.CompanyData.Logo2_Stars = (byte[])rdr["Logo2_Stars"];
                    }
                    else
                    {
                        GlobalData.CompanyData.Logo2_Stars = null;
                    }


                    GlobalData.CompanyData.Tel1 = rdr["Tel1"].ToString();
                    GlobalData.CompanyData.Tel2 = rdr["Tel2"].ToString();
                    GlobalData.CompanyData.Email = rdr["Email"].ToString();
                    GlobalData.CompanyData.Website = rdr["Website"].ToString();
                    GlobalData.CompanyData.Mail_Verification_from= rdr["Mail_Verification_from"].ToString();
                    GlobalData.CompanyData.Mail_Verification_server = rdr["Mail_Verification_server"].ToString();
                    //  GlobalData.CompanyData.Mail_Verification_password = Helpers.StringCipher.Decrypt(rdr["Mail_Verification_password"].ToString(), "KnockerzT");
                    GlobalData.CompanyData.EncryptedMail_Verification_password = rdr["Mail_Verification_password"].ToString();
                    if (rdr["Mail_Verification_port"] != DBNull.Value)
                    { GlobalData.CompanyData.Mail_Verification_port = (int)rdr["Mail_Verification_port"]; }
                    else
                    { GlobalData.CompanyData.Mail_Verification_port = null; }



                    if (rdr["Agreement_Points_Image"] != DBNull.Value)
                    { GlobalData.CompanyData.Agreement_Points_Image = (byte[])rdr["Agreement_Points_Image"]; }
                    else
                    { GlobalData.CompanyData.Agreement_Points_Image = null; }

                    if (rdr["Misc_MinDiff_Total_Vol_Weight"] != DBNull.Value)
                    {
                        GlobalData.CompanyData.weight_difference = (double)rdr["Misc_MinDiff_Total_Vol_Weight"];
                    }
                    else
                    { GlobalData.CompanyData.weight_difference = null; }
                       
                }

                rdr.Close();
            }
        
        }
    }
}
