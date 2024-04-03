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
  public  class CurrencyDa
    {
        public Currency GetCurrency(Int64 wId)
        {
            Currency currency = new Currency();
            using (StersDB stersDB = new StersDB())
            {
                currency = stersDB.Currency.Where(a => a.Id == wId).FirstOrDefault();
            }
            return currency;
        }

        public List<Currency> GetCurrencies()
        {
            List<Currency> currencies = new List<Currency>();
            using (StersDB stersDB = new StersDB())
            {
                currencies = stersDB.Currency.ToList();
            }
            return currencies;
        }

        public void Addcurrency(Currency currency, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    Currency newCurrency = new Currency()
                    {
                        Id = currency.Id,
                        Name = currency.Name
                    ,
                        NameAR = currency.NameAR,
                        NameKU = currency.NameKU
                    };
                    stersDB.Currency.Add(newCurrency);
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

        public void UpdateCurrency(Currency W_currency, out string errormessage)
        {

            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    var Original_currency = stersDB.Currency.Where(a => a.Id == W_currency.Id).FirstOrDefault();
                    if (Original_currency != null)
                    {
                        stersDB.Entry(Original_currency).CurrentValues.SetValues(W_currency);
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


        public void DeleteCurrency(Currency W_currency, out string errormessage)
        {

            errormessage = string.Empty;
            try
            {
                //db logic
                if (W_currency != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.Currency.Find(W_currency.Id);
                        if (x != null)
                        {
                            stersDB.Currency.Remove(x);
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


        public DataTable GetCurrenciesView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select * from tbl_Currency";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }



        public List<ClientCode> getcodeEntries(Currency W_currency)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes = stersDB.ClientCode.Where(x => x.Currency_Type == W_currency.Name).ToList(); // name?
            }
            return codes;
        }


    }
}
