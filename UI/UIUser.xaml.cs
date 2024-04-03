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
using System.Windows.Navigation;
using System.Windows.Shapes;
using StersTransport.Models;
using StersTransport.DataAccess;
using System.Data;
using System.Collections.ObjectModel;
using System.Reflection;
using StersTransport.Helpers;
using StersTransport.GlobalData;
using System.ComponentModel;
using StersTransport.Enumerations;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIUser.xaml
    /// </summary>
    public partial class UIUser : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        private User _user;
        public User user
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(new PropertyChangedEventArgs("user"));
            }
        }

        private ObservableCollection<string> _authorizations;
        public ObservableCollection<string> Authorizations
        {
            get { return _authorizations; }
            set
            {
                _authorizations = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Authorizations"));
            }
        }


        private DataTable _dtusers;
        public DataTable DTusers
        {
            get { return _dtusers; }
            set
            {
                _dtusers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTusers"));
            }
        }

        public DataView DataviewUsers { get; set; }


        private ObservableCollection<Agent> _agents;
        public ObservableCollection<Agent> agents
        {
            get { return _agents; }
            set
            {
                _agents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("agents"));
            }
        }

        UserDa userDa = new UserDa();
        AgentDa agentDa = new AgentDa();


        bool windowsloaded = false;

        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();


        public UIUser()
        {
            InitializeComponent();
        }

        private void ui_user_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowsloaded)
            {
                RefreshData();
                windowsloaded = true;
            }
        }
        private void RefreshData()
        {

            agents = new ObservableCollection<Agent>(agentDa.GetAgents());
            Authorizations = new ObservableCollection<string>();
            Authorizations.Add("Admin"); Authorizations.Add("User");
            refreshusers();
        }
        private void refreshusers()
        {
            DTusers = userDa.GetUsersView1();
            DataviewUsers = new DataView(DTusers);
            grd.ItemsSource = DataviewUsers;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            { DataviewUsers.RowFilter = string.Empty; }
            else
            {
                string filterstring = "UserName like '%" + searchtext + "%' ";
                DataviewUsers.RowFilter = filterstring;

            }
        }

        private void grd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataRowView drv = grd.SelectedItem as DataRowView;
                if (drv != null)
                {
                    long ID = (long)drv["Id"];
                    loaduser(ID);
                }
                e.Handled = true;
            }
        }
        private void loaduser(long Id)
        {
            user = userDa.Getuser(Id);
            txtbx1.Clear();
            passbox1.Clear();
            
        }

        private void validate()
        {

            user.isvalidating = true;
            PropertyInfo[] properties = typeof(User).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(user, property.GetValue(user, null)); }
                catch (Exception) { }

            }
            user.isvalidating = false;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // new
            onNewUser();
        }

        private void onNewUser()
        {
            user = new User();
            txtbx1.Clear();
            passbox1.Clear();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // add
            onAddUser();
        }

        private void onAddUser()
        {
            try
            {
                
                validate();
                if (user.HasErrors) { return; }

                if (passbox1.Password.Length == 0)
                {
                    WpfMessageBox.Show("", "Enter Password", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_user", "Id");
                user.Id = max + 1;

                string errormessage = string.Empty;

                userDa.Adduser(user,passbox1.Password,out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                //  NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);

                // reload
                loaduser(user.Id);
                // refresh collection
                refreshusers();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // update
            onUpdateUser();
        }

        private void onUpdateUser()
        {
            try
            {
                if (user == null) { return; }
                if (user.Id == 0) { return; }

                validate();
                if (user.HasErrors) { return; }


                /*
                if (passbox1.Password.Length == 0)
                {
                    WpfMessageBox.Show("", "Enter Password", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }
                */


                string errormessage = string.Empty;
                userDa.Updateuser(user, passbox1.Password,out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);

                // reload
                loaduser(user.Id);
                // refresh collection
                refreshusers();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }


        private void onUpdateUserPassword()
        {
            try
            {
                if (user == null) { return; }
                if (user.Id == 0) { return; }
 


               
                if (passbox1.Password.Length == 0)
                {
                    WpfMessageBox.Show("", "Enter Password", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }
               


                string errormessage = string.Empty;
                userDa.UpdateuserPassword(user, passbox1.Password, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);

                // reload
                loaduser(user.Id);
                // refresh collection
                //   refreshusers();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // delete
            onDeleteUser(); 
           //   MessageBox.Show("Not Ready");



        }

        private void onDeleteUser()
        {
            try
            {
                if (user == null) { return; }
                if (user.Id == 0) { return; }

                // user must not be the current logged user 
                if (LoggedData.LoggedUserID == user.Id)
                {
                    WpfMessageBox.Show("", "Can't Delete .. User Must Log Out From The Application To Continue.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return; }


                // check if user dont have entries 
                bool haveentries= userDa.UserHaveEntries(user.Id);

                if (haveentries)
                {
                    WpfMessageBox.Show("", "Can't Delete .. User Have Entries.", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return; 
                }


                string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The User", user.UserName);
                MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (msgR == MessageBoxResult.No) { return; }


                string errormessage = string.Empty;
                userDa.DeleteUser(user, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // if no errors: show notification on delete successful
                notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
 
                // refresh collection
                refreshusers();
                onNewUser();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void tgl_showhide_checked(object sender, RoutedEventArgs e)
        {

            txtbx1.Visibility = Visibility.Visible;
            passbox1.Visibility = Visibility.Collapsed;
            txtbx1.Text = passbox1.Password;
            // txtbx1.Text = passbox1.Password;
        
        }

        private void tgl_showhide_Unchecked(object sender, RoutedEventArgs e)
        {


            txtbx1.Visibility = Visibility.Collapsed;
            passbox1.Visibility = Visibility.Visible;
            passbox1.Password = txtbx1.Text;


        }

        private void txtbx1_TextChanged(object sender, TextChangedEventArgs e)
        {
            passbox1.Password = txtbx1.Text;
        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                loaduser(ID);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // update password
            onUpdateUserPassword();
        }
    }
}
