using StersTransport.DataAccess;
using StersTransport.ExtensionMethods;
using StersTransport.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
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

namespace StersTransport
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


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

        private ObservableCollection<User> _users;
        public ObservableCollection<User> users
        {
            get { return _users; }
            set
            {
                _users = value;
                OnPropertyChanged(new PropertyChangedEventArgs("users"));
            }
        }


        UserDa userDa = new UserDa();
        //BranchDa branchDa = new BranchDa();
        AgentDa agentDa = new AgentDa();



        private double _radiusx;
        public double radiusx
        {
            get { return _radiusx; }
            set
            {
                _radiusx = value;
                
            }
        }

        private double _radiusy;
        public double radiusy
        {
            get { return _radiusy; }
            set
            {
                _radiusy = value;
                
            }
        }

      
        public double centerx
        {
            get;set;
        }

        
        public double centery
        {
            get; set;
        }


        private Point _centerpoint;
        public Point centerpoint
        {
            get { return _centerpoint; }
            set
            {
                _centerpoint = value;
                
            }
        }

        private Agent _selectedAgent;
        public Agent selectedAgent
        {
            get { return _selectedAgent; }
            set
            {
                _selectedAgent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedAgent"));
            }
        }

        private User _selecteduser;
        public User selecteduser
        {
            get { return _selecteduser; }
            set
            {
                _selecteduser = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selecteduser"));
            }
        }



        public LoginWindow()
        {
            this.SourceInitialized += (x, y) =>
            {
                this.HideMinimizeAndMaximizeButtons();
            };
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         //   GlobalData.LoggedData.LoggedUserID = 0;
          //  GlobalData.LoggedData.LoggedBranchID = 0;

            RefreshData();


            // try to set the data from logged info ... if this is not the first log ...
            /*
            try
            {
                if (GlobalData.LoggedData.LogggedBranch != null) {
                    selectedAgent = agents.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();
                }

                if (GlobalData.LoggedData.LoggedUser != null)
                { selecteduser = users.Where(x => x.Id == GlobalData.LoggedData.LoggedUser.Id).FirstOrDefault(); }   
            }
            catch (Exception)
            { }
            */

            AssignLoggedUserAndBranch();


            cmb_officeName.Focus();
            if (selectedAgent != null && selecteduser != null)
            {
                passbox1.Focus();
            }

            //hack
            
        }

        private void AssignLoggedUserAndBranch()
        {
            
            try
            {
                int LastloggedBranchId_fromAppsettings = 0;
                if (ConfigurationManager.AppSettings["LastloggedBranchId"] != null)
                {
                    string LastloggedBranchId_Str = ConfigurationManager.AppSettings["LastloggedBranchId"];
                    int.TryParse(LastloggedBranchId_Str, out LastloggedBranchId_fromAppsettings);
                }


                int lastloggeduserIdfromAppsettings = 0;
                if (ConfigurationManager.AppSettings["LastLoggedUserId"] != null)
                {
                    string LastLoggedUserId_Str = ConfigurationManager.AppSettings["LastLoggedUserId"];
                    int.TryParse(LastLoggedUserId_Str, out lastloggeduserIdfromAppsettings);
                }
                if (LastloggedBranchId_fromAppsettings != 0 )
                {
                    selectedAgent = agents.Where(x => x.Id == LastloggedBranchId_fromAppsettings).FirstOrDefault();
                }
                if (lastloggeduserIdfromAppsettings != 0)
                {
                    selecteduser = users.Where(x => x.Id == lastloggeduserIdfromAppsettings).FirstOrDefault();
                }

               
            }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }

            



        }

        private void RefreshData()
        {
            try
            {
                agents = new ObservableCollection<Agent>(agentDa.GetAgentsWithCountries(107)); 
                users = new ObservableCollection<User>();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
  
        }
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            adjustpoints();
        }

       void adjustpoints()
        {
            centerx = window_login.Width * 0.06;
            centery = window_login.Height * 0.33;
            centerpoint = new Point(centerx, centery);

            radiusx = window_login.Width * 0.3125;
            radiusy = window_login.Height * 0.583;


            elips.Center = centerpoint;
            elips.RadiusX = radiusx;
            elips.RadiusY = radiusy;
        }

        private void window_login_StateChanged(object sender, EventArgs e)
        {
            adjustpoints();

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

        private void cmb_officeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            users = new ObservableCollection<User>();
            if (selectedbranch != null)
            {
                // users = new ObservableCollection<User>(userDa.GetUsers(selectedbranch.Id));
                users = new ObservableCollection<User>(userDa.GetActiveUsers(selectedbranch.Id));
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void PerformLogin()
        {
            // login
            try
            {
                string username = cmb_user.Text;
                var selectedbranch = cmb_officeName.SelectedItem as Agent;

                if (selectedbranch == null) { return; }
                if (string.IsNullOrEmpty(username)) { return; }

                string password = Helpers.PasswordHelper.GetSaltedPasswordHash(passbox1.Password);
                User user = userDa.IsLogInInfoValid(username, password, selectedbranch.Id);
                if (user == null)
                {
                    WpfMessageBox.Show("", "Invalid Login Information", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }

                bool userisenabled = user.UserStateLoging.HasValue ? (bool)user.UserStateLoging : false;
                if (userisenabled == false)
                {
                    WpfMessageBox.Show("", "User Is Disabled..", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Warning);
                    return;
                }


                GlobalData.LoggedData.LoggedUserID = user.Id;
                GlobalData.LoggedData.LoggedBranchID = selectedbranch.Id;


                // try to update application settings with the last logged user.
                try
                { 
                    Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    if (config.AppSettings.Settings["LastLoggedUserId"] == null)
                    {
                        config.AppSettings.Settings.Add("LastLoggedUserId", user.Id.ToString()); 
                    }
                    else 
                    { 
                        config.AppSettings.Settings["LastLoggedUserId"].Value = user.Id.ToString();
                    }
                    if (config.AppSettings.Settings["LastloggedBranchId"] == null)
                    { 
                        config.AppSettings.Settings.Add("LastloggedBranchId", selectedbranch.Id.ToString());
                    }
                    else
                    {
                        config.AppSettings.Settings["LastloggedBranchId"].Value = selectedbranch.Id.ToString();
                    }
                       


                    config.Save(ConfigurationSaveMode.Modified, true);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                catch(Exception ex001)
                {
                    WpfMessageBox.Show("","Error Saving User Login Info"+Environment.NewLine+
                        ex001.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                }

                MainWindow main = new MainWindow();
                main.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //exit
            this.Close();

            // or...
            // Application.Current.Shutdown();
        }

        private void txtbx1_TextChanged(object sender, TextChangedEventArgs e)
        {
            passbox1.Password = txtbx1.Text;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // change conenction
            UI.DBConnectionSetting dbconnectionsetting = new UI.DBConnectionSetting();
            dbconnectionsetting.ShowDialog();

        }

        private void window_login_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
                e.Handled = true;

            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var _user = cmb_user.SelectedItem as User;
            if (_user == null)
            {
                WpfMessageBox.Show("", "You  Must Selected A User From The List", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                return;
            }
            // change password click 
            UI.ChangePassword changePassword = new UI.ChangePassword(_user);
            changePassword.ShowDialog();
        }

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            var _user = cmb_user.SelectedItem as User;
            if (_user == null)
            {
                WpfMessageBox.Show("", "You  Must Selected A User From The List", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                return;
            }
            // forget password click 
            UI.ForgetPassword forgetPassword = new UI.ForgetPassword(_user);
            forgetPassword.ShowDialog();
        }
    }
}
