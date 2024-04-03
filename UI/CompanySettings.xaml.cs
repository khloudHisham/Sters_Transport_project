using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for CompanySettings.xaml
    /// </summary>
    public partial class CompanySettings : UserControl
    {
        public CompanySettings()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            { loadsettings(); }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }
           
        }

        private void loadsettings()
        {
          
        
            var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from Company_General_Settings";
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    rdr.Read();
                     txt_ArabicName.Text = rdr["ArabicName"].ToString();
                    txt_ArabicDiscription.Text = rdr["ArabicDiscription"].ToString();
                    txt_EnglishName.Text = rdr["EnglishName"].ToString();
                    txt_EnglishName2.Text = rdr["EnglishName2"].ToString();
                    txt_EnglishDiscription.Text = rdr["EnglishDiscription"].ToString();
                    txt_EnglishDiscription2.Text= rdr["EnglishDiscription2"].ToString();

                    txt_Tel1.Text= rdr["Tel1"].ToString();
                    txt_Tel2.Text = rdr["Tel2"].ToString();
                    txt_Email.Text = rdr["Email"].ToString();
                    txt_Website.Text = rdr["Website"].ToString();
                    txt_Mail_Verification_from.Text = rdr["Mail_Verification_from"].ToString();
                    txt_Mail_Verification_server.Text = rdr["Mail_Verification_server"].ToString();
                    string v1=  rdr["Mail_Verification_password"].ToString();
                    string v2 = Helpers.StringCipher.Decrypt(rdr["Mail_Verification_password"].ToString(), "KnockerzT");
                    txt_Mail_Verification_password.Password = Helpers.StringCipher.Decrypt(rdr["Mail_Verification_password"].ToString(), "KnockerzT");
                   
                    if (rdr["Mail_Verification_port"] != DBNull.Value)
                    { txt_Mail_Verification_port.Text = rdr["Mail_Verification_port"].ToString(); }
                    else
                    { txt_Mail_Verification_port.Text=string.Empty; }


                    // image 
                    if (rdr["Logo2_Stars"] != DBNull.Value)
                    {
                        byte[] imgdata = (byte[])rdr["Logo2_Stars"];
                        imgLogo.Source = Helpers.ImageHelpercs.ToBitmapImage(imgdata);
                    }
                    else
                    { imgLogo.Source = null; }

                    if (rdr["Misc_MinDiff_Total_Vol_Weight"] != DBNull.Value)
                    {
                        txt_Weight_difference.Text = rdr["Misc_MinDiff_Total_Vol_Weight"].ToString();
                    }
                    else
                    {
                        txt_Weight_difference.Text = string.Empty;
                    }
                   
                }

                rdr.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                double Misc_MinDiff_Total_Vol_Weight = 0;
                double.TryParse(txt_Weight_difference.Text, out Misc_MinDiff_Total_Vol_Weight);


                if (txt_ArabicName.Text.Trim().Length == 0)
                {
                    WpfMessageBox.Show("", "You Must Enter Arabic Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                if (txt_EnglishName.Text.Trim().Length == 0)
                {
                    WpfMessageBox.Show("", "You Must Enter English Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connstr)) 
                {

                    int? Mail_Verification_port = null;
                    int res = 0;
                    if (int.TryParse(txt_Mail_Verification_port.Text, out res))
                    {
                        Mail_Verification_port = res;
                    }

                    string encryptedpassword = Helpers.StringCipher.Encrypt(txt_Mail_Verification_password.Password, "KnockerzT");

                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "Update Company_General_Settings Set ArabicName='" + txt_ArabicName.Text.Trim()
                        + "',ArabicDiscription='" + txt_ArabicDiscription.Text.Trim()
                        + "',EnglishName='" + txt_EnglishName.Text.Trim()
                        + "',EnglishDiscription='" + txt_EnglishDiscription.Text.Trim()
                        + "',Tel1='" + txt_Tel1.Text.Trim()
                        + "',Tel2='" + txt_Tel2.Text.Trim()
                        + "',Email='" + txt_Email.Text.Trim()
                        + "',Website='" + txt_Website.Text.Trim()
                        + "',EnglishName2='" + txt_EnglishName2.Text.Trim()
                        + "',EnglishDiscription2='" + txt_EnglishDiscription2.Text.Trim()
                            + "',Mail_Verification_from='" + txt_Mail_Verification_from.Text.Trim()
                            + "',Mail_Verification_server='" + txt_Mail_Verification_server.Text.Trim()
                            + "',Mail_Verification_port='" + Mail_Verification_port
                            + "',Mail_Verification_password='" + encryptedpassword + "',Misc_MinDiff_Total_Vol_Weight='" +
                            Misc_MinDiff_Total_Vol_Weight + "'";
                    cmd.ExecuteNonQuery();
                    WpfMessageBox.Show("",GlobalData.CommonMessages.On_Update_Successful+Environment.NewLine+"Modification Will Take Effect After You Log In Again To The Application.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                }


            }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // select logog from file 
            try
            {
                var ofdlg = new OpenFileDialog()
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
                };
                if (ofdlg.ShowDialog() == true)
                {
                    // ImageSource imageSource = BitmapFromUri(new Uri(ofdlg.FileName));
                    byte[] data;
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(new Uri(ofdlg.FileName)));
                    using (MemoryStream ms = new MemoryStream())
                    {
                        encoder.Save(ms);
                        data = ms.ToArray();
                    }

                    //imgLogo.Source
                    imgLogo.Source = Helpers.ImageHelpercs.ToBitmapImage(data);
                }
            }
            catch (Exception ex)
            { WpfMessageBox.Show("",ex.Message,MessageBoxButton.OK,WpfMessageBox.MessageBoxImage.Error); }
         
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            // clear logo
            imgLogo.Source = null;
        }

        private void Hyperlink_Click_3(object sender, RoutedEventArgs e)
        {
            // save logog changes

            try
            {


               

                var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connstr))
                {
 

                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    
                    byte[] data = null;
                    if (imgLogo.Source != null)
                    {
                        PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
                        data = Helpers.ImageHelpercs.ImageSourceToBytes(bitmapEncoder, imgLogo.Source);

                    }
                    cmd.CommandText = "update Company_General_Settings set Logo2_Stars=@logo";
                    cmd.Parameters.Add("logo", System.Data.SqlDbType.VarBinary);
                    if (data == null)
                    { cmd.Parameters["logo"].Value = DBNull.Value; }
                    else
                    { cmd.Parameters["logo"].Value = data; }
                    cmd.ExecuteNonQuery();
                    WpfMessageBox.Show("", GlobalData.CommonMessages.On_Update_Successful + Environment.NewLine + "Modification Will Take Effect After You Log In Again To The Application.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                }


            }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }

        }
    }
}
