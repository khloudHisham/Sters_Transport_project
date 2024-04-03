using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using StersTransport.Enumerations;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIAgent.xaml
    /// </summary>
    public partial class UIAgent : UserControl, INotifyPropertyChanged
    {

        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        private Agent _agent;
        public Agent agent
        {
            get { return _agent; }
            set
            {
                _agent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("agent"));
            }
        }


        private DataTable _dtagents;
        public DataTable DTAgents
        {
            get { return _dtagents; }
            set
            {
                _dtagents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTAgents"));
            }
        }

        public DataView DataViewAgents { get; set; }


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

        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        AgentDa AgentDa = new AgentDa();
        CurrencyDa currencyDa = new CurrencyDa();

        bool windowsloaded = false;

        bool dataisloading = false;
        bool countryupdated = false;
        bool cityupdated = false;
        bool currencyupdated = false;

        DatabaseOperationHelper dbOperationhelper = new DatabaseOperationHelper();
        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();
        public UIAgent()
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
            
            countries = new ObservableCollection<Country>(countryDa.GetCountries());
             
            cities = new ObservableCollection<City>(cityDa.GetCities());

            currencies = new ObservableCollection<Currency>(currencyDa.GetCurrencies());
            refreshagents();
        }

        private void refreshagents()
        {
            DTAgents= AgentDa.GetAgentsView1();
            DataViewAgents = new DataView(DTAgents);
            grd.ItemsSource = DataViewAgents;
        }

        private void LoadAgent(long Id)
        {
            try
            {

                dataisloading = true;
                countryupdated = false;
                cityupdated = false; currencyupdated = false;
                 Agent p_agent = AgentDa.GetAgent(Id);

                //populate comboboxes
                cities = new ObservableCollection<City>();
                if (p_agent.CountryId.HasValue)
                {
                    cities = new ObservableCollection<City>(cityDa.GetCities((long)p_agent.CountryId));
                }


                agent = p_agent;
                agent.setlanguageflags();
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


        private void validate()
        {
            agent.isvalidating = true;
           PropertyInfo[] properties = typeof(Agent).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(agent, property.GetValue(agent, null)); }
                catch (Exception) { }

            }
            agent.isvalidating = false;

        }

        private void ComboBox_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            // combobountry sourceupdated
            if (!dataisloading)
            {
                countryupdated = true;
            }
        }

        private void ComboBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // combo country lostkeyboard focus
            if (countryupdated)
            {
                cities = new ObservableCollection<City>();
                var selectedcountry = (sender as ComboBox).SelectedItem as Country;
                if (selectedcountry != null)
                {
                    cities = new ObservableCollection<City>(cityDa.GetCities(selectedcountry.Id));

                    // set the currency .. (to be tested...)
                    agent.CurrencyId = selectedcountry.CurrencyId;
                }



                countryupdated = false;
            }
        }

        private void ComboBox_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {
            // combo city sourceupdated

        }

        private void ComboBox_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            // combocity lostfocus
        }

        private void grd_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DataRowView drv = grd.SelectedItem as DataRowView;
                if (drv != null)
                {
                    long ID = (long)drv["Id"];
                    LoadAgent(ID);
                }
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            On_newAgent();
        }

        private void On_newAgent()
        {
            agent = new Agent();
        }

        private void ComboBox_SourceUpdated_2(object sender, DataTransferEventArgs e)
        {
            // currency source updated
            if (dataisloading) { return; };
            currencyupdated = true;
        }

        private void ComboBox_LostKeyboardFocus_2(object sender, KeyboardFocusChangedEventArgs e)
        {
            // currency lost focus
            if (currencyupdated)
            {
                // set the right eq for the selected currency
                var selected_currency= (sender as ComboBox).SelectedItem as Currency;
                if (selected_currency != null)
                {
                    // Note : the currency table should have an eq feild.....
                   // agent.CurrencyAgainst1IraqDinar = selected_currency.
                }
                currencyupdated = false;
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // add 
            onAddAgent();
        }

        private void onAddAgent()
        {
            try
            {
                validate();
                if (agent.HasErrors) { return; }

                // generate new ID
                long max = dbOperationhelper.getMaxLongValueFromTable("tbl_Agent", "Id");
                agent.Id = max + 1;

                agent.setInvoiceLanguage();
                agent.set_PhonesDisplayString();
                string errormessage = string.Empty;
                AgentDa.Addagent(agent, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                //  NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadAgent(agent.Id);
                // refresh collection
                refreshagents();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // update
            onUpdateAgent();
        }

        private void onUpdateAgent()
        {
            try
            {
                if (agent == null) { return; }
                if (agent.Id == 0) { return; }
                validate();
                if (agent.HasErrors) { return; }

                bool newNODigitsIsvalid = true;
                // additional check : if there are codes associated with the branch and we enter number of digits smaller than the existing one ... error
                if (AgentDa.getcodeEntries_as_sender(agent).Count > 0)
                {
                    if (!AgentDa.isNewNumberOfDigitsValid(agent))
                    {
                        newNODigitsIsvalid = false;
                    }
                }
                if (!newNODigitsIsvalid)
                {
                    WpfMessageBox.Show("", "Invalid Number Of Digits...", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }


                agent.setInvoiceLanguage();
                agent.set_PhonesDisplayString();
                string errormessage = string.Empty;
                AgentDa.UpdateAgent(agent,out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                //  NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload
                LoadAgent(agent.Id);
                // refresh collection
                refreshagents();
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // delete
            onDeleteAgent();
        }

        private void onDeleteAgent()
        {
            if (agent == null) { return; }
            if (agent.Id == 0) { return; }

            // check if the agent has entries in codes
            if (AgentDa.getcodeEntries_as_sender_or_receiver(agent).Count > 0) 
            {
                WpfMessageBox.Show("", "Can't Delete...Agent Have Entries....", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }

            string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The Agent", agent.AgentName);
            MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
            if (msgR == MessageBoxResult.No) { return; }

            // warn if agent has prices 
            Agent_PricesDa agent_PricesDa = new Agent_PricesDa();
              var destinationprices=  agent_PricesDa.GetAgent_PricesBy_For_DestinationAgent(agent.Id);
            if (destinationprices.Count > 0)
            {
                MessageBoxResult msgR2 = WpfMessageBox.Show("", "This Agent Has Prices Associated With Other Agent(s) Are You Sure You Want To Proceede With The Delete?", MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (msgR2 == MessageBoxResult.No) { return; }
            }

            string errormessage = string.Empty;
            AgentDa.DeleteAgent(agent,out errormessage);
            if (errormessage.Length > 0)
            {
                WpfMessageBox.Show("",errormessage,MessageBoxButton.OK,WpfMessageBox.MessageBoxImage.Error);
                return;
            }
            //   Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Delete_Successful, MessageBoxImage.Information);
            notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
            refreshagents();
            On_newAgent();

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchtext = (sender as TextBox).Text;
            if (string.IsNullOrEmpty(searchtext))
            { DataViewAgents.RowFilter = string.Empty; }
            else
            {
                string filterstring = "AgentName like '%" + searchtext + "%' Or ContactPersonName Like '%" + searchtext
                    + "%'  Or CompanyName Like '%" + searchtext + "%'";
                DataViewAgents.RowFilter = filterstring;

            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // edit prices
            if (agent != null)
            {
                if (agent.Id == 0) { return; }

                AgentPricesWindow agentPricesWindow = new AgentPricesWindow();
                agentPricesWindow.AgentID = agent.Id;
                agentPricesWindow.ShowDialog();
            }
        }

        private void grd_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            DataRowView drv = grd.SelectedItem as DataRowView;
            if (drv != null)
            {
                long ID = (long)drv["Id"];
                LoadAgent(ID);
            }
        }
    }
}
