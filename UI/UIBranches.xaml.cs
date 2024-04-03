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
    /// Interaction logic for UIBranches.xaml
    /// </summary>
    public partial class UIBranches : UserControl, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion

        private Branch _branch;
        public Branch branch
        {
            get { return _branch; }
            set
            {
                _branch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("branch"));
            }
        }

        private DataTable _dtbranches;
        public DataTable DTBranches
        {
            get { return _dtbranches; }
            set
            {
                _dtbranches = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTBranches"));
            }
        }

        public DataView DataViewBranches { get; set; }


        private ObservableCollection<City> _cities;
        public ObservableCollection<City> cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(new PropertyChangedEventArgs("cities"));
            }
        }
        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        BranchDa branchDa = new BranchDa();

        bool windowsloaded = false;

        bool dataisloading = false;
        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();

        public UIBranches()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowsloaded)
            {
                RefreshData();
                windowsloaded = true;
            }
        }

        private void RefreshData()
        {
            cities = new ObservableCollection<City>(cityDa.GetCities(107)); // Iraq cities....
            refreshbranches();
        }

        private void refreshbranches()
        {
            DTBranches= branchDa.GetBranchesView1();
            DataViewBranches = new DataView(DTBranches);
            grd.ItemsSource = DataViewBranches;
        }


        private void LoadBranch(long Id)
        {
            try
            {

                dataisloading = true;
                branch = branchDa.GetBranch(Id);
                //
                branch.setlanguageflags();
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
            }
            finally
            {
                dataisloading = false;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            {  DataViewBranches.RowFilter = string.Empty; }
            else
            {
                string filterstring = "BranchName like '%" + searchtext + "%' Or ContactPersonName Like '%" + searchtext
                    + "%'  Or BranchCompany Like '%" + searchtext + "%'";
                DataViewBranches.RowFilter = filterstring;

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
                    LoadBranch(ID);
                }
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // new content
            onnewBranch();
        }

        private void onnewBranch()
        {
            branch = new Branch();
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // add 
            onAddBranch();
        }

        private void onAddBranch()
        {
            try
            {
                validate();
                if ( branch.HasErrors) { return; }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_Branch", "Id");
                branch.Id = max + 1;
                branch.setInvoiceLanguage();
                branch.set_PhonesDisplayString();
                string errormessage = string.Empty;

                branchDa.Addbranch(branch, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                //  NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadBranch(branch.Id);
                // refresh collection
                refreshbranches();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void validate()
        {

            branch.isvalidating = true;
            PropertyInfo[] properties = typeof(Branch).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(branch, property.GetValue(branch, null)); }
                catch (Exception) { }

            }
            branch.isvalidating = false;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // update
            onUpdateBranch();
        }

        private void onUpdateBranch()
        {
            try
            {
                if (branch == null) { return; }

                if (branch.Id == 0) { return; }


                validate();
                if (branch.HasErrors) { return; }

                bool newNODigitsIsvalid = true;
                // additional check : if there are codes associated with the branch and we enter number of digits smaller than the existing one ... error
                if (branchDa.getcodeEntries(branch).Count > 0)
                {
                    if (!branchDa.isNewNumberOfDigitsValid(branch))
                    {
                        newNODigitsIsvalid = false;
                    }
                }
                if (!newNODigitsIsvalid)
                {
                    WpfMessageBox.Show("", "Invalid Number Of Digits...", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }

                branch.setInvoiceLanguage();
                branch.set_PhonesDisplayString();

                string errormessage = string.Empty;
                branchDa.UpdateBranch(branch, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadBranch(branch.Id);
                // refresh collection
                refreshbranches();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // delete
            onDeleteBranch();
        }

        private void onDeleteBranch()
        {
            if (branch == null) { return; }
            if (branch.Id == 0) { return; }

            // check if the agent has entries in codes
            if (branchDa.getcodeEntries(branch).Count > 0)
            {
                WpfMessageBox.Show("", "Can't Delete...Branch Have Entries....", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }

            string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The Branch", branch.BranchName);
            MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (msgR == MessageBoxResult.No) { return; }

            string errormessage = string.Empty;
            branchDa.DeleteBranch(branch, out errormessage);

            if (errormessage.Length > 0)
            {
                WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }
            //  Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Delete_Successful, MessageBoxImage.Information);
            notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
            refreshbranches();
            onnewBranch();
        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                LoadBranch(ID);
            }
        }
    }
}
