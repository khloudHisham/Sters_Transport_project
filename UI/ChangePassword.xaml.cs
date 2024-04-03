using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using StersTransport.Models;
using StersTransport.DataAccess;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {

        private User user;
        UserDa userDa = new UserDa();
         
        public ChangePassword()
        {
            InitializeComponent();
        }


        public ChangePassword(User _user)
        {
            InitializeComponent();
            user = _user;
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

        private void txtbx2_TextChanged(object sender, TextChangedEventArgs e)
        {
            passbox2.Password = txtbx2.Text;
        }

        private void tgl_showhide_checked2(object sender, RoutedEventArgs e)
        {
            txtbx2.Visibility = Visibility.Visible;
            passbox2.Visibility = Visibility.Collapsed;
            txtbx2.Text = passbox2.Password;
        }

        private void tgl_showhide_Unchecked2(object sender, RoutedEventArgs e)
        {
            txtbx2.Visibility = Visibility.Collapsed;
            passbox2.Visibility = Visibility.Visible;
            passbox2.Password = txtbx2.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txt_user.Text = user.UserName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // change password
            try
            {
                string password = Helpers.PasswordHelper.GetSaltedPasswordHash(passbox1.Password);
                // first validate if the old password is correct...
                bool iscorrect = userDa.is_password_correct(user.Id, password);
                if (!iscorrect)
                {
                    WpfMessageBox.Show("", "The Current Password For The User Is Not Correct", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // check if new password is not empty 
                if (passbox2.Password.Length == 0)
                {
                    WpfMessageBox.Show("", "Please Enter The New Password..", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                    return;
                }

                // store the new password for the seleced user
                string errors = string.Empty;
                userDa.UpdateuserPassword(user, passbox2.Password, out errors);
                if (errors.Length > 0)
                {
                    WpfMessageBox.Show("", errors, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                WpfMessageBox.Show("", GlobalData.CommonMessages.On_Update_Successful, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            

        }
    }
}
