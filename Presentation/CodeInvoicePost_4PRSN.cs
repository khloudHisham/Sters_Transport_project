using StersTransport.DataAccess;
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
    public class CodeInvoicePost_4PRSN
    {

        //SolidColorBrush Orangecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E89933"));
        SolidColorBrush GreenColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00BB7E"));
        SolidColorBrush yellowColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#f5f205"));

        SolidColorBrush Orangecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFFCB483"));
        SolidColorBrush bluecolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#3fc7f9"));


        protected ClientCodeDA clientCodeDA;
        protected NewPostInvoice invoice;
        protected CountryDa countryDa;
        protected AgentDa agentDa;
        protected BranchDa branchDa;
        protected CurrencyDa currencyDa ;
        protected Agent agent { get; set;}
        protected Agent branch { get; set;}
        protected Currency OuterCurrency { get; set;}
        Country countryAgent { get; set; }
        City AgentCity { get; set; }

        public CodeInvoicePost_4PRSN() 
        {
             clientCodeDA = new ClientCodeDA();
             invoice = new NewPostInvoice();
             countryDa = new CountryDa();
             agentDa = new AgentDa();
             branchDa = new BranchDa();
             currencyDa = new CurrencyDa();
        }

        public CodeInvoicePost_4PRSN(bool ispost): base()
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


            if (agent != null)
            {
                long agentCityId = agent.CityId.HasValue ? (long)agent.CityId : 0;
                CityDa cityDa = new CityDa();
                AgentCity = cityDa.GetCity(agentCityId);

            }

            if (clientcode.CountryPostId.HasValue == true)
            {
                country = Getcountry((long)clientcode.CountryPostId);

            }

            if (countryAgent != null) 
            { 
                OuterCurrency = currencyDa.GetCurrency((long)countryAgent.CurrencyId);
            }

            bool havelocalpost = clientcode.Have_Local_Post.HasValue ? (bool)clientcode.Have_Local_Post : false;

            if (havelocalpost )
            {

            }

            //header Details
            invoice.branch_val.Text = branch.AgentName;
            invoice.tele.Text = branch.PhonesDisplayString.Replace("/", "");

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
            invoice.country.Text = countryAgent.CountryName;

            /////////////////////////////////

            //transportation costs


            if (clientcode.TotalPaid_IQD.HasValue && clientcode.TotalPaid_IQD != 0)
            {
                invoice.paid_border.Background = yellowColor;
            }

            if (clientcode.POST_DoorToDoor_IQD.HasValue && clientcode.POST_DoorToDoor_IQD != 0)
            {

                invoice.recv_address_border.Background = bluecolor;
            }

            if (clientcode.Remaining_IQD.HasValue && clientcode.Remaining_IQD != 0)
            {
                invoice.remaining_border.Background = Orangecolor;

            }

            if (clientcode.EuropaToPay.HasValue && clientcode.EuropaToPay != 0)
            {
                invoice.europe_border.Background = Orangecolor;

            }




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
                invoice.insurance_border.Background = GreenColor;
            }
            else
            {
                invoice.insurance_no.IsChecked = true;
            }

            // goods details 
            double boxno =  clientcode.Box_No.HasValue ? (double)clientcode.Box_No : 0;
            double Palletno = clientcode.Pallet_No.HasValue ? (double)clientcode.Pallet_No : 0;
            string boxstr = string.Format("{0} Box(s)", boxno.ToString());
            string palletstr = string.Empty;
            if (Palletno > 0)
            {
                palletstr = string.Format("{0} Pallet(s)", Palletno.ToString());
                invoice.boxes_no.Text = string.Format("{0}\n{1}", boxstr, palletstr);
            }
            else
            {
                invoice.boxes_no.Text = string.Format("{0}", boxstr);
            }
            if (!clientcode.Weight_H_cm.HasValue  || clientcode.Weight_H_cm == 0 )
            {
                invoice.weight.Text = clientcode.Weight_Total.HasValue ? String.Format("{0} Kg weight", clientcode.Weight_Total.ToString()) : "0";

            }
            else
            {
                //10 + 9.2 Kg volume weight 40x40x60 cm
                invoice.weight.Text = String.Format("{0}+{1} Kg weight {2}x{3}x{4} cm", clientcode.Weight_Kg, clientcode.AdditionalWeight, clientcode.Weight_L_cm, clientcode.Weight_W_cm, clientcode.Weight_H_cm);
            }
            invoice.good_desc.Text = clientcode.Goods_Description;
            invoice.good_val.Text = clientcode.Goods_Value.HasValue ? string.Format("{0:#,0.##}", clientcode.Goods_Value.ToString()) : string.Empty;


            // agent details 
            invoice.office_val.Text = string.Format("{0}-{1}", agent.AgentName, countryAgent.CountryName);
            invoice.agent_val.Text = string.Format("{0}", agent.CompanyName);
            invoice.phone.Text = string.Format("{0}", agent.PhonesDisplayString.Replace("/", "")); 
            invoice.agent_address.Text = agent.Address;

            if (agent.InvoiceLanguage == "Ku")
            {

                TranslateToKurdish();
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

        public void TranslateToKurdish()
        {
            invoice.StersCompany.Text = "کۆمپانیای ستێرس\r\n";
            invoice.TransportCost.Text = "تێچووى گەیاندن\r\n";
            invoice.adress_header.Text = "";
            invoice.LeadingInTransport.Text = "پێشەنگ لە پۆستی نێودەوڵەتی\r\n";
            invoice.Branch.Text = ":لقي\r\n";
            invoice.Phone.Text = ":ژمارەى تەلەفون\r\n";
            invoice.cost_delivery.Text = "پسولەى گەیاندن و تێچووی گواستنەوە لە کوردستان وشارەکانى عێراق بۆ ئەوروپا\r\n";
            invoice.Date.Text = "بەروار:";
            invoice.Code.Text = "کۆدى وەرگرتن :\r\n";
            invoice.TransportCost_header.Text = "تێچووى گەیاندن\r\n";
            invoice.PackigingCost.Text = "پــارەى کـارتـۆنــى بـەتـــــــــــاڵ\r\n";
            invoice.AdminExportCost.Text = "گــومـرک - کـارى ئـیــــــــــدارى\r\n";
            invoice.DoorToDoorCost.Text = "تێچووى گەیاندن بۆ ناونیشانى وەرگر\r\n";
            invoice.InsuranceCost.Text = "تـێـچـووى دڵـنـیــــــــــــــــــــایـى\r\n";
            invoice.TotalCost.Text = "کــــــۆى گـشـتــــــــــــــــــــــــــى\r\n";
            invoice.PaidAmount.Text = "بــــــڕى پــــــــــــــــــــارەى دراو\r\n";
            invoice.RemainingAmount.Text = "بـڕى پــارەى داوەکـــــــــــــــــراو\r\n";
            invoice.PaidINEurope.Text = "بـرى پـارەى داوەکراو لە ئەوروپا\r\n";
            invoice.HaveInsurance.Text = "دڵنیایى ئەشیاکان کراوە :\r\n";
            invoice.Yes.Text = "بەڵێ\r\n";
            invoice.No.Text = "نەخێر\r\n";
            invoice.NOBoxes.Text = "ژمارەى کارتۆن\r\n";
            invoice.WeightKG.Text = "کێش - کغم\r\n";
            invoice.GoodsDiscription.Text = "وردەکارى ئەشیاکان\r\n";
            invoice.GoodsValue.Text = "بەهاى کاڵا\r\n";
            invoice.SenderInfo.Text = "زانیارى نێردەر";
            invoice.ReceiverInfo.Text = "- وەرگر";
            invoice.SenderName.Text = "نــاوى نێردەر\r\n";
            invoice.SenderPhone.Text = "ژمـارەى نـێــــــردەر\r\n";
            invoice.RecieverName.Text = "ناوى وەرگــر\r\n";
            invoice.RecieverPhone.Text = "ژمـارەى وەرگــــــــر\r\n";
            invoice.FullAddress.Text = "ناونیشانى وەرگر شەقام - ژمارە\r\n";
            invoice.PostalCode.Text = "ژمارەى پۆستى شار\r\n";
            invoice.City.Text = "شـــــــــــــــار\r\n";
            invoice.Country.Text = "وڵات\r\n";
            invoice.AgentInfo.Text = "زانیارییەکانى بریکار\r\n";
            invoice.Agent.Text = "بریکار\r\n";
            invoice.AgentPhone.Text = "ژ. تەلەفون\r\n";
            invoice.office.Text = "ناوى ئۆفیس\r\n ";
            invoice.AgentAddreess.Text = "ناونیشان بە تەواوى\r\n";
            invoice.PleaseReadFollowingPointsBeforeSign.Text = "تکایە ئەم خاڵانەى خوارەوە بخوێنەرەوە و دواى رەزامەندیتان لە خوارەوە واژووى بکە\r\n";

            BitmapImage bitmapImage = new BitmapImage(new Uri("pack://application:,,,/STERS;component\\Resources\\KurdishNote.png"));
            invoice.notes.Source = bitmapImage;

            invoice.MainOffice.Text = "ئۆفیسى سەرەکى کۆمپانیا لە ئەوروپا\r\n";
            invoice.Notes.Text = "تێبینى";

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
