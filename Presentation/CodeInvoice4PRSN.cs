﻿using StersTransport.DataAccess;
using StersTransport.GlobalData;
using StersTransport.Models;
using StersTransport.Properties;
using StersTransport.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace StersTransport.Presentation
{
    public class CodeInvoice4PRSN
    {
        ClientCodeDA clientCodeDA;
        NewPostInvoice invoice;
        CountryDa countryDa;
        AgentDa agentDa;
        BranchDa branchDa;
        CurrencyDa currencyDa ;
        Agent agent { get; set;}
        Agent branch { get; set;}
        Currency OuterCurrency { get; set;}


        public CodeInvoice4PRSN() 
        {
             clientCodeDA = new ClientCodeDA();
            invoice = new NewPostInvoice();
            countryDa = new CountryDa();
             agentDa = new AgentDa();
             branchDa = new BranchDa();
            currencyDa = new CurrencyDa();
        }

        public CodeInvoice4PRSN(bool ispost): base()
        {

        }

        public void GenerateDocument(Frame frame ,string code )
        {
            var clientcode = clientCodeDA.GetClientCode(code);
            var country = string.Empty;
            long _agentID = clientcode.AgentId.HasValue ? (long)clientcode.AgentId : 0;
            long _branchID = clientcode.BranchId.HasValue ? (long)clientcode.BranchId : 0;
            agent = agentDa.GetAgent(_agentID);
            branch = agentDa.GetAgent(_branchID);
            long _CountryAgentId = clientcode.CountryAgentId.HasValue ? (long)clientcode.CountryAgentId : 0;
            var countryAgent = countryDa.GetCountry(_CountryAgentId);

            if (clientcode.CountryPostId.HasValue == true)
            {
                country = Getcountry((long)clientcode.CountryPostId);

            }

            if (countryAgent != null) 
            { 
                OuterCurrency = currencyDa.GetCurrency((long)countryAgent.CurrencyId);
            }

            bool havelocalpost = clientcode.Have_Local_Post.HasValue ? (bool)clientcode.Have_Local_Post : false;


            //header Details
            invoice.branch_val.Text = branch.AgentName;
            invoice.tele.Text = branch.PhonesDisplayString;

            //Code Details 
            invoice.code_number.Text = clientcode.Code;
            invoice.date_val.Text = clientcode.PostDate.HasValue ? clientcode.PostDate.Value.ToShortDateString() : string.Empty;

            //sender and reciever information 

            invoice.sender_name.Text = clientcode.SenderName;
            invoice.reciever_name.Text = clientcode.ReceiverName;
            invoice.sender_phone.Text = clientcode.Sender_Tel;
            invoice.reciever_phone.Text = clientcode.Receiver_Tel;
            invoice.zipcode.Text = clientcode.ZipCode;
            invoice.address.Text = $"{clientcode.Street_Name_No}";
            invoice.city.Text = clientcode.CityPost;
            invoice.country.Text = country;

            /////////////////////////////////

            //transportation costs

            invoice.trans_cost.Text = clientcode.Sub_Post_Cost_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.Sub_Post_Cost_IQD) : "0";
            invoice.empty_cost.Text = clientcode.Packiging_cost_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.Packiging_cost_IQD) : "0";
            invoice.customs_cost.Text = clientcode.Custome_Cost_Qomrk.HasValue ? string.Format("{0:#,0.##}", clientcode.Custome_Cost_Qomrk): "0";
            invoice.insurance_cost.Text = clientcode.Insurance_Amount.HasValue ? string.Format("{0:#,0.##}", clientcode.Insurance_Amount) : "0";
            invoice.total_cost.Text = clientcode.Total_Post_Cost_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.Total_Post_Cost_IQD) : "0";
            invoice.amount_paid.Text = clientcode.TotalPaid_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.TotalPaid_IQD) : "0";
            invoice.amount_not_paid.Text = clientcode.Remaining_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.Remaining_IQD) : "0";
            invoice.europe_amount.Text = clientcode.EuropaToPay.HasValue ? string.Format("{0:#,0.##}", clientcode.EuropaToPay) : "0";
            invoice.reciever_addr_cost.Text = clientcode.POST_DoorToDoor_IQD.HasValue ? string.Format("{0:#,0.##}", clientcode.POST_DoorToDoor_IQD) :"0" ;
            invoice.currency.Text = OuterCurrency.Name;
            //insurance check 
            var haveinsurance = clientcode.Have_Insurance.HasValue ? (bool)clientcode.Have_Insurance : false;

            if(haveinsurance == true)
            {
                invoice.insurance_yes.IsChecked = true;
            }
            else
            {
                invoice.insurance_no.IsChecked = true;
            }

            // goods details 
            double boxno =  clientcode.Box_No.HasValue ? (double)clientcode.Box_No : 0;
            double Palletno = clientcode.Pallet_No.HasValue ? (double)clientcode.Box_No : 0;
            string boxstr = string.Format("{0} Box(s)", boxno.ToString());
            string palletstr = string.Empty;
            if (Palletno > 0)
            {
                palletstr = string.Format("{0} Pallet(s)", Palletno.ToString());
                invoice.pallets_no.Text = palletstr;
            }
            invoice.boxes_no.Text = boxstr;
            invoice.weight.Text = clientcode.Weight_Total.HasValue ? clientcode.Weight_Total.ToString() : "0";
            invoice.good_desc.Text = clientcode.Goods_Description;
            invoice.good_val.Text = clientcode.Goods_Value.HasValue ? string.Format("{0:#,0.##}", clientcode.Goods_Value.ToString()) : string.Empty;


            // agent details 
            invoice.office_val.Text = string.Format("{0}-{1}", agent.AgentName, countryAgent.CountryName);
            invoice.agent_val.Text = string.Format("{0}", agent.CompanyName);
            invoice.phone.Text = string.Format("{0}", agent.PhonesDisplayString);
            invoice.agent_address.Text = agent.Address;

            if (agent.InvoiceLanguage == "Ku")
            {


            }

            frame.Content = invoice;
        }

        public string Getcountry(long? id )
        {
            if ( id != null )
            {
                var newid = (long)id;
                return countryDa.GetCountry(newid).CountryName;
            }

            return "";
        }

        public void GenerateDocumentPost(Frame frame, string code)
        {
        }



        


        //private string getlabel(string key, string labelLanguageColumnName)
        //{
        //    string lbl = string.Empty;
        //    DataRow DR_Label = StaticData.Labels.Select("Keyword='" + key + "'").FirstOrDefault();

        //    if (DR_Label != null)
        //    {
        //        //labelLanguageColumnName
        //        lbl = DR_Label[labelLanguageColumnName].ToString();
        //    }
        //    return lbl;
        //}
    }
}
