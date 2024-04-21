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

namespace StersTransport.Presentation
{
    public class CodeLabelOffice_PRSN_2
    {
        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#23C99C"));
        SolidColorBrush BorderColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#404040"));

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

        public void generateDocument(FlowDocument flowdocument, string code)
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

            flowdocument.Blocks.Clear();

            
            /////////////////////////////////////////////////////////////////////
                                // fill fields

            Office_Label_A6_Size officeLabel = new Office_Label_A6_Size();

            officeLabel.sender_txt.Text = ClientCode.SenderName ?? "";
            officeLabel.receiver_txt.Text = ClientCode.ReceiverName ?? "";
            officeLabel.phone_txt.Text = ClientCode.Receiver_Tel ?? "";
            officeLabel.itemDetails_txt.Text = ClientCode.Goods_Description ?? "";
            

            // fuck prev developer
            officeLabel.firstNumber_txt.Text = ClientCode.Pallet_No.ToString() ?? "0";
            officeLabel.secondNumber_txt.Text = ClientCode.Box_No.ToString() ?? "0";

            officeLabel.weight_txt.Text = ClientCode.Weight_Total.ToString() ?? "0";
            officeLabel.city_txt.Text = ClientCode.CityPost ?? "";
            officeLabel.country_txt.Text = countryPost.CountryName ?? "";
            officeLabel.code_txt.Text = ClientCode.Code ?? "";

            // fuck prev developer
            officeLabel.office_txt.Text = branchDa.GetBranch(branch.Id).BranchName ?? "";


            bool haveinsurance = ClientCode.Have_Insurance ?? false;
            if (haveinsurance)
            {
                officeLabel.insurance_txt.Text = " INSURANCE=YES ";
                //officeLabel.insurance_txt.Background = new SolidColorBrush(Colors.LightGreen);
                officeLabel.insurance_txt.Background = Brushes.LightGreen;
            }
            else
            {
                officeLabel.insurance_txt.Text = " INSURANCE=NO ";
                officeLabel.insurance_txt.Background = Brushes.Orange;
            }

            bool allPaid = ClientCode.IsAllPaid;
            if (allPaid == true)
            {
                officeLabel.allPaid_txt.Text = "ALL PAID";
                officeLabel.insurance_txt.Background = Brushes.LightGreen;
            }
            else
            {
                officeLabel.insurance_txt.Text = "PAY in EU";
                officeLabel.insurance_txt.Background = Brushes.Orange;
            }

            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(countryPost.ImgForBoxLabel);
                bitmapImage.EndInit();
                officeLabel.country_img.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading country image: " + ex.Message);
            }

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.CODE_128;
            writer.Options = new ZXing.Common.EncodingOptions() { PureBarcode = true };
            var barcodeBitmap = new System.Drawing.Bitmap(writer.Write(ClientCode.Code));
            officeLabel.barcode_img.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap);

            officeLabel.date_txt.Text = string.Format("{0}", DateTime.Now.Date.ToString("dd/MM/yyyy"));
            officeLabel.time_txt.Text = string.Format("{0}", DateTime.Now.ToString("t"));


        }
    }
}
