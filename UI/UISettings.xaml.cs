using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
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
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.Win32;
using StersTransport.DataAccess;
using StersTransport.Models;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UISettings.xaml
    /// </summary>
    public partial class UISettings : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        private ObservableCollection<Agent> _branches;
        public ObservableCollection<Agent> branches
        {
            get { return _branches; }
            set
            {
                _branches = value;
                OnPropertyChanged(new PropertyChangedEventArgs("branches"));
            }
        }
        private Agent _selectedBranch;
        public Agent selectedBranch
        {
            get { return _selectedBranch; }
            set
            {
                _selectedBranch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedBranch"));
            }
        }


        private string _office_data;
        
        public string officedata
        {
            get { return _office_data; }
            set {
                _office_data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("officedata"));
            }
        }

        AgentDa agentDa = new AgentDa();
        bool windowisloaded;
        public UISettings()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtservername.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Server Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtdbname.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Database Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // 
                if (string.IsNullOrWhiteSpace(txtbackuppath.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Back Up Path", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }


                Server myServer = new Server(txtservername.Text);
                myServer.ConnectionContext.LoginSecure = true;
                /*
                myServer.ConnectionContext.Connect();
                ////
                //Do your work
                ////
                if (myServer.ConnectionContext.IsOpen)
                    myServer.ConnectionContext.Disconnect();

                */

                /*
                 * //Using SQL Server authentication
myServer.ConnectionContext.LoginSecure = false;
myServer.ConnectionContext.Login = "SQLLogin";
myServer.ConnectionContext.Password = "entry@2008";
                 */
                string DBName = txtdbname.Text;

                Backup bkpDBFull = new Backup();
                /* Specify whether you want to back up database or files or log */
                bkpDBFull.Action = BackupActionType.Database;
                /* Specify the name of the database to back up */
                bkpDBFull.Database = DBName;
                /* You can take backup on several media type (disk or tape), here I am
                 * using File type and storing backup on the file system */
                bkpDBFull.Devices.AddDevice(txtbackuppath.Text, DeviceType.File);
                bkpDBFull.BackupSetName = DBName+" database Backup";
                bkpDBFull.BackupSetDescription = DBName+" database - Full Backup";
                /* You can specify the expiration date for your backup data
                 * after that date backup data would not be relevant */
                //  bkpDBFull.ExpirationDate = DateTime.Today.AddDays(10);

                /* You can specify Initialize = false (default) to create a new 
                 * backup set which will be appended as last backup set on the media. You
                 * can specify Initialize = true to make the backup as first set on the
                 * medium and to overwrite any other existing backup sets if the all the
                 * backup sets have expired and specified backup set name matches with
                   * the name on the medium */
                   bkpDBFull.Initialize = false;
                /* SqlBackup method starts to take back up
                 * You can also use SqlBackupAsync method to perform the backup 
                 * operation asynchronously */
                bkpDBFull.SqlBackup(myServer);
                WpfMessageBox.Show("","Back Up Completed..",MessageBoxButton.OK,WpfMessageBox.MessageBoxImage.Information);
               // MessageBox.Show("Back Up Complete..");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }
       

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {


                if (string.IsNullOrWhiteSpace(txtservername.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Server Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(txtdbname.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Database Name", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // 
                if (string.IsNullOrWhiteSpace(txt_restorepath.Text))
                {
                    WpfMessageBox.Show("", "You Must Provide The Restore Path", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                WpfMessageBox.Show("","Please Make A Valid Back Up Of The Database Before Proceede..",MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning);
                MessageBoxResult msgr=  WpfMessageBox.Show("", "Are You Sure You Want To Proceede With The Restore Process?..", MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Warning);

                if (msgr == MessageBoxResult.No) { return; }




                Server myServer = new Server(txtservername.Text);
               
                myServer.ConnectionContext.LoginSecure = true;
                Restore restoreDB = new Restore();
                string DBName = txtdbname.Text;
                Database database = myServer.Databases[DBName];
                restoreDB.Database = DBName;
                /* Specify whether you want to restore database, files or log */
                restoreDB.Action = RestoreActionType.Database;
                restoreDB.Devices.AddDevice(txt_restorepath.Text, DeviceType.File);

                /* You can specify ReplaceDatabase = false (default) to not create a new
                 * database, the specified database must exist on SQL Server
                 * instance. If you can specify ReplaceDatabase = true to create new
                 * database image regardless of the existence of specified database with
                 * the same name */


                   restoreDB.ReplaceDatabase = true;

                /* If you have a differential or log restore after the current restore,
                 * you would need to specify NoRecovery = true, this will ensure no
                 * recovery performed and subsequent restores are allowed. It means it
                 * the database will be in a restoring state. */
                    restoreDB.NoRecovery = false;

                    myServer.KillAllProcesses(DBName);
            //    database.DatabaseOptions.UserAccess = DatabaseUserAccess.Single;
           //     database.Alter(TerminationClause.RollbackTransactionsImmediately);
                

                // Detach database
                //    myServer.DetachDatabase(DBName, false);


                /* SqlRestore method starts to restore the database
                 * You can also use SqlRestoreAsync method to perform restore 
                 * operation asynchronously */
                restoreDB.SqlRestore(myServer);
                WpfMessageBox.Show("", "Restore Completed..", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
              //  MessageBox.Show("Restore Complete..");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // select backup location
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();

                string initialname;
                initialname = DateTime.Now.Day.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Year.ToString();
                saveFileDialog.FileName = initialname;
                saveFileDialog.Filter = "Backup Files(.bak)| *.bak";
                if (saveFileDialog.ShowDialog() == true)
                {
                    txtbackuppath.Text = saveFileDialog.FileName;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
            

             
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // select backup file torestore
            var ofdlg = new OpenFileDialog();

            if (ofdlg.ShowDialog() == true)
            {
                txt_restorepath.Text = ofdlg.FileName;

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {



            if (!windowisloaded)
            {
                refreshcollections();
                windowisloaded = true;
            }


            try
            {
                selectedBranch = branches.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();
            }
            catch (Exception) { }


            // read configuaration file
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            var connstr = config.ConnectionStrings.ConnectionStrings["StersDB"].ConnectionString;
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connstr);

            //     MessageBox.Show(string.Format("server:{0},db:{1},user:{2},pass:{3},IS:{4},App{5}",builder.DataSource,builder.InitialCatalog,builder.UserID,builder.Password,builder.IntegratedSecurity.ToString(),builder.ApplicationName));

            txtservername.Text = builder.DataSource;
            txtdbname.Text = builder.InitialCatalog;
      
        }

        private void refreshcollections()
        {
            branches = new ObservableCollection<Agent>(agentDa.GetAgents());
        }

        private void cmb_officeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // get office data ....
            try
            {
                officedata = string.Empty;
                var selectedagnet = cmb_officeName.SelectedItem as Agent;
                if (selectedagnet == null) { return; }

                string currentstr = string.Format("{0} : {1}","Address",selectedagnet.Address);
                officedata += currentstr;
                officedata += Environment.NewLine;


                

                currentstr = string.Format("{0} : {1}", "Company", selectedagnet.CompanyName);
                officedata += currentstr;
                officedata += Environment.NewLine;

                currentstr = string.Format("{0} : {1}", "Phone1", selectedagnet.PhoneNo1);
                officedata += currentstr;
                officedata += Environment.NewLine;


                currentstr = string.Format("{0} : {1}", "Phone2", selectedagnet.PhoneNo2);
                officedata += currentstr;
                officedata += Environment.NewLine;


                currentstr = string.Format("{0} : {1}", "Contact Person Name", selectedagnet.ContactPersonName);
                officedata += currentstr;
                officedata += Environment.NewLine;


                currentstr = string.Format("{0} : {1}", "Email", selectedagnet.E_mail);
                officedata += currentstr;
                officedata += Environment.NewLine;

                currentstr = string.Format("{0} : {1}", "WebSite", selectedagnet.Web);
                officedata += currentstr;
                officedata += Environment.NewLine;




            }
            catch (Exception) { }

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // additional settings 
            AdditionalSettings additionalSettings = new AdditionalSettings();
            additionalSettings.ShowDialog();
        }
    }
}
