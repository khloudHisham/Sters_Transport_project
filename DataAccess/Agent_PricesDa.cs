using StersTransport.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.DataAccess
{
    public class Agent_PricesDa
    {
        public List<Agent_Prices> GetAgent_Prices(long agentID)
        {
            List<Agent_Prices> agent_Prices = new List<Agent_Prices>();
            using (StersDB stersDB = new StersDB())
            {
                agent_Prices  = stersDB.Agent_Prices.Where(x=>x.Agent_Id==agentID).ToList();
            }
            return agent_Prices;
        }

        public List<Agent_Prices> GetAgent_PricesBy_For_DestinationAgent(long DestagentID)
        {
            List<Agent_Prices> agent_Prices = new List<Agent_Prices>();
            using (StersDB stersDB = new StersDB())
            {
                agent_Prices = stersDB.Agent_Prices.Where(x => x.Agent_Id_Destination == DestagentID).ToList();
            }
            return agent_Prices;
        }


        public void Update_AgentPrices(List<Agent_Prices>w_newAgentPrices,long W_agentID, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                var original_agent_Prices = GetAgent_Prices(W_agentID);
                using (StersDB stersDB = new StersDB())
                {
                    // Update Existing ....
                    #region Update

                    var ToBeUpdatedAgentPrices = original_agent_Prices.Where(o => w_newAgentPrices.Any(n => o.ID == n.ID));
                    foreach (var ToBeUpdatedAgentPricesItem in ToBeUpdatedAgentPrices)
                    {
                        var newPricesAgentItem = w_newAgentPrices.Where(n => n.ID == ToBeUpdatedAgentPricesItem.ID).FirstOrDefault();
                        var original_PricesAgent_Entry = stersDB.Agent_Prices.Where(t => t.ID == ToBeUpdatedAgentPricesItem.ID).FirstOrDefault();
                        if (original_PricesAgent_Entry != null)
                        {
                            stersDB.Entry(original_PricesAgent_Entry).CurrentValues.SetValues(newPricesAgentItem);
                        }
                    }
                    #endregion

                    // Insert New 
                    #region Insert
                    var ToBeInsertedAgentPrices = w_newAgentPrices.Where(n => n.ID == 0);
                    foreach (var tobeInserted_AgentPricesItem in ToBeInsertedAgentPrices)
                    {
                        Agent_Prices agent_Prices = new Agent_Prices()
                        {
                            Agent_Id = tobeInserted_AgentPricesItem.Agent_Id,
                            Agent_Id_Destination = tobeInserted_AgentPricesItem.Agent_Id_Destination,
                            Price1to5_7KG = tobeInserted_AgentPricesItem.Price1to5_7KG,
                            PriceKG = tobeInserted_AgentPricesItem.PriceKG,
                            PriceDoorToDoor = tobeInserted_AgentPricesItem.PriceDoorToDoor,
                            BoxPackaging = tobeInserted_AgentPricesItem.BoxPackaging,
                            CommissionBox = tobeInserted_AgentPricesItem.CommissionBox,
                            CommissionKG = tobeInserted_AgentPricesItem.CommissionKG,
                            CurrencyEQ = tobeInserted_AgentPricesItem.CurrencyEQ
                        };
                        stersDB.Agent_Prices.Add(agent_Prices);
                    }
                    #endregion


                    // delete 
                    #region Delete
                    var ToBeDeletedAgentPrices = original_agent_Prices.Where(o => !w_newAgentPrices.Any(n => o.ID == n.ID));
                    foreach (var tobeDeleted_agentPricesItem in ToBeDeletedAgentPrices)
                    {
                        var item = stersDB.Agent_Prices.Find(tobeDeleted_agentPricesItem.ID);
                        stersDB.Agent_Prices.Remove(item);
                    }
                    #endregion

                    stersDB.SaveChanges();

                }
            }
            catch (DbUpdateException ex)
            {
                errormessage = string.Format("{0} : {1}", "Inner Exception", ex.Message);
            }
            catch (Exception ex)
            {
                errormessage = string.Format("{0} : {1}", "General Exception", ex.Message);
            }


            
        }
    }
}
