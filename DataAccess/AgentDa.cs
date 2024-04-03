using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StersTransport.Models;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace StersTransport.DataAccess
{
    public class AgentDa
    {
       public Agent GetAgent(Int64 wId)
        {
            Agent agent = new Agent();
            using (StersDB stersDB = new StersDB())
            {
                agent = stersDB.Agent.Where(a => a.Id == wId).FirstOrDefault();
            }
            return agent;
        }

        public List<Agent> GetAgents()
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.ToList();
            }
            return agents;
        }

        public List<Agent> GetAgentsWithCountries(long countryid_)
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.Include(x => x.Country).Where(xz=>xz.CountryId==countryid_).OrderBy(xx=>xx.Country.CountryName).ToList();
            }
            return agents;
        }

        public List<Agent> GetAgents(Int64 w_country)
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.Where(x=>x.CountryId==w_country).ToList();
            }
            return agents;
        }

        /*
        public List<Agent> GetAgentsWithPrices(Int64 w_country)
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.Where(x => x.CountryId == w_country).Include(xx=>xx.Agent_Prices1).ToList();
            }
            return agents;
        }
        */

        public List<Agent> GetAgents_ExludedCountry(Int64 wExcluded_country)
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.Where(x => x.CountryId != wExcluded_country).ToList();
            }
            return agents;
        }
        public List<Agent> GetAgents_ExludedAgent(Int64 wExcluded_Agent)
        {
            List<Agent> agents = new List<Agent>();
            using (StersDB stersDB = new StersDB())
            {
                agents = stersDB.Agent.Where(x => x.Id != wExcluded_Agent).ToList();
            }
            return agents;
        }




        public DataTable GetAgentsView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select tbl_agent.Id,tbl_agent.AgentName,tbl_Country.CountryName,tbl_agent.ContactPersonName,tbl_agent.CompanyName,tbl_agent.PhoneNo1,tbl_agent.PhoneNo2,tbl_Agent.Address,tbl_Agent.ZipCode,tbl_Agent.[E-mail],tbl_Agent.Web,tbl_Currency.Name as CurrencyName from tbl_agent left join tbl_Country on tbl_Agent.CountryId=tbl_Country.Id
left join tbl_Currency on tbl_Agent.CurrencyId=tbl_Currency.Id";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public void Addagent(Agent agent, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    Agent newAgent = new Agent()
                    {
                        Id = agent.Id,
                        AgentName = agent.AgentName,
                        CountryId = agent.CountryId,
                        ContactPersonName = agent.ContactPersonName,
                        CompanyName = agent.CompanyName,
                        PhoneNo1 = agent.PhoneNo1,
                        PhoneNo2 = agent.PhoneNo2,
                        Address = agent.Address,
                        CityId = agent.CityId,
                        CurrencyId = agent.CurrencyId,
                        HavePostService = agent.HavePostService,
                        AddressAR = agent.AddressAR,
                        AddressKu = agent.AddressKu,
                        CharactersPrefix = agent.CharactersPrefix,
                        YearPrefix = agent.YearPrefix,
                        NumberOfDigits = agent.NumberOfDigits,
                        InvoiceLanguage = agent.InvoiceLanguage,
                        IsLocalCompanyBranch = agent.IsLocalCompanyBranch,
                        PhonesDisplayString = agent.PhonesDisplayString
                        ,AgentIsDisabled=agent.AgentIsDisabled
                    };

                    stersDB.Agent.Add(newAgent);
                    //stersDB.Set<Agent>().Add(agent);
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

        public void UpdateAgent(Agent W_agent,out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                using (StersDB stersDB = new StersDB())
                {
                    var Original_agent = stersDB.Agent.Where(a => a.Id == W_agent.Id).FirstOrDefault();
                    if (Original_agent != null)
                    {
                        stersDB.Entry(Original_agent).CurrentValues.SetValues(W_agent);
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
        public void DeleteAgent(Agent W_agent, out string errormessage)
        {
            errormessage = string.Empty;

            try
            {
                if (W_agent != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.Agent.Find(W_agent.Id);
                        using (DbContextTransaction transaction = stersDB.Database.BeginTransaction())
                        {
                            try
                            {
                                if (x != null)
                                {
                                    // remove prices...
                                    // get list of prices 
                                    List<Agent_Prices> prices=  stersDB.Agent_Prices.Where(xx => xx.Agent_Id_Destination == W_agent.Id).ToList();
                                    for (int c = 0; c < prices.Count; c++)
                                    {
                                        var priceid = prices[c].ID;
                                        var AgentPrice=  stersDB.Agent_Prices.Find(priceid);
                                        if (AgentPrice != null)
                                        {
                                            stersDB.Agent_Prices.Remove(AgentPrice);
                                        }
                                    }
                                    stersDB.SaveChanges();


                                    // remove agent
                                    stersDB.Agent.Remove(x);
                                    stersDB.SaveChanges();
                                }
                                transaction.Commit();
                            }
                            catch (Exception tsex)
                            {
                                transaction.Rollback();
                                errormessage = string.Format("{0} : {1}", "Transaction Error", tsex.Message);
                            }
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


        public List<ClientCode> getcodeEntries_as_sender(Agent W_agent)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes=stersDB.ClientCode.Where(x => x.BranchId == W_agent.Id).ToList();
            }
            return codes;
        }
        public List<ClientCode> getcodeEntries_as_receiver(Agent W_agent)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes = stersDB.ClientCode.Where(x => x.AgentId == W_agent.Id).ToList();
            }
            return codes;
        }
        public List<ClientCode> getcodeEntries_as_sender_or_receiver(Agent W_agent)
        {
            List<ClientCode> codes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                codes = stersDB.ClientCode.Where(x => x.AgentId == W_agent.Id || x.BranchId == W_agent.Id).ToList();
            }
            return codes;
        }


        public bool isNewNumberOfDigitsValid(Agent w_agent)
        {
            long? newNUmber = w_agent.NumberOfDigits;
            Agent originalagent = GetAgent(w_agent.Id);
            long? originalNUmber = originalagent.NumberOfDigits;

            bool isvalid = false;

            if (newNUmber < originalNUmber)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }
            return isvalid;
        }

    }
}
