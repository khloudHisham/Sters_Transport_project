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

namespace StersTransport.Presentation
{
    public class CodeLabelPost_PRSN_2
    {
        #region data members i use
        ClientCodeDA clientCodeDA = new ClientCodeDA();
        Post_Label_A5_Size Label_A5;
        #endregion

        #region declare data members
        SolidColorBrush NotPaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#DD015B"));
        SolidColorBrush PaidColor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#23C99C"));
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

        public void generateDocument(FlowDocument flowdocument, string code)
        {
            ClientCode = clientCodeDA.GetClientCode(code);

            if (ClientCode == null)
                return;

            List<Agent> allbranches = agentDa.GetAgents();
            Branches = allbranches.Where(x => x.IsLocalCompanyBranch == true).ToList();

            long _CountryAgentId = ClientCode.CountryAgentId.HasValue ? (long)ClientCode.CountryAgentId : 0;
            countryAgent = countryDa.GetCountry(_CountryAgentId);

            long _CountryPostId = ClientCode.CountryPostId.HasValue ? (long)ClientCode.CountryPostId : 0;
            countryPost = countryDa.GetCountry(_CountryPostId);

            long _agentID = ClientCode.AgentId.HasValue ? (long)ClientCode.AgentId : 0;
            agent = agentDa.GetAgent(_agentID);

            long _branchID = ClientCode.BranchId.HasValue ? (long)ClientCode.BranchId : 0;

            branch = agentDa.GetAgent(_branchID);

            flowdocument.Blocks.Clear();

            Label_A5 = new Post_Label_A5_Size();

            // sender frame

            string from = branch.AgentName ?? "";
            Label_A5.from_txt.Text = from;

            string senderName = ClientCode.SenderName ?? "";
            Label_A5.senderName_txt.Text = senderName;

            double pln = ClientCode.Pallet_No ?? 0;
            Label_A5.myParcelQuantity_txt.Text = pln.ToString();

            double bxn = ClientCode.Box_No ?? 0;
            Label_A5.allParcelQuantity_txt.Text = bxn.ToString();

            double wkg = ClientCode.Weight_Total ?? 0;
            Label_A5.weight_txt.Text = wkg.ToString();

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
            if (!string.IsNullOrEmpty(address))
            {
                address += " / ";
            }
            if (!string.IsNullOrEmpty(ClientCode.Dep_Appar))
            {
                address += ClientCode.Dep_Appar;
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

            string countryName = Label_A5.countryName_txt.Text;

            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = new System.IO.MemoryStream(countryPost.ImgForPostLabel);
                bitmapImage.EndInit();
                // freeze
                Label_A5.countryImage_img.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading country image: " + ex.Message);
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
            //Image image2 = new Image();
            //image2.Width = 300;
            //image2.Height = 50;
            //image2.Stretch = Stretch.Fill;
            //image2.Source = 
            Label_A5.barcode_img.Source = ImageHelpercs.ConvertBitmap(barcodeBitmap); 
        }
    }
}