using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StersTransport.Models;

namespace StersTransport.DataAccess
{
   public class CountryDa
    {
        public Country GetCountry(Int64 wId)
        {
            Country country = new Country();
            using (StersDB stersDB = new StersDB())
            {
                country = stersDB.Country.Where(a => a.Id == wId).FirstOrDefault();
            }
            return country;
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = new List<Country>();
            using (StersDB stersDB = new StersDB())
            {
                countries = stersDB.Country.ToList();
            }
            return countries;
        }
        public List<Country> GetCountries(bool haveAgents)
        {
            List<Country> countries = new List<Country>();
            using (StersDB stersDB = new StersDB())
            {
                countries = stersDB.Country.Where(x=>x.WeHaveAgentsThereIn==true).ToList();
            }
            return countries;
        }

        public void Addcountry(Country country, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic

                using (StersDB stersDB = new StersDB())
                {
                    Country newcountry = new Country
                    {
                        Id = country.Id,
                        CountryName = country.CountryName,
                        Alpha_2_Code = country.Alpha_2_Code,
                        Zip_Code_Digit_1 = country.Zip_Code_Digit_1,
                        Zip_Code_Digit_2 = country.Zip_Code_Digit_2,
                        Zip_Code_TXT = country.Zip_Code_TXT,
                        CurrencyId = country.CurrencyId,
                        CurrencyAgainst1IraqDinar = country.CurrencyAgainst1IraqDinar,
                        ImgForBoxLabel = country.ImgForBoxLabel,
                        ImgForPostLabel = country.ImgForPostLabel,
                        WeHaveAgentsThereIn = country.WeHaveAgentsThereIn,
                        MaximumWeighBox = country.MaximumWeighBox,
                        CheckMaximumWeighBox = country.CheckMaximumWeighBox
                        ,
                        continent = country.continent
                        ,
                        CountryNameAR = country.CountryNameAR,
                        CountryNameKU = country.CountryNameKU


                    };
                    stersDB.Country.Add(newcountry);
                    stersDB.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }




        }
        public void UpdateCountry(Country W_country, out string errormessage)
        {
            errormessage = string.Empty;

            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    var Original_country = stersDB.Country.Where(a => a.Id == W_country.Id).FirstOrDefault();
                    if (Original_country != null)
                    {
                        stersDB.Entry(Original_country).CurrentValues.SetValues(W_country);
                        stersDB.SaveChanges();
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }


           
        }

        public void DeleteCountry(Country W_country, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic


                if (W_country != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.Country.Find(W_country.Id);
                        if (x != null)
                        {
                            stersDB.Country.Remove(x);
                            stersDB.SaveChanges();
                        }
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.InnerException.InnerException.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }

        }


        public DataTable GetCountriesView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select tbl_Country.Id,tbl_Country.CountryName,tbl_Country.Alpha_2_Code,tbl_Country.Zip_Code_Digit_1,tbl_Country.Zip_Code_Digit_2,tbl_Country.Zip_Code_TXT,tbl_Currency.Name
from [dbo].[tbl_Country] left join tbl_Currency on tbl_Country.CurrencyId=tbl_Currency.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }



        public List<ClientCode> getcodeEntries(Country W_country)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes = stersDB.ClientCode.Where(x => x.CountryAgentId == W_country.Id||x.CountryPostId==W_country.Id).ToList();
            }
            return codes;
        }
        public List<City> getcitiesentries(Country W_country)
        {
            List<City> cities = new List<City>();
            using (StersDB stersDB = new StersDB())
            {
                cities = stersDB.City.Where(x => x.CountryId == W_country.Id).ToList();
            }
            return cities;
        }



    }
}
