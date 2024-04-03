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
using System.IO;
using StersTransport.Enumerations;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UICity.xaml
    /// </summary>
    public partial class UICity : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion



        private City _city;
        public City city
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(new PropertyChangedEventArgs("city"));
            }
        }

        private DataTable _dt_cities;
        public DataTable dt_cities
        {
            get { return _dt_cities; }
            set
            {
                _dt_cities = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dt_cities"));
            }
        }

        public DataView DataViewCities { get; set; }

        private ObservableCollection<Country> _countries;
        public ObservableCollection<Country> countries
        {
            get { return _countries; }
            set
            {
                _countries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("countries"));
            }
        }


        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();

        bool windowsloaded = false;

        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();

        public UICity()
        {
            InitializeComponent();
        }

        private void ui_city_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowsloaded)
            {
                RefreshData();
                windowsloaded = true;
            }
        }

        private void RefreshData()
        {
            countries = new ObservableCollection<Country>(countryDa.GetCountries());
            refreshcities();
        }

        private void refreshcities()
        {
            dt_cities = cityDa.GetcitiesView1();
            DataViewCities = new DataView(dt_cities);
            grd.ItemsSource = DataViewCities;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            { DataViewCities.RowFilter = string.Empty; }
            else
            {
                string filterstring = "CityName like '%" + searchtext + "%' ";
                DataViewCities.RowFilter = filterstring;

            }
        }


        private void LoadCity(long Id)
        {
            try
            {

                // dataisloading = true;
                city = cityDa.GetCity(Id);

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
                    LoadCity(ID);
                }
                e.Handled = true;
            }
        }

        private void validate()
        {

            city.isvalidating = true;
            PropertyInfo[] properties = typeof(City).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(city, property.GetValue(city, null)); }
                catch (Exception) { }

            }
            city.isvalidating = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // new
            onnewcity();
        }

        private void onnewcity()
        {
            city = new City();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //add
            onaddcity();
          
        }

        private void onaddcity()
        {
            try
            {

                validate();
                if (city.HasErrors) { return; }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_City", "Id");
                city.Id = max + 1;
                string errormessage = string.Empty;
                cityDa.Addcity(city, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                // NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCity(city.Id);
                // refresh collection
                refreshcities();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // update
            onupdatecity();
        }

        private void onupdatecity()
        {
            try
            {
                if (city == null) { return; }
                
                if (city.Id == 0) { return; }


                validate();
                if (city.HasErrors) { return; }
                string errormessage = string.Empty;
                cityDa.Updatecity(city, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                // NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadCity(city.Id);
                // refresh collection
                refreshcities();

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // delete
            ondeletecity();
        }

        private void ondeletecity()
        {
            // no checking for cities for now
            if (city == null) { return; }

            if (city.Id == 0) { return; }
            string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The City", city.CityName);
            MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (msgR == MessageBoxResult.No) { return; }

            string errormessage = string.Empty;
            cityDa.Deletecity(city, out errormessage);
            if (errormessage.Length > 0)
            {
                WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }
            //  Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Delete_Successful, MessageBoxImage.Information);
            notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
            refreshcities();
            onnewcity();

        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                LoadCity(ID);
            }
        }
    }
}
