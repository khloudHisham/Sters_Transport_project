using StersTransport.DataAccess;
using StersTransport.GlobalData;
using StersTransport.Helpers;
using StersTransport.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using ZXing;
using StersTransport.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using System.Windows.Media.Imaging;
using StersTransport.BusinessLogic;

namespace StersTransport.Presentation
{
    public class CodeLabelOffice_PRSN_2
    {
        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#65ff99"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));

        SolidColorBrush OrangeColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ffbf00"));

        ClientCodeDA clientCodeDA = new ClientCodeDA();
        AgentDa agentDa = new AgentDa();
        CountryDa countryDa = new CountryDa();
        BranchDa branchDa = new BranchDa();

        ClientCode ClientCode { get; set; }
        Country countryAgent { get; set; }
        Country countryPost { get; set; }

        Agent agent { get; set; }
        Agent branch { get; set; }

        List<Agent> Branches = new List<Agent>();

        public void generateDocument(Frame frame , string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);
            if (ClientCode == null)
                return;

            // List<Branch> allbranches = branchDa.GetBranches();
            List<Agent> allbranches = agentDa.GetAgents();
            // Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();

            long _CountryAgentId = ClientCode.CountryAgentId ?? 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);

            long _CountryPostId = ClientCode.CountryPostId ?? 0;
            countryPost = countryDa.GetCountry(_CountryPostId);

            long _agentID = ClientCode.AgentId ?? 0;
            agent = agentDa.GetAgent(_agentID);

            long _branchID = ClientCode.BranchId ?? 0;

            branch = agentDa.GetAgent(_branchID);

            agent = agentDa.GetAgent(_agentID);


            /////////////////////////////////////////////////////////////////////
            // fill fields

            Office_Label_A6_Size officeLabel = new Office_Label_A6_Size();

            officeLabel.sender_txt.Text = ClientCode.SenderName ?? "";
            officeLabel.receiver_txt.Text = ClientCode.ReceiverName ?? "";
            officeLabel.phone_txt.Text = ClientCode.Receiver_Tel ?? "";
            officeLabel.itemDetails_txt.Text = ClientCode.Goods_Description ?? "";
            

            //officeLabel.firstNumber_txt.Text = ClientCode.Pallet_No.ToString() ?? "0";
            //officeLabel.secondNumber_txt.Text = ClientCode.Box_No.ToString() ?? "0";

            double pln = ClientCode.Pallet_No ?? 0;

            double bxn = ClientCode.Box_No ?? 0;
            string boxstr = string.Format("{0} Box(s)", bxn.ToString());
            string palletstr = string.Empty;

            double totalunits = bxn + pln;

            if (pln > 0)
            {
                palletstr = string.Format("{0} Pallet(s)", pln.ToString());
                //officeLabel.allParcelQuantity_txt.Text = string.Format("{0}+{1}", boxstr, palletstr);
                officeLabel.allParcelQuantity_txt.Text = string.Format("{0} Units", totalunits);

            }
            else
            {
                //officeLabel.allParcelQuantity_txt.Text = string.Format("{0}", boxstr);
                officeLabel.allParcelQuantity_txt.Text = string.Format("{0} Units", totalunits);


            }



            officeLabel.weight_txt.Text = ClientCode.Weight_Total.ToString() ?? "0";

            if (!ClientCode.AdditionalWeight.HasValue || ClientCode.AdditionalWeight == 0 )
            {
                officeLabel.weight_txt.Text = ClientCode.Weight_Total.HasValue ? $"{ClientCode.Weight_Total.ToString()} KG Weight" : "0";

            }
            else
            {
                officeLabel.weight_txt.Text = String.Format("{0}+{1} KG Weight", ClientCode.Weight_Kg, ClientCode.AdditionalWeight);
            }



            string cntrystr = string.Empty;
            if (countryAgent != null)
            {
                cntrystr = countryAgent.CountryName;
            }

            officeLabel.country_txt.Text = cntrystr ?? "";

            string citystr = string.Empty;
            if (agent != null)
            {
                citystr = agent.AgentName;
            }

            officeLabel.city_txt.Text = citystr;

            officeLabel.code_txt.Text = ClientCode.Code ?? "";

            //officeLabel.office_txt.Text = branchDa.GetBranch(branch.Id).BranchName ?? "";
            officeLabel.office_txt.Text = agent.AgentName;


            bool haveinsurance = ClientCode.Have_Insurance.HasValue ? (bool)ClientCode.Have_Insurance : false;
        

            double EuropeToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;

            if (EuropeToPay > 0)
            {
                if (countryAgent.continent == StaticData.Continent_Europe)
                {
                    officeLabel.allPaid_txt.Text = "PAY IN " + StaticData.Continent_Europe;
                }
                else
                {
                    officeLabel.allPaid_txt.Text = "PAY IN " + countryAgent.CountryName;
                }

                officeLabel.allPaid_txt.Background = NotPaidColor;
            }
            else
            {
                officeLabel.allPaid_txt.Text = "ALL PAID";
                officeLabel.allPaid_txt.Background = OrangeColor;
            }

            if (haveinsurance == true)
            {
                officeLabel.insurance_txt.Text = "INSURANCE YES";
                officeLabel.insurance_txt.Background = PaidColor;
            }

            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(countryAgent.ImgForBoxLabel);
                bitmapImage.EndInit();
                officeLabel.country_img.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading country image: " + ex.Message);
            }

          
            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;


            writer.Options = new ZXing.Common.EncodingOptions()
            {
                //Height = 50,
                PureBarcode = true

            };

            var result = writer.Write(ClientCode.Code);
            var barcodeBitmap = new System.Drawing.Bitmap(result);
            officeLabel.barcode_img.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);


            officeLabel.date_txt.Text = string.Format("{0}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            officeLabel.time_txt.Text = string.Format("{0}", DateTime.Now.ToString("t"));

            //Frame frame = new Frame();
            frame.Content = officeLabel;
            //frame.Margin = new Thickness(0, 0, 0, 100);
            //panel.Children.Add(frame);
        }
    }
}
