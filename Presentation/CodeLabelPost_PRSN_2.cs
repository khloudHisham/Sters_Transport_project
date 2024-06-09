using StersTransport.DataAccess;
using StersTransport.GlobalData;
using StersTransport.Helpers;
using StersTransport.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ZXing;
using System.Windows.Media.Media3D;
using StersTransport.UI;
using System.Windows.Media.Imaging;
using StersTransport.BusinessLogic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace StersTransport.Presentation
{
    public class CodeLabelPost_PRSN_2
    {
        #region data members i use
        ClientCodeDA clientCodeDA = new ClientCodeDA();
       // Post_Label_A5_Size Label_A5;
        #endregion

        #region declare data members
        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#65ff99"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));

        
        AgentDa agentDa = new AgentDa();
        CountryDa countryDa = new CountryDa();
        BranchDa branchDa = new BranchDa();

        ClientCode ClientCode { get; set; }
        Country countryAgent { get; set; }
        Country countryPost { get; set; }

        Agent agent { get; set; }
        Agent branch { get; set; }

        List<Agent> Branches = new List<Agent>();
        #endregion

        public void generateDocument(Frame panel, string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);

            if (ClientCode == null)
                return;

            List<Agent> allbranches = agentDa.GetAgents();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();

            long _CountryAgentId = ClientCode.CountryAgentId ?? 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);

            long _CountryPostId = ClientCode.CountryPostId ?? 0;
            countryPost = countryDa.GetCountry(_CountryPostId);

            long _agentID = ClientCode.AgentId ?? 0;
            agent = agentDa.GetAgent(_agentID);

            long _branchID = ClientCode.BranchId ?? 0;

            branch = agentDa.GetAgent(_branchID);

            string from = branch.AgentName ?? "";
          //  flowdocument.Blocks.Clear();
            string senderName = ClientCode.SenderName ?? "";
            double pln = ClientCode.Pallet_No ?? 0;

            double bxn = ClientCode.Box_No ?? 0;
            string boxstr = string.Format("{0} Box(s)", bxn.ToString());
            string palletstr = string.Empty;


                // sender frame

               //Grid grid = new Grid();
               double boxcount = bxn + pln;

           
           
                var  Label_A5 = new Post_Label_A5_Size();

                Label_A5.from_txt.Text = from;

                Label_A5.senderName_txt.Text = senderName;

                //Label_A5.myParcelQuantity_txt.Text = (c + 1).ToString();

                if (pln > 0)
                {
                    palletstr = string.Format("{0} Pallet(s)", pln.ToString());
                    Label_A5.allParcelQuantity_txt.Text = string.Format("{0}+{1}", boxstr, palletstr);
                }
                else
                {
                    Label_A5.allParcelQuantity_txt.Text = string.Format("{0}", boxstr);

                }
                //double wkg = ClientCode.Weight_Total ?? 0;

                if (!ClientCode.AdditionalWeight.HasValue || ClientCode.AdditionalWeight == 0)
                {
                    Label_A5.weight_txt.Text = ClientCode.Weight_Total.HasValue ? ClientCode.Weight_Total.ToString() : "0";

                }
                else
                {
                    Label_A5.weight_txt.Text = String.Format("{0}+{1} KG volume weight", ClientCode.Weight_Kg, ClientCode.AdditionalWeight, ClientCode.Weight_L_cm, ClientCode.Weight_W_cm, ClientCode.Weight_H_cm);
                }


                // receiver frame

                string receiverName = ClientCode.ReceiverName ?? "";
                Label_A5.receiverName_txt.Text = receiverName;
                string country = countryPost.CountryName ?? "";
                Label_A5.countryName_txt.Text = country;
                string city = ClientCode.CityPost ?? "";
                Label_A5.cityName_txt.Text = city;

                string receiver_phone = ClientCode.Receiver_Tel ?? "";
                Label_A5.receiverPhoneNumber_txt.Text = receiver_phone;

                string address = ClientCode.Street_Name_No;
                if (!string.IsNullOrEmpty(ClientCode.Dep_Appar))
                { 
                    address = string.Format("{0} / {1}", ClientCode.Street_Name_No, ClientCode.Dep_Appar); 
                }
                else
                { 
                    address = ClientCode.Street_Name_No; 
                }


                Label_A5.address_txt.Text = address;

                string PostalCode = ClientCode.ZipCode ?? "";
                Label_A5.postalCode_txt.Text = PostalCode;

                string Code = ClientCode.Code ?? "";
                Label_A5.code_txt.Text = Code;

                string Time = DateTime.Now.ToString();  // edit this field
                Label_A5.DateTime_txt.Text = Time;

                string GoodsDesc = ClientCode.Goods_Description ?? "";
                Label_A5.goodsDescription_txt.Text = GoodsDesc;

                //string countryName = Label_A5.countryName_txt.Text;

                try
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new System.IO.MemoryStream(countryPost.ImgForPostLabel);
                    bitmapImage.EndInit();
                    Label_A5.countryImage_img.Source = bitmapImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading country image: " + ex.Message);
                }


                double EuropeToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
                if (EuropeToPay > 0)
                {
                    if (countryAgent.continent == StaticData.Continent_Europe)
                    {
                        Label_A5.isPaid_txt.Text = "PAY IN " + StaticData.Continent_Europe;
                    }
                    else
                    {
                        Label_A5.isPaid_txt.Text = "PAY IN " + countryAgent.CountryName;
                    }

                    Label_A5.isPaid_txt.Background = NotPaidColor;
                }
                else
                {
                    Label_A5.isPaid_txt.Text = "ALL PAID";
                    Label_A5.isPaid_txt.Background = PaidColor;
                }

                if (ClientCode.Have_Insurance == true)
                {
                    Label_A5.insuraned.Text = "INSURANCE = YES";
                    Label_A5.insuraned.Background = PaidColor;
                }



                BarcodeWriter writer = new BarcodeWriter();
                writer.Format = BarcodeFormat.CODE_128;


                writer.Options = new ZXing.Common.EncodingOptions()
                {
                    Height = 50,
                    //  Width = 300
                    PureBarcode = true

                };

                var result = writer.Write(ClientCode.Code);
                var barcodeBitmap = new System.Drawing.Bitmap(result);
                Label_A5.barcode_img.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);

               // Frame frame = new Frame();
                panel.Content = Label_A5;
                //frame.Margin = new Thickness(0,0,0,100);

                //panel.Children.Add(frame);
        }
    }
}