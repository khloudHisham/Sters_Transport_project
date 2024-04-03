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
using Microsoft.Win32;
using System.IO;
using StersTransport.Enumerations;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UICountry.xaml
    /// </summary>
    public partial class UICountry : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        private Country _country;
        public Country country
        {
            get { return _country; }
            set
            {
                _country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("country"));
            }
        }

        private DataTable _dt_countries;
        public DataTable dt_countries
        {
            get { return _dt_countries; }
            set
            {
                _dt_countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dt_countries"));
            }
        }

        public DataView DataViewCountries { get; set; }

        private ObservableCollection<Currency> _currencies;
        public ObservableCollection<Currency> currencies
        {
            get { return _currencies; }
            set
            {
                _currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("currencies"));
            }
        }


        private List<string> _continents;
        public List<string> continents
        {
            get { return _continents; }
            set { _continents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("continents"));
            }
        }

        CountryDa countryDa = new CountryDa();
        CurrencyDa currencyDa = new CurrencyDa();

        bool windowsloaded = false;
        bool dataisloading = false;
        bool currencyupdated = false;


        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();
        public UICountry()
        {
            InitializeComponent();
            continents = StaticData.continents;
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
            currencies = new ObservableCollection<Currency>(currencyDa.GetCurrencies());
            refreshcountries();
        }
        private void refreshcountries()
        {

            dt_countries = countryDa.GetCountriesView1();
            DataViewCountries = new DataView(dt_countries);
             grd.ItemsSource = DataViewCountries;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            { DataViewCountries.RowFilter = string.Empty; }
            else
            {
                string filterstring = "CountryName like '%" + searchtext + "%' ";
                DataViewCountries.RowFilter = filterstring;

            }
        }


        private void LoadCountry(long Id)
        {
            try
            {

                dataisloading = true;
                country = countryDa.GetCountry(Id);
        
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

        private void grd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataRowView drv = grd.SelectedItem as DataRowView;
                if (drv != null)
                {
                    long ID = (long)drv["Id"];
                    LoadCountry(ID);
                }
                e.Handled = true;
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            // select flag box
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

                country.ImgForBoxLabel = data;

            }
        }

        private BitmapImage BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(source.AbsoluteUri);
            bitmap.EndInit();
            return bitmap;
        }
         

        private void Hyperlink_Click_1(object sender, RoutedEventArgs e)
        {
            // select flag post
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

                country.ImgForPostLabel = data;

            }
        }


        private void validate()
        {

            country.isvalidating = true;
            PropertyInfo[] properties = typeof(Country).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(country, property.GetValue(country, null)); }
                catch (Exception) { }

            }
            country.isvalidating = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // new
            onnewCountry();
        }

        private void onnewCountry()
        {
            country = new Country();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //add
            onAddCountry();
        }

        private void onAddCountry()
        {
            try
            {
                validate();
                if (country.HasErrors) { return; }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_Country", "Id");
                country.Id = max + 1;
                string errormessage = string.Empty;
                countryDa.Addcountry(country, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCountry(country.Id);
                // refresh collection
                refreshcountries();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //update
            onUpdateCountry();
        }

        private void onUpdateCountry()
        {
            try
            {
                if (country == null) { return; }
                if (country.Id == 0) { return; }


                validate();
                if (country.HasErrors) { return; }

                string errormessage = string.Empty;
                countryDa.UpdateCountry(country, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCountry(country.Id);
                // refresh collection
                refreshcountries();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // delete
            onDeleteCountry();
        }

        private void onDeleteCountry()
        {
            if (country == null) { return; }
            if (country.Id == 0) { return; }

            // check if the agent has entries in codes
            if ( countryDa.getcodeEntries(country).Count > 0)
            {
                WpfMessageBox.Show("", "Can't Delete...Country Have code Entries....", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }
            if (countryDa.getcitiesentries(country).Count > 0)
            {
                WpfMessageBox.Show("", "Can't Delete...Country Have cities Entries....", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }

            string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The Country", country.CountryName);
            MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (msgR == MessageBoxResult.No) { return; }

            string errormessage = string.Empty;
            countryDa.DeleteCountry(country, out errormessage);
            if (errormessage.Length > 0)
            {
                WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }

            //  Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Delete_Successful, MessageBoxImage.Information);
            notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
            refreshcountries();
            onnewCountry();
        }

        private void Hyperlink_Click_2(object sender, RoutedEventArgs e)
        {
            country.ImgForBoxLabel = null;
        }

        private void Hyperlink_Click_3(object sender, RoutedEventArgs e)
        {
            // clear flag post
            country.ImgForPostLabel = null;
        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                LoadCountry(ID);
            }
        }
    }
}
