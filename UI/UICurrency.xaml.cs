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
    /// Interaction logic for UICurrency.xaml
    /// </summary>
    public partial class UICurrency : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        private Currency _currency;
        public Currency currency
        {
            get { return _currency; }
            set
            {
                _currency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("currency"));
            }
        }

        private DataTable _dt_currencies;
        public DataTable dt_currencies
        {
            get { return _dt_currencies; }
            set
            {
                _dt_currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dt_currencies"));
            }
        }

        public DataView DataViewCurrencies { get; set; }


        bool windowsloaded = false;

        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();
        CurrencyDa currencyDa = new CurrencyDa();

        public UICurrency()
        {
            InitializeComponent();
        }

        private void ui_Currency_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowsloaded)
            {
                RefreshData();
                windowsloaded = true;
            }
        }
        private void RefreshData()
        {
           
            refreshcurrencies();
        }
        private void refreshcurrencies()
        {
            dt_currencies = currencyDa.GetCurrenciesView1();
            DataViewCurrencies = new DataView(dt_currencies);
            grd.ItemsSource = DataViewCurrencies;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            { DataViewCurrencies.RowFilter = string.Empty; }
            else
            {
                string filterstring = "Name like '%" + searchtext + "%' ";
                DataViewCurrencies.RowFilter = filterstring;

            }
        }

        private void LoadCurrency(long Id)
        {
            try
            {

                // dataisloading = true;
                currency = currencyDa.GetCurrency(Id);
                currency.NameIsReadOnly = true;

            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
            }
            finally
            {
                //dataisloading = false;
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
                    LoadCurrency(ID);
                }
                e.Handled = true;
            }
        }


        private void validate()
        {

            currency.isvalidating = true;
            PropertyInfo[] properties = typeof(Currency).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(currency, property.GetValue(currency, null)); }
                catch (Exception) { }

            }
            currency.isvalidating = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //new
            onnewcurency();
        }

        private void onnewcurency()
        {
            currency = new Currency();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //add
            onaddcurrency();
        }

        private void onaddcurrency()
        {
            try
            {

                validate();
                if (currency.HasErrors) { return; }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_Currency", "Id");
                currency.Id = max + 1;
                string errormessage = string.Empty;
                currencyDa.Addcurrency(currency,out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCurrency(currency.Id);
                // refresh collection
                refreshcurrencies();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //update
            onupdatecurrency();
        }

        private void onupdatecurrency()
        {


            // because of storeing name issue in client code 
            // WpfMessageBox.Show("", "Updating Currency Is Not Allowed In this Version", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Warning);

            try
            {
                if (currency == null) { return; }
                if (currency.Id == 0) { return; }

                validate();
                if (currency.HasErrors) { return; }

                string errormessage = string.Empty;
                currencyDa.UpdateCurrency(currency, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCurrency(currency.Id);
                // refresh collection
                refreshcurrencies();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //delete
            ondeletecurrency();
        }

        private void ondeletecurrency()
        {
            try
            {
                if (currency == null) { return; }
                if (currency.Id == 0) { return; }

                // check if not used...
                if (currencyDa.getcodeEntries(currency).Count > 0)
                {
                    WpfMessageBox.Show("", "Can't Delete...Currency Have code Entries....", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }


                string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The Currency", currency.Name );
                MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (msgR == MessageBoxResult.No) { return; }

                string errormessage = string.Empty;
                currencyDa.DeleteCurrency(currency,out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // NotifierShow.show_notificationMessage(CommonMessages.On_Delete_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);

                // refresh collection
                refreshcurrencies();

                onnewcurency();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                LoadCurrency(ID);
            }
        }
    }
}
