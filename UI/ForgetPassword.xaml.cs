using StersTransport.DataAccess;
using StersTransport.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for ForgetPassword.xaml
    /// </summary>
    public partial class ForgetPassword : Window
    {

        private User user;
        UserDa userDa = new UserDa();


        public ForgetPassword()
        {
            InitializeComponent();
        }

        public ForgetPassword(User _user)
        {
            InitializeComponent();
            user = _user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_user.Text = user.UserName;
            txt_email.Text = user.Email;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // 
            // change password
            // for the user id : check if the entered verification code is equal to the one stored in the database

            // Note : For Now We Dont im[lement the time (expired) codes issue , may be in future releases will do 
            // for now we will compare with the latest code in the database...
            try
            {
                if (user.Id > 0)
                {
                    string storedverificationcode = string.Empty;
                    // get the equevalent data (verification code) from the database
                    var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
                    using (SqlConnection conn = new SqlConnection(connstr))
                    {
                        conn.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "select top 1 * from Users_Verfication_codes where UserId='"+user.Id+ "' order by TimeSent Desc";
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.HasRows)
                        {
                            rdr.Read();
                            storedverificationcode = rdr["VerificationCode"].ToString();
                            rdr.Close();
                        }
                        else
                        {
                            rdr.Close();
                        }
                    }

                    if (storedverificationcode.Length == 0)
                    {
                        WpfMessageBox.Show("", "There Was An Error Getting The Verification Code From The Database..", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }

                    if (txt_verificationcode.Text != storedverificationcode)
                    {
                        WpfMessageBox.Show("", "Invalid Verification Code", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }
                    if (passbox1.Password.Length == 0)
                    {
                        WpfMessageBox.Show("", "Please Enter The New Password", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }
                    // continue to update the password
                    string errors = string.Empty;
                    userDa.UpdateuserPassword(user, passbox1.Password, out errors);
                    if (errors.Length > 0)
                    {
                        WpfMessageBox.Show("", errors, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }

                    WpfMessageBox.Show("", GlobalData.CommonMessages.On_Update_Successful, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);

                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
           
        }

        private void txtbx1_TextChanged(object sender, TextChangedEventArgs e)
        {
            passbox1.Password = txtbx1.Text;
        }

        private void tgl_showhide_checked(object sender, RoutedEventArgs e)
        {
            txtbx1.Visibility = Visibility.Visible;
            passbox1.Visibility = Visibility.Collapsed;
            txtbx1.Text = passbox1.Password;
        }

        private void tgl_showhide_Unchecked(object sender, RoutedEventArgs e)
        {
            txtbx1.Visibility = Visibility.Collapsed;
            passbox1.Visibility = Visibility.Visible;
            passbox1.Password = txtbx1.Text;
        }

        private void button_sendcode_click(object sender, RoutedEventArgs e)
        {
            try
            { //
              // check if  email is provided and its valid...
                if (txt_email.Text.Length == 0)
                {
                    WpfMessageBox.Show("", "E-Mail Is Not Provided..", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }



                // send verification code...


                // generate 6 digits random number 
                Random r = new Random();
                int randomcodeint = r.Next(1, 999999);
                string randomcode = randomcodeint.ToString("D6");

                DateTime? serverTime = Helpers.ServerTimeHelper.get_server_Time();


                string frommail = GlobalData.CompanyData.Mail_Verification_from;
                string tomaill = txt_email.Text;
                string frommailpasswoed = Helpers.StringCipher.Decrypt( GlobalData.CompanyData.EncryptedMail_Verification_password, "KnockerzT"); 
                int Mailserverprot = GlobalData.CompanyData.Mail_Verification_port.HasValue ? (int)GlobalData.CompanyData.Mail_Verification_port : 0;
                var smtpClient = new SmtpClient(GlobalData.CompanyData.Mail_Verification_server)
                {
                    // Port = 587,
                    Port = Mailserverprot,
                    Credentials = new NetworkCredential(frommail, frommailpasswoed),
                    EnableSsl = true,
                };

                string suubject = "Verification Code To Recover Password";
                string Body = "Your Verification Code Is:" + randomcode;
                smtpClient.Send(frommail, tomaill, suubject, Body);

                // store data in the database....
                var connstr = ConfigurationManager.ConnectionStrings["StersDBSQL"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "Insert Into Users_Verfication_codes(UserId,VerificationCode,TimeSent) Values ('"+user.Id
                        +"','"+ randomcode + "',@servertime)";
                    cmd.Parameters.Add("servertime", System.Data.SqlDbType.SmallDateTime);
                    cmd.Parameters["servertime"].Value = serverTime;

                    cmd.ExecuteNonQuery();
                }

                WpfMessageBox.Show("", "Completed", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);


            }
            catch (SmtpException smtpex)
            {
                WpfMessageBox.Show("Smtp Exception", smtpex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            
            catch (Exception ex)
            {
                WpfMessageBox.Show("General Exception", ex.ToString(), MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
           



        }
    }
}
