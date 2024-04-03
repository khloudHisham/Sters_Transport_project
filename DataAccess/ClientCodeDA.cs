using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StersTransport.Models;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Configuration;

namespace StersTransport.DataAccess
{
   public class ClientCodeDA
    {
       
        string BaseSenderReceiverSummaryQuery { get; set; }
        public ClientCodeDA()
        {
            BaseSenderReceiverSummaryQuery = "select Client_Code,Shipment_No,SenderName,Sender_Tel,SenderCompany,ReceiverName,ReceiverCompany,Receiver_Tel,Goods_Description,Goods_Value,Box_No,Pallet_No,Weight_Total,PostDate,Total_Post_Cost_IQD,TotalPaid_IQD,EuropaToPay,branches.AgentName as BranchName,tbl_Agent.AgentName from CODE_LIST inner join  tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id inner join tbl_Agent branches on CODE_LIST.BranchId=branches.Id";

        }

        public  ClientCode GetClientCode(string W_code)
        {
            ClientCode clientCode = new ClientCode();
            using (StersDB stersDB = new StersDB())
            {
                clientCode = stersDB.ClientCode.Where(c => c.Code == W_code).FirstOrDefault();
            }
            return clientCode;
        }
        public ClientCode GetClientCodeWithBranchAndUser(string W_code)
        {
            ClientCode clientCode = new ClientCode();
            using (StersDB stersDB = new StersDB())
            {
                clientCode = stersDB.ClientCode.Where(c => c.Code == W_code)
                    .Include(x => x.virtual_Branch)
                    .Include(x => x.User)
                    .Include(x=>x.IdentityType).
                    FirstOrDefault();
            }
            return clientCode;
        }

        

        public List<ClientCode> GetClientCodes()
        {
            List<ClientCode> clientCodes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                clientCodes = stersDB.ClientCode.ToList();
            }
            return clientCodes;
        }

        public DataTable GetClientCodesView1()
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = @"select CODE_LIST.Client_Code,CODE_LIST.Shipment_No,CODE_LIST.SenderName,CODE_LIST.Sender_Tel,CODE_LIST.ReceiverName,CODE_LIST.Receiver_Tel,
CODE_LIST.CountryAgentId,tbl_Country.CountryName,CODE_LIST.AgentId,tbl_Agent.AgentName,CODE_LIST.Person_in_charge_Id,tbl_User.UserName,
CODE_LIST.Goods_Description,CODE_LIST.Box_No,CODE_LIST.Pallet_No,CODE_LIST.Weight_Total,
CODE_LIST.PostDate,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.EuropaToPay,
CODE_LIST.BranchId,branches.AgentName as BranchName
from code_list left join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.Id
left join tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id
left join tbl_User on CODE_LIST.Person_in_charge_Id=tbl_User.Id
inner join tbl_Agent branches on CODE_LIST.BranchId=branches.Id order by AutoNumber Desc";

                // order by PostYear Desc, Num desc .... 
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public List<ClientCode> GetClientCodes(long w_branch,bool reverse_order)
        {
            List<ClientCode> clientCodes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                if (reverse_order)
                {
                    //   clientCodes = stersDB.ClientCode.Where(c => c.BranchId == w_branch).OrderByDescending(x=>x.PostYear).ThenByDescending(x=>x.Num).ToList();

                    clientCodes = stersDB.ClientCode.Where(c => c.BranchId == w_branch).OrderByDescending(x => x.AutoNumber).ToList();
                }
                else
                { clientCodes = stersDB.ClientCode.Where(c => c.BranchId == w_branch).ToList(); }
               
            }
            return clientCodes;
        }

        public List<ClientCode> GetBranchYearClientCodesGreaterThanNumber(string branchCode,string yearCode,long? Number)
        {
            List<ClientCode> clientCodes = new List<ClientCode>();
            using (StersDB stersDB = new StersDB())
            {
                clientCodes = stersDB.ClientCode.Where(c => c.BranchCode == branchCode && c.YearCode == yearCode && c.Num >= Number).ToList();
            }
            return clientCodes;
        }

        public void AddClientCode(ClientCode clientCode, out string errormessage)
        {

            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    ClientCode newcode = new ClientCode
                    {
                        Code = clientCode.Code,
                        BranchCode = clientCode.BranchCode,
                        YearCode = clientCode.YearCode,
                        Num = clientCode.Num,
                        Shipment_No = clientCode.Shipment_No,
                        SenderName = clientCode.SenderName,
                        Sender_ID = clientCode.Sender_ID,
                        Sender_ID_Type = clientCode.Sender_ID_Type,
                        Sender_Tel = clientCode.Sender_Tel,
                        ReceiverName = clientCode.ReceiverName,
                        Receiver_Tel = clientCode.Receiver_Tel,
                        Goods_Description = clientCode.Goods_Description,
                        Goods_Value = clientCode.Goods_Value,
                        Have_Insurance = clientCode.Have_Insurance,
                        Insurance_Percentage = clientCode.Insurance_Percentage,
                        Insurance_Amount = clientCode.Insurance_Amount,
                        Pallet_No = clientCode.Pallet_No,
                        Box_No = clientCode.Box_No,
                        Weight_Kg = clientCode.Weight_Kg,
                        Weight_Vol_Factor = clientCode.Weight_Vol_Factor,
                        AdditionalWeight = clientCode.AdditionalWeight,
                        Weight_Total = clientCode.Weight_Total,
                        Admin_ExportDoc_Cost = clientCode.Admin_ExportDoc_Cost,
                        Custome_Cost_Qomrk = clientCode.Custome_Cost_Qomrk,
                        PostDate = clientCode.PostDate,
                        Packiging_cost_IQD = clientCode.Packiging_cost_IQD,
                        Custom_Cost_IQD = clientCode.Custom_Cost_IQD,
                        POST_DoorToDoor_IQD = clientCode.POST_DoorToDoor_IQD,
                        Sub_Post_Cost_IQD = clientCode.Sub_Post_Cost_IQD,
                        Discount_Post_Cost_Send = clientCode.Discount_Post_Cost_Send,
                        Total_Post_Cost_IQD = clientCode.Total_Post_Cost_IQD,
                        TotalPaid_IQD = clientCode.TotalPaid_IQD,
                        EuropaToPay = clientCode.EuropaToPay,
                        Currency_Rate_1_IQD = clientCode.Currency_Rate_1_IQD,
                        Currency_Type = clientCode.Currency_Type,
                        PriceDoorToDoorEach10KG = clientCode.PriceDoorToDoorEach10KG,
                        Price_KG_IQD = clientCode.Price_KG_IQD,
                        StartPrice_1_to_7KG = clientCode.StartPrice_1_to_7KG,
                        Street_Name_No = clientCode.Street_Name_No,
                        Dep_Appar = clientCode.Dep_Appar,
                        ZipCode = clientCode.ZipCode,
                        CityPost = clientCode.CityPost,
                        Note_Send = clientCode.Note_Send,
                        Have_Local_Post = clientCode.Have_Local_Post,
                        CommissionBox = clientCode.CommissionBox,
                        CommissionKG = clientCode.CommissionKG,
                        Weight_W_cm = clientCode.Weight_W_cm,
                        Weight_L_cm = clientCode.Weight_L_cm,
                        Weight_H_cm = clientCode.Weight_H_cm,
                        UserId = clientCode.UserId,
                        CountryAgentId = clientCode.CountryAgentId,
                        BranchId = clientCode.BranchId,
                        CountryPostId = clientCode.CountryPostId,
                        AgentId = clientCode.AgentId,
                        Person_in_charge_Id = clientCode.Person_in_charge_Id,
                        BoxPackigingFactor = clientCode.BoxPackigingFactor


                    };

                    stersDB.ClientCode.Add(newcode);
                    //  stersDB.ClientCode.Add(clientCode);
                    //  stersDB.Set<ClientCode>().Add(clientCode);
                    // stersDB.Entry(clientCode.Branch).State = EntityState.Unchanged;
                    //  stersDB.Entry(clientCode.User).State = EntityState.Unchanged;
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

        public double get_last_shipmentNoForSpecificYear(long w_BranchID, int year)
        {
            double lastno = 0;
            using (StersDB stersDB = new StersDB())
            {
                var last = stersDB.ClientCode.Where(x => x.BranchId == w_BranchID && x.PostYear == year).Max(b => b.Shipment_No);
                if (last != null)
                {
                    lastno = (double)last;
                }
            }
            return lastno;
        }

        public void UpdateClientCode(ClientCode W_clientCode, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {
                    var Original_clientcode = stersDB.ClientCode.Where(c => c.Code == W_clientCode.Code && c.stamp == W_clientCode.stamp).FirstOrDefault();
                    if (Original_clientcode != null)
                    {
                        stersDB.Entry(Original_clientcode).CurrentValues.SetValues(W_clientCode);
                        if (Original_clientcode.virtual_Branch != null)
                        { stersDB.Entry(Original_clientcode.virtual_Branch).State = EntityState.Unchanged; }

                        if (Original_clientcode.User != null)
                        { stersDB.Entry(Original_clientcode.User).State = EntityState.Unchanged; }

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

        public void DeleteClientCode(ClientCode W_clientCode, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                if (W_clientCode != null)
                {
                    using (StersDB stersDB = new StersDB())
                    {
                        var x = stersDB.ClientCode.Find(W_clientCode.Code);
                        if (x != null)
                        {
                            stersDB.ClientCode.Remove(x);


                            if (x.virtual_Branch != null)
                            { stersDB.Entry(x.virtual_Branch).State = EntityState.Unchanged; }

                            if (x.User != null)
                            { stersDB.Entry(x.User).State = EntityState.Unchanged; }


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

        public void Set_Code_Properties_ToNull(ClientCode clientCode, out string errormessage)
        {
            errormessage = string.Empty;
            try
            {
                //db logic
                using (StersDB stersDB = new StersDB())
                {

                    //  var originalcode = code.Code;
                    //   var originalstamp = code.stamp;

                    // set values to null
                    var C_code = clientCode;
                    var Original_clientcode = stersDB.ClientCode.Where(c => c.Code == C_code.Code && c.stamp == C_code.stamp).FirstOrDefault();

                    C_code.shipmentNoIsvalidating = false;
                    C_code.isvalidating = false;

                    C_code.Weight_By_Size_Is_Checked = false;
                    //  C_code.BranchCode = null;
                    //  C_code.YearCode = null;
                    C_code.Shipment_No = null;
                    C_code.SenderName = null;
                    C_code.Sender_ID = null;
                    C_code.Sender_ID_Type = null;
                    C_code.Sender_ID_Type_Shortcut = null;
                    C_code.Sender_Tel = null;
                    C_code.ReceiverName = null;
                    C_code.Receiver_Tel = null;
                    C_code.Goods_Description = null;
                    C_code.Goods_Value = null;
                    C_code.Have_Insurance = false;
                    C_code.Insurance_Percentage = null;
                    C_code.Insurance_Amount = null;
                    C_code.Pallet_No = null;
                    C_code.Box_No = null;
                    C_code.Weight_Kg = null;
                    C_code.Weight_Vol_Factor = null;
                    C_code.AdditionalWeight = null;
                    C_code.Weight_Total = null;
                    C_code.Admin_ExportDoc_Cost = null;
                    C_code.Custome_Cost_Qomrk = null;
                   // C_code.PostDate = null;  // modifeild to be not affected...
                    C_code.Packiging_cost_IQD = null;
                    C_code.BoxPackigingFactor = null;
                    C_code.Custom_Cost_IQD = null;
                    C_code.POST_DoorToDoor_IQD = null;

                    C_code.Discount_Post_Cost_Send = null;

                    C_code.TotalPaid_IQD = null;

                    C_code.Currency_Rate_1_IQD = null;
                    C_code.Currency_Type = null;
                    C_code.PriceDoorToDoorEach10KG = null;
                    C_code.Price_KG_IQD = null;
                    C_code.StartPrice_1_to_7KG = null;

                    // keep order for now for the follwing three lines ...
                    C_code.Sub_Post_Cost_IQD = null;
                    C_code.Total_Post_Cost_IQD = null;
                    C_code.EuropaToPay = null;


                    C_code.Street_Name_No = null;
                    C_code.Dep_Appar = null;
                    C_code.ZipCode = null;
                    C_code.CityPost = null;
                    C_code.Note_Send = null;
                    C_code.Have_Local_Post = false;
                    //  C_code.Num = null;
                    C_code.CommissionKG = null;
                    C_code.CommissionBox = null;
                    C_code.Weight_L_cm = null;
                    C_code.Weight_H_cm = null;
                    C_code.Weight_W_cm = null;
                    C_code.CountryAgentId = null;
                    C_code.CountryPostId = null;
                    C_code.AgentId = null;


                    if (Original_clientcode != null)
                    {
                        stersDB.Entry(Original_clientcode).CurrentValues.SetValues(C_code);
                    }

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
        public long get_max_number(long branchID, string w_branchprefix, string w_yearprefix)
        {
            long maxnumber = 0;
            using (StersDB stersDB = new StersDB())
            {
                var maxclientcodeNumber = stersDB.ClientCode.Where
                    (x => x.BranchCode == w_branchprefix && x.YearCode == w_yearprefix && x.BranchId == branchID).Max(x => x.Num);
                if (maxclientcodeNumber != null)
                { maxnumber = (long)maxclientcodeNumber; }
            }
            return maxnumber;
        }

        public double get_last_shipmentNo(Int64 W_branch,string branchCode,string yearCode)
        {
            double lastno = 0;
            using (StersDB stersDB = new StersDB())
            {
                var last = stersDB.ClientCode.Where(x => x.BranchId == W_branch&&x.BranchCode==branchCode&&x.YearCode==yearCode).Max(b => b.Shipment_No);
                if (last != null)
                {
                    lastno = (double)last;
                }
            }
            return lastno;
        }

        public bool IsLastCode(Int64 W_branch,long currentNum)
        {
            bool islast = false;
            long lastno = 0;
            using (StersDB stersDB = new StersDB())
            {
                var last = stersDB.ClientCode.Where(x => x.BranchId == W_branch).Max(b => b.Num);
                if (last != null)
                {
                    lastno = (long)last;
                }
            }

            if (currentNum == lastno)
            { islast = true; }

            return islast;
        }

        public bool IsLastCodeForSpecificYear(Int64 W_branch, long currentNum,string yearCode,string branchCode)
        {
            bool islast = false;
            long lastno = 0;
            using (StersDB stersDB = new StersDB())
            {
                var last = stersDB.ClientCode.Where(x => x.BranchId == W_branch&&x.YearCode== yearCode&&x.BranchCode==branchCode).Max(b => b.Num);
                if (last != null)
                {
                    lastno = (long)last;
                }
            }

            if (currentNum == lastno)
            { islast = true; }

            return islast;
        }

        public DataTable Generate_Country_Summary_View(Agent selectedBranch, Country selectedCountry,Agent selectedAgent,double selectedShipmentNumber,DateTime?sd,DateTime?ed)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                //  string commandTXT = "select CODE_LIST.Client_Code, tbl_Country.CountryName,CODE_LIST.CountryAgentId,CODE_LIST.AgentId,CODE_LIST.Shipment_No,CODE_LIST.PostDate,CODE_LIST.BranchId,CODE_LIST.Box_No,CODE_LIST.Pallet_No,CODE_LIST.Weight_Total,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.CommissionBox,CODE_LIST.CommissionKG,TotalPaid_IQD,CODE_LIST.EuropaToPay from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id Where 21>19";

                string commandTXT = @"select CODE_LIST.Client_Code, tbl_Country.CountryName,CODE_LIST.CountryAgentId,CODE_LIST.AgentId,CODE_LIST.Shipment_No,CODE_LIST.PostDate,CODE_LIST.BranchId,CODE_LIST.Box_No,CODE_LIST.Pallet_No,
CODE_LIST.Weight_Kg,CODE_LIST.Weight_Vol,CODE_LIST.Weight_Total,CODE_LIST.Packiging_cost_IQD,CODE_LIST.Custome_Cost_Qomrk,CODE_LIST.POST_DoorToDoor_IQD,CODE_LIST.Sub_Post_Cost_IQD,CODE_LIST.Discount_Post_Cost_Send,
CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.CommissionBox,CODE_LIST.CommissionKG,TotalPaid_IQD,CODE_LIST.EuropaToPay,CODE_LIST.Currency_Type
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id Where 21>19";
                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And BranchId='" + selectedBranch.Id + "'"; }
                        
                }
                if (selectedCountry != null)
                {
                    if (selectedCountry.Id != 0) { commandTXT += " And CountryAgentId='" + selectedCountry.Id + "'"; }
                    
                }
                if (selectedAgent != null)
                {
                    if (selectedAgent.Id != 0)
                    { commandTXT += " And AgentId='" + selectedAgent.Id + "'"; }
                   
                }
                if (selectedShipmentNumber != 0)
                {
                    commandTXT += " And Shipment_No='" + selectedShipmentNumber + "'";
                }

                //date...
                if (sd.HasValue || ed.HasValue)
                {
                    if (sd.HasValue && !ed.HasValue)
                    {
                        commandTXT += " And PostDate>@sd";
                        cmd.Parameters.Add("sd", SqlDbType.SmallDateTime).Value = sd.Value;
                    }
                    else if (!sd.HasValue && ed.HasValue)
                    {
                        commandTXT += " And PostDate<@ed";
                        cmd.Parameters.Add("ed", SqlDbType.SmallDateTime).Value = ed.Value;
                    }
                    else
                    {
                        commandTXT += " And PostDate Between @sd And @ed";
                        cmd.Parameters.Add("sd", SqlDbType.SmallDateTime).Value = sd.Value;
                        cmd.Parameters.Add("ed", SqlDbType.SmallDateTime).Value = ed.Value;
                    }
                }
               
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable Generate_Country_Report_View(Agent selectedBranch, Country selectedCountry, Agent selectedAgent, double selectedShipmentNumber, double selectedShipmentNumberto)  
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                /* old query
                string commandTXT = @"select CODE_LIST.Client_Code,CODE_LIST.BranchCode,CODE_LIST.YearCode,CODE_LIST.Num,CODE_LIST.Shipment_No, CODE_LIST.SenderName,CODE_LIST.SenderCompany,CODE_LIST.Sender_ID,CODE_LIST.Sender_Tel,
CODE_LIST.ReceiverName,CODE_LIST.ReceiverCompany,CODE_LIST.Receiver_Tel,CODE_LIST.Goods_Description,CODE_LIST.Goods_Value,CODE_LIST.Have_Insurance,CODE_LIST.Insurance_Percentage,CODE_LIST.Insurance_Amount,
CODE_LIST.Pallet_No,CODE_LIST.Box_No,CODE_LIST.Weight_Kg,CODE_LIST.Weight_Vol_Factor,CODE_LIST.Weight_Vol,CODE_LIST.Weight_Total,CODE_LIST.Custome_Cost_Qomrk,CODE_LIST.PostDate,
CODE_LIST.Packiging_cost_IQD,CODE_LIST.POST_DoorToDoor_IQD,CODE_LIST.Sub_Post_Cost_IQD,CODE_LIST.Discount_Post_Cost_Send,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.TotalPaid_IQD,CODE_LIST.EuropaToPay,
CODE_LIST.Currency_Rate_1_IQD,CODE_LIST.Currency_Type,CODE_LIST.PriceDoorToDoorEach10KG,CODE_LIST.Price_KG_IQD,CODE_LIST.StartPrice_1_to_7KG,CODE_LIST.Weight_H_cm,CODE_LIST.Weight_L_cm,CODE_LIST.Weight_W_cm,
CODE_LIST.BranchId,CODE_LIST.AgentId,CODE_LIST.CountryAgentId,CODE_LIST.UserId,CODE_LIST.CountryPostId,CODE_LIST.Person_in_charge_Id,
CODE_LIST.Street_Name_No,CODE_LIST.Dep_Appar,CODE_LIST.ZipCode,CODE_LIST.CityPost,CODE_LIST.Note_Send,CODE_LIST.Have_Local_Post,
tbl_Country.CountryName
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id where 4<21";

                */

                /*
                string commandTXT = @"select CODE_LIST.Client_Code,CODE_LIST.BranchCode,CODE_LIST.YearCode,CODE_LIST.Num,CODE_LIST.Shipment_No, CODE_LIST.SenderName,CODE_LIST.SenderCompany,CODE_LIST.Sender_ID,CODE_LIST.Sender_Tel,
CODE_LIST.ReceiverName,CODE_LIST.ReceiverCompany,CODE_LIST.Receiver_Tel,CODE_LIST.Goods_Description,CODE_LIST.Goods_Value,CODE_LIST.Have_Insurance,CODE_LIST.Insurance_Percentage,CODE_LIST.Insurance_Amount,
CODE_LIST.Pallet_No,CODE_LIST.Box_No,CODE_LIST.Weight_Kg,CODE_LIST.Weight_Vol_Factor,CODE_LIST.Weight_Vol,CODE_LIST.Weight_Total,CODE_LIST.Custome_Cost_Qomrk,CODE_LIST.PostDate,
CODE_LIST.Packiging_cost_IQD,CODE_LIST.POST_DoorToDoor_IQD,CODE_LIST.Sub_Post_Cost_IQD,CODE_LIST.Discount_Post_Cost_Send,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.TotalPaid_IQD,CODE_LIST.EuropaToPay,
CODE_LIST.Currency_Rate_1_IQD,CODE_LIST.Currency_Type,CODE_LIST.PriceDoorToDoorEach10KG,CODE_LIST.Price_KG_IQD,CODE_LIST.StartPrice_1_to_7KG,CODE_LIST.Weight_H_cm,CODE_LIST.Weight_L_cm,CODE_LIST.Weight_W_cm,
BranchName,tbl_Country.CountryName,AgentName,tbl_User.UserName,tbl_CountryPOst.CountryName as CountryPost,PersonsIncharge.UserName as Person_IN_Charge,
CODE_LIST.Street_Name_No,CODE_LIST.Dep_Appar,CODE_LIST.ZipCode,CODE_LIST.CityPost,CODE_LIST.Note_Send,CODE_LIST.Have_Local_Post
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id
inner join tbl_Branch on CODE_LIST.BranchId=tbl_Branch.Id
inner join tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id
left join tbl_User on CODE_LIST.UserId=tbl_User.Id
left join tbl_Country tbl_CountryPOst on CODE_LIST.CountryPostId=tbl_CountryPOst.Id
left join tbl_User PersonsIncharge on CODE_LIST.Person_in_charge_Id=PersonsIncharge.Id
where 4<21";
                */

                /*
                string commandTXT = @"select CODE_LIST.Client_Code,CODE_LIST.BranchCode,CODE_LIST.YearCode,CODE_LIST.Num,CODE_LIST.Shipment_No, CODE_LIST.SenderName,CODE_LIST.SenderCompany,CODE_LIST.Sender_ID,CODE_LIST.Sender_Tel,
CODE_LIST.ReceiverName,CODE_LIST.ReceiverCompany,CODE_LIST.Receiver_Tel,CODE_LIST.Goods_Description,CODE_LIST.Goods_Value,CODE_LIST.Have_Insurance,CODE_LIST.Insurance_Percentage,CODE_LIST.Insurance_Amount,
CODE_LIST.Pallet_No,CODE_LIST.Box_No,CODE_LIST.Weight_Kg,CODE_LIST.Weight_Vol_Factor,CODE_LIST.Weight_Vol,CODE_LIST.Weight_Total,CODE_LIST.Custome_Cost_Qomrk,CODE_LIST.PostDate,
CODE_LIST.Packiging_cost_IQD,CODE_LIST.POST_DoorToDoor_IQD,CODE_LIST.Sub_Post_Cost_IQD,CODE_LIST.Discount_Post_Cost_Send,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.TotalPaid_IQD,CODE_LIST.EuropaToPay,
CODE_LIST.Currency_Rate_1_IQD,CODE_LIST.Currency_Type,CODE_LIST.PriceDoorToDoorEach10KG,CODE_LIST.Price_KG_IQD,CODE_LIST.StartPrice_1_to_7KG,CODE_LIST.Weight_H_cm,CODE_LIST.Weight_L_cm,CODE_LIST.Weight_W_cm,
branches.AgentName as BranchName,tbl_Country.CountryName,tbl_Agent.AgentName,tbl_User.UserName,tbl_CountryPOst.CountryName as CountryPost,PersonsIncharge.UserName as Person_IN_Charge,
CODE_LIST.Street_Name_No,CODE_LIST.Dep_Appar,CODE_LIST.ZipCode,CODE_LIST.CityPost,CODE_LIST.Note_Send,CODE_LIST.Have_Local_Post,CODE_LIST.BoxPackigingFactor
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id
inner join tbl_Agent branches on CODE_LIST.BranchId=branches.Id
inner join tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id
left join tbl_User on CODE_LIST.UserId=tbl_User.Id
left join tbl_Country tbl_CountryPOst on CODE_LIST.CountryPostId=tbl_CountryPOst.Id
left join tbl_User PersonsIncharge on CODE_LIST.Person_in_charge_Id=PersonsIncharge.Id
where 4<21";
                */

                string commandTXT = @"select CODE_LIST.Client_Code,CODE_LIST.Shipment_No, CODE_LIST.SenderName,CODE_LIST.SenderCompany,CODE_LIST.Sender_ID,CODE_LIST.Sender_Tel,
CODE_LIST.ReceiverName,CODE_LIST.ReceiverCompany,CODE_LIST.Receiver_Tel,CODE_LIST.Goods_Description,CODE_LIST.Goods_Value,CODE_LIST.Have_Insurance,CODE_LIST.Insurance_Percentage,CODE_LIST.Insurance_Amount,
CODE_LIST.Pallet_No,CODE_LIST.Box_No,CODE_LIST.Weight_Kg,CODE_LIST.Weight_Vol_Factor,CODE_LIST.Weight_Vol,CODE_LIST.Weight_Total,CODE_LIST.Custome_Cost_Qomrk,CODE_LIST.PostDate,
CODE_LIST.Packiging_cost_IQD,CODE_LIST.POST_DoorToDoor_IQD,CODE_LIST.Sub_Post_Cost_IQD,CODE_LIST.Discount_Post_Cost_Send,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.TotalPaid_IQD,CODE_LIST.EuropaToPay,
CODE_LIST.Currency_Rate_1_IQD,CODE_LIST.Currency_Type,CODE_LIST.PriceDoorToDoorEach10KG,CODE_LIST.Price_KG_IQD,CODE_LIST.StartPrice_1_to_7KG,CODE_LIST.Weight_H_cm,CODE_LIST.Weight_L_cm,CODE_LIST.Weight_W_cm,
branches.AgentName as BranchName,tbl_Country.CountryName,tbl_Agent.AgentName,tbl_User.UserName,tbl_CountryPOst.CountryName as CountryPost,PersonsIncharge.UserName as Person_IN_Charge,
CODE_LIST.Street_Name_No,CODE_LIST.Dep_Appar,CODE_LIST.ZipCode,CODE_LIST.CityPost,CODE_LIST.Note_Send,CODE_LIST.Have_Local_Post,CODE_LIST.BoxPackigingFactor
,CODE_LIST.Update_Send_KU,CODE_LIST.Receive_State,CODE_LIST.Receive_Date_Time,CODE_LIST.Note_Received,CODE_LIST.Agent_EU_ReceiverName,CODE_LIST.Person_in_charge_Receive,
CODE_LIST.Received_Amount_EU,CODE_LIST.Discount_Post_Cost_Received,CODE_LIST.PaymentWAY_Cash_PIN_Bank,CODE_LIST.Update_Receive_EU,CODE_LIST.Sender_ID_Type,
CODE_LIST.BranchCode,CODE_LIST.YearCode,CODE_LIST.Num
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id
inner join tbl_Agent branches on CODE_LIST.BranchId=branches.Id
inner join tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id
left join tbl_User on CODE_LIST.UserId=tbl_User.Id
left join tbl_Country tbl_CountryPOst on CODE_LIST.CountryPostId=tbl_CountryPOst.Id
left join tbl_User PersonsIncharge on CODE_LIST.Person_in_charge_Id=PersonsIncharge.Id
where 4<21";


                /* old 
                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And BranchId='" + selectedBranch.Id + "'"; }

                }
                if (selectedCountry != null)
                {
                    if (selectedCountry.Id != 0) { commandTXT += " And CountryAgentId='" + selectedCountry.Id + "'"; }

                }
                if (selectedAgent != null)
                {
                    if (selectedAgent.Id != 0)
                    { commandTXT += " And AgentId='" + selectedAgent.Id + "'"; }

                }
                */


                // here filtering with name has no effect as we dont store name in code list table....
                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And branches.AgentName='" + selectedBranch.AgentName + "'"; }

                }
                if (selectedCountry != null)
                {
                    if (selectedCountry.Id != 0) { commandTXT += " And tbl_Country.CountryName='" + selectedCountry.CountryName + "'"; }

                }
                if (selectedAgent != null)
                {
                    if (selectedAgent.Id != 0)
                    { commandTXT += " And tbl_Agent.AgentName='" + selectedAgent.AgentName + "'"; }

                }

                if (selectedShipmentNumber != 0|| selectedShipmentNumberto!=0)
                {

                    if (selectedShipmentNumber != 0 && selectedShipmentNumberto == 0)
                    {
                        commandTXT += " And Shipment_No>='" + selectedShipmentNumber + "'";
                    }
                    else if (selectedShipmentNumber == 0 && selectedShipmentNumberto != 0)
                    { commandTXT += " And Shipment_No<='" + selectedShipmentNumberto + "'"; }
                    else
                    {
                        commandTXT += " And Shipment_No Between @sp And @spto ";
                        cmd.Parameters.Add("sp", SqlDbType.Float).Value = selectedShipmentNumber;
                        cmd.Parameters.Add("spto", SqlDbType.Float).Value = selectedShipmentNumberto;
                    }
                       
                }
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }



        public DataTable Generate_Country_Report_View_FewerColumns(Agent selectedBranch, Country selectedCountry, Agent selectedAgent, double selectedShipmentNumber, double selectedShipmentNumberto)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

 

                string commandTXT = @"select CODE_LIST.Client_Code,CODE_LIST.Shipment_No, CODE_LIST.SenderName,CODE_LIST.Sender_Tel,
CODE_LIST.ReceiverName,CODE_LIST.Receiver_Tel,CODE_LIST.Goods_Description,CODE_LIST.Goods_Value,CODE_LIST.Insurance_Amount,CODE_LIST.Box_No,CODE_LIST.Weight_Total,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.EuropaToPay,
branches.AgentName as BranchName,tbl_Agent.AgentName
from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id
inner join tbl_Agent branches on CODE_LIST.BranchId=branches.Id
inner join tbl_Agent on CODE_LIST.AgentId=tbl_Agent.Id
left join tbl_User on CODE_LIST.UserId=tbl_User.Id
left join tbl_Country tbl_CountryPOst on CODE_LIST.CountryPostId=tbl_CountryPOst.Id
left join tbl_User PersonsIncharge on CODE_LIST.Person_in_charge_Id=PersonsIncharge.Id
where 4<21";

               
                // here filtering with name has no effect as we dont store name in code list table....
                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And branches.AgentName='" + selectedBranch.AgentName + "'"; }

                }
                if (selectedCountry != null)
                {
                    if (selectedCountry.Id != 0) { commandTXT += " And tbl_Country.CountryName='" + selectedCountry.CountryName + "'"; }

                }
                if (selectedAgent != null)
                {
                    if (selectedAgent.Id != 0)
                    { commandTXT += " And tbl_Agent.AgentName='" + selectedAgent.AgentName + "'"; }

                }

                if (selectedShipmentNumber != 0 || selectedShipmentNumberto != 0)
                {

                    if (selectedShipmentNumber != 0 && selectedShipmentNumberto == 0)
                    {
                        commandTXT += " And Shipment_No>='" + selectedShipmentNumber + "'";
                    }
                    else if (selectedShipmentNumber == 0 && selectedShipmentNumberto != 0)
                    { commandTXT += " And Shipment_No<='" + selectedShipmentNumberto + "'"; }
                    else
                    {
                        commandTXT += " And Shipment_No Between @sp And @spto ";
                        cmd.Parameters.Add("sp", SqlDbType.Float).Value = selectedShipmentNumber;
                        cmd.Parameters.Add("spto", SqlDbType.Float).Value = selectedShipmentNumberto;
                    }

                }
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        public DataTable Generate_Quick_Report_View(Agent selectedBranch, double selectedShipmentNumber)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = "select CODE_LIST.Client_Code, tbl_Country.CountryName,CODE_LIST.CountryAgentId,CODE_LIST.AgentId,CODE_LIST.Shipment_No,CODE_LIST.PostDate,CODE_LIST.BranchId,CODE_LIST.Box_No,CODE_LIST.Pallet_No,CODE_LIST.Weight_Total,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.CommissionBox,CODE_LIST.CommissionKG,TotalPaid_IQD,CODE_LIST.EuropaToPay,CODE_LIST.Currency_Type from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id Where 21>19";

                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And BranchId='" + selectedBranch.Id + "'"; }

                }
                
                if (selectedShipmentNumber != 0 )
                {

                    commandTXT += " And Shipment_No='" + selectedShipmentNumber + "'";

                }
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable Generate_Daily_Report_View(Agent selectedBranch,DateTime? selecteddate)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = "select CODE_LIST.Client_Code, tbl_Country.CountryName,CODE_LIST.CountryAgentId,CODE_LIST.AgentId,CODE_LIST.Shipment_No,CODE_LIST.PostDate,CODE_LIST.BranchId,CODE_LIST.Box_No,CODE_LIST.Pallet_No,CODE_LIST.Weight_Total,CODE_LIST.Total_Post_Cost_IQD,CODE_LIST.CommissionBox,CODE_LIST.CommissionKG,TotalPaid_IQD,CODE_LIST.EuropaToPay,CODE_LIST.Currency_Type from code_list inner join tbl_Country on CODE_LIST.CountryAgentId=tbl_Country.id Where 21>19";

                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { commandTXT += " And BranchId='" + selectedBranch.Id + "'"; }

                }

                if (selecteddate.HasValue  )
                {
                    commandTXT += " And Cast(PostDate As Date)=@sd";
                    cmd.Parameters.Add("sd", SqlDbType.SmallDateTime).Value = selecteddate.Value.Date;
                }

                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        /// <summary>
        /// Get The Codes By Providing Sender Telelphone
        /// </summary>
        /// <param name="senderTel"></param>
        /// <returns></returns>
        public DataTable get_sender_Codes(string senderTel)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = " "+BaseSenderReceiverSummaryQuery+" where Sender_Tel = '"+
                    senderTel + "' or Sender_Tel like '%'+CHAR(13) + CHAR(10)+'"+ senderTel + "' or Sender_Tel like '"+ senderTel + "' + CHAR(13) + CHAR(10) + '%'";
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public DataTable get_receiver_Codes(string receiverTel)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = " "+BaseSenderReceiverSummaryQuery+" where Receiver_Tel = '" +
                     receiverTel + "' or Receiver_Tel like '%'+CHAR(13) + CHAR(10)+'" + receiverTel + "' or Receiver_Tel like '" + receiverTel + "' + CHAR(13) + CHAR(10) + '%'";
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public DataTable get_sender_Codes_BySenderName(string senderName)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = " " + BaseSenderReceiverSummaryQuery + " where SenderName = '" +
                    senderName + "'";
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


        public DataTable get_receiver_Codes_ByReceiverName(string receiverName)
        {
            DataTable dt = new DataTable();
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                string commandTXT = " " + BaseSenderReceiverSummaryQuery + " where ReceiverName = '" +
                     receiverName + "' ";
                cmd.CommandText = commandTXT;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }


    }
}
