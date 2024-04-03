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
   public class CityDa
    {
        public City GetCity(Int64 wId)
        {
            City city = new City();
            using (StersDB stersDB = new StersDB())
            {
                city = stersDB.City.Where(a => a.Id == wId).FirstOrDefault();
            }
            return city;
        }

        public List<City> GetCities()
        {
            List<City> cities = new List<City>();
            using (StersDB stersDB = new StersDB())
            {
                cities = stersDB.City.ToList();
            }
            return cities;
        }

        public List<City> GetCities(long w_countryID)
        {
            List<City> cities = new List<City>();
            using (StersDB stersDB = new StersDB())
            {
                cities = stersDB.City.Where(x => x.CountryId == w_countryID).ToList();
            }
            return cities;
        }


        public List<string> GetCitiesNames()
        {
            List<string> cities = new List<string>();
            using (StersDB stersDB = new StersDB())
            {
                foreach (var item in stersDB.City)
                {
                    cities.Add(item.CityName);
                }
            }
            return cities;
        }


        public void Addcity(City city, out string errormessage)
        {

            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    City newCity = new City()
                    {
                        Id = city.Id,
                        CityName = city.CityName,
                        CountryId = city.CountryId,
                        WeHaveAgentsThereIn = city.WeHaveAgentsThereIn
                    };
                    stersDB.City.Add(newCity);
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


        public void Updatecity(City W_city, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    var Original_city = stersDB.City.Where(a => a.Id == W_city.Id).FirstOrDefault();
                    if (Original_city != null)
                    {
                        stersDB.Entry(Original_city).CurrentValues.SetValues(W_city);
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

        public void Deletecity(City W_city, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                if (W_city != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.City.Find(W_city.Id);
                        if (x != null)
                        {
                            stersDB.City.Remove(x);
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


        public DataTable GetcitiesView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select tbl_City.Id,tbl_City.CityName,tbl_Country.CountryName from tbl_City inner join tbl_Country
on tbl_City.CountryId=tbl_Country.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }






    }
}
