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
using StersTransport.BusinessLogic;
using StersTransport.DataAccess;
using System.Collections.ObjectModel;
using StersTransport.GlobalData;
using System.Reflection;
using StersTransport.Enumerations;
using System.Windows.Media.Animation;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for Send.xaml
    /// </summary>
    public partial class Send : UserControl, INotifyPropertyChanged
    {
        //INotifyPropertyChanged  implementation 
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion

        #region Properties and Memebers
        private ClientCode _clientcode;
        public ClientCode ClientCode
        {
            get { return _clientcode; }
            set
            {
                _clientcode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClientCode"));
            }
        }


        private ObservableCollection<ClientCode> _clientcodes;
        public ObservableCollection<ClientCode> Clientcodes 
        {
            get { return _clientcodes; }
            set { _clientcodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Clientcodes"));
            }
        }


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

        private ObservableCollection<string> _cities;
        public ObservableCollection<string> cities
        {
            get { return _cities; }
            set
            {
                _cities = value;
                OnPropertyChanged(new PropertyChangedEventArgs("cities"));
            }
        }


        private ObservableCollection<Currency> _currencies;
        public ObservableCollection<Currency> Currencies
        {
            get { return _currencies; }
            set
            {
                _currencies = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currencies"));
            }
        }


        private ObservableCollection<Country> _agentcountries;
        public ObservableCollection<Country> Agentcountries
        {
            get { return _agentcountries; }
            set
            {
                _agentcountries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Agentcountries"));
            }
        }

        private ObservableCollection<Agent> _cityagentsoffices;
        public ObservableCollection<Agent> cityAgentsoffices
        {
            get { return _cityagentsoffices; }
            set
            {
                _cityagentsoffices = value;
                OnPropertyChanged(new PropertyChangedEventArgs("cityAgentsoffices"));
            }
        }

        private ObservableCollection<double?> _vol_factors;
        public ObservableCollection<double?> vol_factors
        {
            get { return _vol_factors; }
            set
            {
                _vol_factors = value;
                OnPropertyChanged(new PropertyChangedEventArgs("vol_factors"));
            }
        }



        private double _lastShipmentNo;
        public double lastShipmentNO
        {
            get { return _lastShipmentNo; }
            set
            {
                _lastShipmentNo = value;
                OnPropertyChanged(new PropertyChangedEventArgs("lastShipmentNO"));
            }
        }

        public double ZoomFactor
        {
            get
            {
                return (double)GetValue(ZoomFactorProperty);
            }
            set { SetValue(ZoomFactorProperty, value); }
        }


        private double _headerFontSize;
        public double HeaderFontSize
        {
            get { return _headerFontSize; }
            set
            {
                _headerFontSize = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HeaderFontSize"));
            }
        }

        private double _headerFontSize2;
        public double HeaderFontSize2
        {
            get { return _headerFontSize2; }
            set
            {
                _headerFontSize2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HeaderFontSize2"));
            }
        }


        public bool isCheckPaidChecked
        {
            get;set;
        }


        #endregion

        #region dependency properties

        public static readonly DependencyProperty
            ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor",
            typeof(double), typeof(Send),
            new PropertyMetadata(1.0));

        #endregion

        #region Variables
        ClientCodeDA ClientCodeDA = new ClientCodeDA();
        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        AgentDa agentDa = new AgentDa();
        CurrencyDa currencyDa = new CurrencyDa();
        Agent_PricesDa agent_PricesDa = new Agent_PricesDa();

        ClientCodeBL clientCodeBL;


        Helpers.NotificationHelper notificationHelper = new Helpers.NotificationHelper();
        #endregion

        #region enums
        private windowStatesenum _windowsstate;
        public windowStatesenum windowsstate
        {
            get { return _windowsstate; }
            set
            {
                _windowsstate = value;
                 setActionButtonsEnabledStates();
                 setdeletepathfill();
            }
        }


        #endregion

        #region SpecialVariables
        private const double DEFAULT_INSURANCE_PERC = 6.0;
        bool windowsloaded = false;
        bool dataisloading;// this flag is setteed to true when loading the client code object and thus setting its properties -
        //- and by setting its properties we want to freeze setting other properties automatically for some sitiuations
        // the following variables is related to spurce updated of the boubd proerty to a control 
        // this can be done in other way (like using interactions and commands ....)



        bool boxnoupdated;
        bool weightupdated;
        bool additionalweightupdated;
        bool widthupdated;
        bool lengthupdated;
        bool heightupdated;
        bool factorupdated;
        bool shipmentnoupdated;

        bool countryupdated;
        bool agentupdated;
        bool zipcodeupdated;
        bool citypostupdated;

        bool customecostupdated;
        bool boxpackingsourceupdated;
        bool exportdoccostupdated;
        bool discountupdated;
        bool paidamountupdated;

        bool insurancepercentageupdated;
        bool goodsvalueupdated;


        bool DataIsSetting;
        public bool SearchCodeconfirmed = false;


        object original_LabelTitleForeColor;
        object original_lightmodebackground;



        public string streetpickLabel;
        public string streetpickvalue;
        #endregion

        public Send()
        {
            original_LabelTitleForeColor = Application.Current.Resources["LabelTitleForeColor"];
            _headerFontSize = 12.0;
            _headerFontSize2 = 12.0;
            InitializeComponent();
            clientCodeBL = new ClientCodeBL();
        }

        private void UiSend_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowsloaded)
            {
                RefreshData();
                Refreshcodes(new object(), new RoutedEventArgs());
                OnclearContent();
                windowsloaded = true;
            }

            AdjustHeaderFontSize();
            setdeletepathfill();
            txt_sender.Focus();

        }

        private void AdjustHeaderFontSize()
        {
            try { HeaderFontSize = txt_weight.FontSize + 2.0; HeaderFontSize2 = txt_weight.FontSize + 1.0; }
            catch (Exception) { }
           
        }

        #region Methods


        private void RefreshData()
        {
            vol_factors = new ObservableCollection<double?>();
            vol_factors.Add(5000.0);
            vol_factors.Add(6000.0);


            // sender Id types Selector Datasource 
            identityTypeSelectorUC.identityTypes = GlobalData.StaticData.IdentityTypes; // must be filled after Log in ...

            countries = new ObservableCollection<Country>(countryDa.GetCountries());
            // cities = new ObservableCollection<City>(cityDa.GetCities());
            cities = new ObservableCollection<string>(cityDa.GetCitiesNames());
            Currencies = new ObservableCollection<Currency>(currencyDa.GetCurrencies());
            Agentcountries = new ObservableCollection<Country>(countryDa.GetCountries(true));


            //exclude the logged in agent country
            long? loggedcountryid = LoggedData.LogggedBranch.CountryId.HasValue ? (long)LoggedData.LogggedBranch.CountryId : 0;
            Agentcountries = new ObservableCollection<Country>(Agentcountries.Where(x => x.Id != loggedcountryid));

        }

        private void setActionButtonsEnabledStates()
        {
            switch (windowsstate)
            {
                case windowStatesenum.insert:
                    btn_add.IsEnabled = true;
                    btn_update.IsEnabled = false;
                    btn_invoice.IsEnabled = false;
                    btn_makelabel.IsEnabled = false;
                    btn_delete.IsEnabled = false;
                    break;

                case windowStatesenum.update:
                    btn_add.IsEnabled = false;
                    btn_update.IsEnabled = true;
                    btn_delete.IsEnabled = true;
                    btn_invoice.IsEnabled = true;
                    btn_makelabel.IsEnabled = true;
                   

                    break;

                case windowStatesenum.view:
                    btn_add.IsEnabled = false;
                    btn_update.IsEnabled = false;
                    btn_delete.IsEnabled = false;
                    btn_invoice.IsEnabled = true;
                    btn_makelabel.IsEnabled = true;
                    break;

                default:
                    btn_add.IsEnabled = false;
                    btn_update.IsEnabled = false;
                    btn_delete.IsEnabled = false;
                    btn_invoice.IsEnabled = false;
                    btn_makelabel.IsEnabled = false;
                    break;
            }
        }

        private void setdeletepathfill()
        {
            switch (windowsstate)
            {
                

                case windowStatesenum.update:
                    deletepath.Fill = Brushes.Red;


                    break;

             

                default:
                    // original_LabelTitleForeColor = Application.Current.Resources["LabelTitleForeColor"];
                  //  deletepath.Fill = (Brush)original_LabelTitleForeColor; // this assigntment remove the binding .. you have to keep synchronized
                    deletepath.Fill=(Brush)Application.Current.Resources["LabelTitleForeColorUpper"];
                    break;
            }
        }


        private async void Refreshcodes(object sender, RoutedEventArgs e)
        {
            adorner1.IsAdornerVisible = true;
            await Task.Run(() => Clientcodes = new ObservableCollection<ClientCode>(ClientCodeDA.GetClientCodes(LoggedData.LogggedBranch.Id, true)));
            adorner1.IsAdornerVisible = false;
        }


        private void Populate_Post_Countries(ClientCode ccode)
        {
            // first get all countries 
            countries = new ObservableCollection<Country>(countryDa.GetCountries());
            // begin filtering
            bool _Have_Local_Post  = ccode.Have_Local_Post.HasValue ? (bool)ccode.Have_Local_Post : false;
            if (!_Have_Local_Post)
            { return; }


            // selected country 
           long _CountryAgentId= ccode.CountryAgentId.HasValue ? (long)ccode.CountryAgentId : 0;
            if (_CountryAgentId == 0)
            { return; }

            // check post domain type 
            // get agent 
           long _AgentId= ccode.AgentId.HasValue ? (long)ccode.AgentId : 0;
            if (_AgentId == 0) { return; }
            var selectedAgent = agentDa.GetAgent(_AgentId);
            if (selectedAgent == null) { return; }

            if (!selectedAgent.PostDomainID.HasValue)
            {
                // same country 
                countries = new ObservableCollection<Country>(countries.Where(x => x.Id == _CountryAgentId));
                if (!dataisloading || !DataIsSetting)
                {
                    ccode.CountryPostId = ccode.CountryAgentId;
                }
            }
            else
            {
                if ((Int16)selectedAgent.PostDomainID == (Int16)PostDomainTypes.InsideEurope)
                {
                    countries = new ObservableCollection<Country>(countries.Where(x => x.continent == StaticData.Continent_Europe));
                }
                else if ((Int16)selectedAgent.PostDomainID == (Int16)PostDomainTypes.OutsideEurope)
                {
                    countries = new ObservableCollection<Country>(countries.Where(x => x.continent != StaticData.Continent_Europe));
                }
                else if ((Int16)selectedAgent.PostDomainID == (Int16)PostDomainTypes.AUSTRALIA_NEWZEALAND)
                {
                    countries = new ObservableCollection<Country>(countries.Where(x => x.Id == 14 || x.Id == 159)); // funny huh?
                }
            }


            


        }

        private void load_code(string W_code)
        {
            try
            {
                dataisloading = true;

                boxnoupdated = false;
                weightupdated = false;
                additionalweightupdated = false;
                widthupdated = false;
                lengthupdated = false;
                heightupdated = false;
                factorupdated = false;
                shipmentnoupdated = false;
                countryupdated = false;
                agentupdated = false;
                zipcodeupdated = false;
                citypostupdated = false;
                customecostupdated = false;
                boxpackingsourceupdated = false;
                exportdoccostupdated = false;
                discountupdated = false;
                paidamountupdated = false;
                insurancepercentageupdated = false;
                goodsvalueupdated = false;

                ClientCode p_clientcode = ClientCodeDA.GetClientCodeWithBranchAndUser(W_code);
                // populate agent combobox (which has dynamic data) with proper data before binding .....
                cityAgentsoffices = new ObservableCollection<Agent>();
                if (p_clientcode.CountryAgentId.HasValue) // in deleted code the country Id dont have a value
                { cityAgentsoffices = new ObservableCollection<Agent>(agentDa.GetAgents((long)p_clientcode.CountryAgentId)); }
                // clear post city combobox items...
                cities = new ObservableCollection<string>();
                // populate the country post with proper data before binding


                Populate_Post_Countries(p_clientcode);
                ClientCode = p_clientcode;
                ClientCode.Set_Currency_Exchange_Title();
                ClientCode.Set_IndentityTypeShortcut();
                clientCodeBL.clientCode = ClientCode;
                clientCodeBL.Set_IsWeight_By_SizeChecked();
                clientCodeBL.Calculate_Weight_By_Size();


                int year = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.Year : 0;
                //    ClientCode.BranchLastShipmentNO = clientCodeBL.get_last_shipmentNO(LoggedData.LogggedBranch.Id, ClientCode.BranchCode, ClientCode.YearCode); // for validation in user-entry level.. ....
                 ClientCode.BranchLastShipmentNO = clientCodeBL.get_last_shipmentNOForSpecificYear(LoggedData.LogggedBranch.Id, year);  // for validation in user-entry level.. ....

                windowsstate = windowStatesenum.update;  // not the best solution to set here (maybe change later)....
                set_notesControl_Location();
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("",ex.Message,MessageBoxButton.OK,(WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
            }
            finally
            {
                dataisloading = false;
            }
           
        }

        private void calculatewight()
        {
            clientCodeBL.clientCode = ClientCode;
            clientCodeBL.Calculate_Weight();
        }

        private void Calculate_Weight_By_Size()
        {
            clientCodeBL.clientCode = ClientCode;
            clientCodeBL.Calculate_Weight_By_Size();
        }

        private void CalculatePrices(bool UseCalculatedPackiging_cost)
        {
            clientCodeBL.clientCode = ClientCode;
            clientCodeBL.Calculate_Prices(UseCalculatedPackiging_cost);
        }


        private void OnclearContent()
        {
            DataIsSetting = true;
            try
            {
                ClientCode = new ClientCode();
                cityAgentsoffices = new ObservableCollection<Agent>();// clear the agents....
                ClientCode.virtual_Branch = LoggedData.LogggedBranch; // must detach or set as unchanged when saving client code
                ClientCode.BranchId = LoggedData.LogggedBranch.Id;
                ClientCode.User = LoggedData.LoggedUser; // must detach or set as unchanged when saving client code
                ClientCode.UserId = LoggedData.LoggedUser.Id;
                ClientCode.Person_in_charge_Id = LoggedData.LoggedUser.Id;
                ClientCode.Have_Local_Post = false;
                ClientCode.Weight_By_Size_Is_Checked = false;
                ClientCode.Have_Insurance = false;
                ClientCode.PostDate = DateTime.Now;

                string yearprefix_out;
                string branchprefix_out;
                long num_out;
                string generatedcode = clientCodeBL.generate_new_code(LoggedData.LogggedBranch.Id, out yearprefix_out, out branchprefix_out, out num_out);
                ClientCode.YearCode = yearprefix_out;
                ClientCode.BranchCode = branchprefix_out;
                ClientCode.Num = num_out;
                ClientCode.Code = generatedcode;



                //   ClientCode.BranchLastShipmentNO = clientCodeBL.get_last_shipmentNO(LoggedData.LogggedBranch.Id,
                //     ClientCode.BranchCode, ClientCode.YearCode); // re-fetch before saving data to database...


                int year = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.Year : 0;
                  ClientCode.BranchLastShipmentNO = clientCodeBL.get_last_shipmentNOForSpecificYear(LoggedData.LogggedBranch.Id,
                    year); // re-fetch before saving data to database...

                ClientCode.Shipment_No = ClientCode.BranchLastShipmentNO;
                if (ClientCode.Shipment_No == 0)
                {
                    ClientCode.Shipment_No = 1;
                }


                windowsstate = windowStatesenum.insert;

                // set focus on sender textbox 
                txt_sender.Focus();
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            finally
            { DataIsSetting = false; }
            
           
        }

        private void OnAgentUpdated()
        {
            if (ClientCode.BranchId.HasValue == false) { return; }
            // logic / calculation for agent updated...
            // check if the new agent have local post 
            // get the selected item 
            var selectedagent = cmb_agent.SelectedItem as Agent;
            if (selectedagent != null)
            {
                if (selectedagent.HavePostService.HasValue)
                {
                    ClientCode.Have_Local_Post = (bool)selectedagent.HavePostService;
                }
                else
                {
                    ClientCode.Have_Local_Post = false;
                }

                //  load agent related data.... and set properties in client code...
                //  


                long currencyId = selectedagent.CurrencyId.HasValue ? (long)selectedagent.CurrencyId : 0;


                // get agent prices...
                List<Agent_Prices> agent_Prices = agent_PricesDa.GetAgent_Prices((long)ClientCode.BranchId);
                // get specific entry
                var selectedAgentPrice=  agent_Prices.Where(x=>x.Agent_Id_Destination == selectedagent.Id).FirstOrDefault();


                /*
                decimal currencyAgainst1IraqDinar = selectedagent.CurrencyAgainst1IraqDinar.HasValue ? (decimal)selectedagent.CurrencyAgainst1IraqDinar : 0;
                decimal priceDoorToDoorEach10kgIraqDinar = selectedagent.PriceDoorToDoorEach10kgIraqDinar.HasValue ? (decimal)selectedagent.PriceDoorToDoorEach10kgIraqDinar : 0;
                decimal priceKGIraqDinar = selectedagent.PriceKGIraqDinar.HasValue ? (decimal)selectedagent.PriceKGIraqDinar : 0;
                decimal price1to5_7KGIraqDinar = selectedagent.Price1to5_7KGIraqDinar.HasValue ? (decimal)selectedagent.Price1to5_7KGIraqDinar : 0;
                long currencyId = selectedagent.CurrencyId.HasValue ? (long)selectedagent.CurrencyId : 0;
                int commissionType = selectedagent.CommissionType.HasValue ? (int)selectedagent.CommissionType : 0;
                decimal commissionBox = selectedagent.CommissionBox.HasValue ? (decimal)selectedagent.CommissionBox : 0;
                decimal commissionKG = selectedagent.CommissionKG.HasValue ? (decimal)selectedagent.CommissionKG : 0;

                */
                decimal currencyAgainst1IraqDinar = 0;
                decimal priceDoorToDoorEach10kgIraqDinar = 0;
                decimal priceKGIraqDinar = 0;
                decimal price1to5_7KGIraqDinar = 0;
                double BoxPackigingFactor = 0;


                decimal commissionBox = 0;
                decimal commissionKG = 0;

                if (selectedAgentPrice != null)
                {
                    currencyAgainst1IraqDinar = selectedAgentPrice.CurrencyEQ.HasValue ? (decimal)selectedAgentPrice.CurrencyEQ : 0;
                    priceDoorToDoorEach10kgIraqDinar = selectedAgentPrice.PriceDoorToDoor.HasValue ? (decimal)selectedAgentPrice.PriceDoorToDoor : 0;
                    priceKGIraqDinar = selectedAgentPrice.PriceKG.HasValue ? (decimal)selectedAgentPrice.PriceKG : 0;
                    price1to5_7KGIraqDinar = selectedAgentPrice.Price1to5_7KG.HasValue ? (decimal)selectedAgentPrice.Price1to5_7KG : 0;
                    commissionBox = selectedAgentPrice.CommissionBox.HasValue ? (decimal)selectedAgentPrice.CommissionBox : 0;
                    commissionKG = selectedAgentPrice.CommissionKG.HasValue ? (decimal)selectedAgentPrice.CommissionKG : 0;
                    BoxPackigingFactor = selectedAgentPrice.BoxPackaging.HasValue ? (double)selectedAgentPrice.BoxPackaging : 0;

                }

                ClientCode.Currency_Rate_1_IQD = (double)currencyAgainst1IraqDinar;
                ClientCode.PriceDoorToDoorEach10KG = priceDoorToDoorEach10kgIraqDinar;
                ClientCode.Price_KG_IQD = (double)priceKGIraqDinar;
                ClientCode.StartPrice_1_to_7KG = (double)price1to5_7KGIraqDinar;
                ClientCode.BoxPackigingFactor = BoxPackigingFactor;

                var currencyIteminfo = Currencies.Where(x => x.Id == currencyId).FirstOrDefault();
                if (currencyIteminfo != null)
                {
                    ClientCode.Currency_Type = currencyIteminfo.Name;
                }

                ClientCode.Set_Currency_Exchange_Title();

               
            }
            else
            {
                ClientCode.Have_Local_Post = false;
                ClientCode.Currency_Rate_1_IQD = 0;
                ClientCode.PriceDoorToDoorEach10KG = 0;
                ClientCode.Price_KG_IQD = 0;
                ClientCode.StartPrice_1_to_7KG = 0;
                ClientCode.Currency_Type = string.Empty;
                ClientCode.Set_Currency_Exchange_Title();
             
            }


            // reset -clear- related values  
            // note : we handle this action here and not in the viewmodel (or model) becuase when loading data we dont need to alter other properties -
            //- unlike the check weight by size property which is not mapped  to a database feild....-
            //- thiught we can handle in model by adding another flag (is loading ) to the model and change related properties only if is loading =false-
            // - but we dont need extra complixity in the view model...
          


            Populate_Post_Countries(ClientCode);

            if (!dataisloading)
            {
                ClientCode.Street_Name_No = string.Empty;
                ClientCode.Dep_Appar = string.Empty;
                ClientCode.ZipCode = string.Empty;
                ClientCode.CityPost = string.Empty;
                
            }


            set_notesControl_Location();

           

            CalculatePrices(false);

        }

        private void set_notesControl_Location()
        {

            if (ClientCode.Have_Local_Post == false) // the readonly to the controls is setted in binding....
            {

                /*
                if (!dataisloading)
                {
                    ClientCode.Street_Name_No = string.Empty;
                    ClientCode.Dep_Appar = string.Empty;
                    ClientCode.ZipCode = string.Empty;
                    ClientCode.CityPost = string.Empty;
                    ClientCode.CountryPostId = null;
                }
                */
           

                brd_post.Visibility = Visibility.Hidden;
                Grid.SetRow(brd_insurance, 2);
                Grid.SetRowSpan(brd_insurance, 3);
                row_notes_insurance.Height = new GridLength(5, GridUnitType.Star);
                try
                {
                    //txtNotes
                    grd_items.Children.Remove(grd_notes);
                    grd_items.Children.Remove(txtNotes);

                    bool grd_notesAdded = false;
                    foreach (var chld in grd_insurance.Children)
                    {
                        if (chld is Grid)
                        {
                            var gr = chld as Grid;
                            if (gr.Name == "grd_notes")
                            { grd_notesAdded = true; }
                        }
                    }
                    if (!grd_notesAdded)
                    {
                        grd_insurance.Children.Add(grd_notes);
                        Grid.SetRow(grd_notes, 3);
                    }


                    bool txtnotesadded = false;
                    foreach (var chld in grd_insurance.Children)
                    {
                        if (chld is CustomeControls.TextBoxEx)
                        {
                            var txt = chld as CustomeControls.TextBoxEx;
                            if (txt.Name == "txtNotes")
                            { txtnotesadded = true; }
                        }
                    }
                    if (!txtnotesadded)
                    {
                        grd_insurance.Children.Add(txtNotes);
                        Grid.SetRow(txtNotes, 3);
                        Grid.SetColumn(txtNotes, 1);
                        Grid.SetColumnSpan(txtNotes, 1);
                    }
                   
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }


                row_notes_items.Height = new GridLength(0.01, GridUnitType.Star);
            }
            else // if this HavePost (local or non-local)
            {

                brd_post.Visibility = Visibility.Visible;
                Grid.SetRow(brd_insurance, 3);
                Grid.SetRowSpan(brd_insurance, 2);
                row_notes_insurance.Height = new GridLength(0.01, GridUnitType.Star);
                try
                {
                    grd_insurance.Children.Remove(grd_notes);
                    grd_insurance.Children.Remove(txtNotes);


                    // adding ....
                    bool grd_notesAdded = false;
                    foreach (var chld in grd_items.Children)
                    {
                        if (chld is Grid)
                        {
                            var gr = chld as Grid;
                            if (gr.Name == "grd_notes")
                            { grd_notesAdded = true; }
                        }
                    }
                    if (!grd_notesAdded)
                    {
                        grd_items.Children.Add(grd_notes);
                        Grid.SetRow(grd_notes, 8);
                    }
                  

                    bool txtnotesadded = false;
                    foreach (var chld in grd_items.Children)
                    {
                        if (chld is CustomeControls.TextBoxEx)
                        {
                            var txt = chld as CustomeControls.TextBoxEx;
                            if (txt.Name == "txtNotes")
                            { txtnotesadded = true; }
                        }
                    }
                    if (!txtnotesadded) { 
                        
                        grd_items.Children.Add(txtNotes);
                        Grid.SetColumnSpan(txtNotes, 4);
                        Grid.SetRow(txtNotes, 8);
                        Grid.SetColumn(txtNotes, 1);
                    }
                  
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }

                row_notes_items.Height = new GridLength(1, GridUnitType.Star);


                // set the country post the same as agent country...(not correct!!!)
               
            }
        }

        private void validate(out bool? Nisvalid_Maximum_weight_eachBox_entries)
        {


            // set the : MustValidateNotes feild
            // this is special handling and may change in future....
            // client code is not null already 
            long _selectedcountryID = ClientCode.CountryAgentId.HasValue ? (long)ClientCode.CountryAgentId : 0;
            if (_selectedcountryID == 107)  // (Iraq ID)  , again : this is temporary solution....
            {
                if (!string.IsNullOrEmpty(ClientCode.Note_Send))
                {
                    ClientCode.MustValidateNotesLength = true;
                }
                else
                {
                    ClientCode.MustValidateNotesLength = false;
                }

               
            }
            else
            {
                ClientCode.MustValidateNotesLength = false;
            }

            // 
            ClientCode.MustValidateStreetNameForDigitsAndChars = false;
            var selectedCoountryPost=  cmb_countrypost.SelectedItem as Country;
            if (selectedCoountryPost != null)
            {
                int spindex = selectedCoountryPost.Special_Index.HasValue ? (int)selectedCoountryPost.Special_Index : 0;
                if (spindex != 0)
                {
                    ClientCode.MustValidateStreetNameForDigitsAndChars = true;
                }
            }



            Nisvalid_Maximum_weight_eachBox_entries = null;
            // validation test 
            ClientCode.isvalidating = true;ClientCode.shipmentNoIsvalidating = true;

            PropertyInfo[] properties = typeof(ClientCode).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                try
                { property.SetValue(ClientCode, property.GetValue(ClientCode, null)); }
                catch (Exception) { }

            }

          

            ClientCode.shipmentNoIsvalidating = false;
            ClientCode.isvalidating = false;


            // validate maximum weight for each box issue ...
            clientCodeBL.clientCode = ClientCode;
            Nisvalid_Maximum_weight_eachBox_entries = clientCodeBL.Validate_Maximum_weight_eachBox(countries.ToList());
             
        }

        private bool validate_Is_Agent_Disabled()
        {
            bool isdisabled = false;
            clientCodeBL.clientCode = ClientCode;
            long _agentid = ClientCode.AgentId.HasValue ? (long)ClientCode.AgentId : 0;
            isdisabled = clientCodeBL.Is_Agent_Disabled(_agentid, ClientCode.Code);
            return isdisabled;
        }

        private void OnMakeInvoice()
        {
            if (ClientCode == null) { return; }

            //show label UI
            /*
            try
            {
                MainWindow mainwindow_;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.show_invoice_ui(ClientCode.Code);
                        break;
                    }
                }


            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
            */

            InvoicePreviewWindow invoicePreviewWindow = new InvoicePreviewWindow(ClientCode.Code);
            invoicePreviewWindow.ShowDialog();


        }
        private void OnDeleteCode()
        {
            DataIsSetting = true;
            try
            {
                if (ClientCode == null) { return; }


                string message = string.Format("{0} {1} ?", "Are You Sure You Want To Delete The Code", ClientCode.Code);
                MessageBoxResult msgR = WpfMessageBox.Show("", message, MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Question);
                if (msgR == MessageBoxResult.No) { return; }



                // determine if this is the last code of the branch......

                long num = ClientCode.Num.HasValue ? (long)ClientCode.Num : 0;
                double shipmentNo=  ClientCode.Shipment_No.HasValue ? (double)ClientCode.Shipment_No : 0;
                string Yearcode = ClientCode.YearCode;
                string branchCode = ClientCode.BranchCode;


                //  bool isLastCode = ClientCodeDA.IsLastCode(LoggedData.LoggedBranchID, num);

                bool isLastCode = ClientCodeDA.IsLastCodeForSpecificYear(LoggedData.LoggedBranchID, num, Yearcode, branchCode);

                if (isLastCode)
                {
                    string errormessage = string.Empty;
                    ClientCodeDA.DeleteClientCode(ClientCode, out errormessage);
                    if (errormessage.Length > 0)
                    {
                        WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }
                }
                else
                {
                    // so before setting properties to null 
                    // we must consider the following sitiuation : 
                    // if the shipment number is the joint between proceeding and previous numbers (EX : the shipment number is 2 
                    // and there is only one shipment number with the number 2 (the one being setted to null)
                    // and there is number with 3 and numbers with 1
                    // hence the outcome will be : 1 and 3 numbers and that is not correct....
                    // so we must chekc the sequence if its valid
                    // )

                    //IS_new_shipmentNumbers_Order_SequentialPerYear_SetPropertiesNullMode
                    bool issequential = clientCodeBL.IS_new_shipmentNumbers_Order_SequentialPerYear_SetPropertiesNullMode(LoggedData.LogggedBranch,
                     ClientCode);


                    if (!issequential)
                    {
                        WpfMessageBox.Show("", "Invalid Shipment Numbers Sequence...", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                        return;
                    }

                    string errormessage = string.Empty;
                    ClientCodeDA.Set_Code_Properties_ToNull(ClientCode, out errormessage);
                    if (errormessage.Length > 0)
                    {
                        WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                        return;
                    }

                }


                notificationHelper.showMainNotification(CommonMessages.On_Delete_Successful, MessagesTypes.messagestypes.info, 2);
                // Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);

                Refreshcodes(new object(), new RoutedEventArgs());
                OnclearContent();
                

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
            finally { DataIsSetting = false; }


        }
        private void OnMakeLabel()
        {
            if (ClientCode == null) { return; }

           
            bool ispost = ClientCode.Have_Local_Post.HasValue ? (bool)ClientCode.Have_Local_Post : false;
            LabelPreviewWindow labelPreviewWindow = new LabelPreviewWindow(ClientCode.Code, ispost);
            labelPreviewWindow.ShowDialog();
        }

        private void OnUpdateClientCode()
        {
            try
            {
                if (ClientCode == null) { return; }
                // validate...
                bool? Nisvalid_Maximum_weight_eachBox_entries;
                validate(out Nisvalid_Maximum_weight_eachBox_entries);


                if (ClientCode.HasErrors)
                {
                    WpfMessageBox.Show("", CommonMessages.PleaseCorrectErrorsBeforeProceede, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return; 
                }

                bool isvalid_Maximum_weight_eachBox_entries = Nisvalid_Maximum_weight_eachBox_entries.HasValue ? (bool)Nisvalid_Maximum_weight_eachBox_entries : true;

                if (!isvalid_Maximum_weight_eachBox_entries)
                {
                    WpfMessageBox.Show("", "Invalid Maximum Weight For Box Entry", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }
                // .. validate

                // validate Is Agent Disabled
                if (validate_Is_Agent_Disabled())
                {
                    WpfMessageBox.Show("", "Agent Is Disabled....Can't Proceede With The Operation", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }

                // re-assure that there is no gaps between shipment numbers 
                int year = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.Year : 0;
                // double _lastshipmentno = clientCodeBL.get_last_shipmentNO(LoggedData.LogggedBranch.Id, ClientCode.BranchCode, ClientCode.YearCode);
                double _lastshipmentno = clientCodeBL.get_last_shipmentNOForSpecificYear(LoggedData.LogggedBranch.Id, year); 
                if (ClientCode.Shipment_No > _lastshipmentno + 1)
                {
                    WpfMessageBox.Show("", "The Shipment Number You Entered Is Not Valid.",
                        MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }


                // Sequential Shipment NUmbers Issue....
                // if shipment number changed...recheck sequence....

                // or Year Changed .. apply

                if (clientCodeBL.Is_ShipmentNUmber_Changed(ClientCode.Code, (double)ClientCode.Shipment_No)||
                    clientCodeBL.Is_Year_Changed(ClientCode.Code, (int)ClientCode.PostYear))
                {
                    /*
                    bool issequential=  clientCodeBL.IS_new_shipmentNumbers_Order_Sequential(LoggedData.LogggedBranch,
                        
                        ClientCode.BranchCode,ClientCode.YearCode,
                        ClientCode, (double)ClientCode.Shipment_No);
                    */
                    bool issequential = clientCodeBL.IS_new_shipmentNumbers_Order_SequentialPerYear(LoggedData.LogggedBranch,
                       ClientCode, (double)ClientCode.Shipment_No);


                    if (!issequential)
                    {
                        WpfMessageBox.Show("", "Invalid Shipment Numbers Sequence...", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                        return;
                    }
                }



                CalculatePrices(false);

                string errormessage = string.Empty;
                ClientCodeDA.UpdateClientCode(ClientCode, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                //   Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Update_Successful, MessageBoxImage.Information);
                notificationHelper.showMainNotification(CommonMessages.On_Update_Successful, MessagesTypes.messagestypes.info, 2);
                // reload 
                load_code(ClientCode.Code);

                // refresh collection .. abetter appraoch is to modify the item updated in the colection
                  Refreshcodes(new object(), new RoutedEventArgs());
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }
      
        private void OnAddClientCode()
        {
            try
            {

                if (ClientCode == null)
                { return; }
              

                // validate...
                bool? Nisvalid_Maximum_weight_eachBox_entries;
                validate(out Nisvalid_Maximum_weight_eachBox_entries);


                 if (ClientCode.HasErrors) 
                {
                    WpfMessageBox.Show("", CommonMessages.PleaseCorrectErrorsBeforeProceede, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }

                bool isvalid_Maximum_weight_eachBox_entries = Nisvalid_Maximum_weight_eachBox_entries.HasValue ? (bool)Nisvalid_Maximum_weight_eachBox_entries : true;

                if (!isvalid_Maximum_weight_eachBox_entries)
                {
                    WpfMessageBox.Show("", "Invalid Maximum Weight For Box Entry", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }
                // .. validate


                // validate Is Agent Disabled
                if (validate_Is_Agent_Disabled())
                {
                    WpfMessageBox.Show("", "Agent Is Disabled....Can't Proceede With The Operation", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }


                CalculatePrices(false);



                // re-assure that there is no gaps between shipment numbers 
                int year = ClientCode.PostDate.HasValue ? ClientCode.PostDate.Value.Year : 0;
                //   double _lastshipmentno = clientCodeBL.get_last_shipmentNO(LoggedData.LogggedBranch.Id, ClientCode.BranchCode, ClientCode.YearCode);
                double _lastshipmentno = clientCodeBL.get_last_shipmentNOForSpecificYear(LoggedData.LogggedBranch.Id, year);
                if (ClientCode.Shipment_No > _lastshipmentno + 1)
                {
                    WpfMessageBox.Show("", "The Shipment Number You Entered Is Not Valid.",
                        MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    return;
                }

             
                bool need_to_generate_newCode = false;
                // re-check if the code is exists ask the user if he wants to re-generate the code....
                ClientCode _clientcode = ClientCodeDA.GetClientCode(ClientCode.Code);
                if (_clientcode != null)
                {
                    MessageBoxResult msgresult = WpfMessageBox.Show("","The Code You Entered Is Taken , Do You Want To Continue And Generate New Code ? ",
                        MessageBoxButton.YesNo,(WpfMessageBox.MessageBoxImage) MessageBoxImage.Warning);

                    if (msgresult == MessageBoxResult.No)
                    { return; }
                    else { need_to_generate_newCode = true; }
                }

                // re-generate code (optional)
                if (need_to_generate_newCode)
                {
                    string yearprefix_out;
                    string branchprefix_out;
                    long num_out;
                    string generatedcode = clientCodeBL.generate_new_code(LoggedData.LogggedBranch.Id, out yearprefix_out, out branchprefix_out, out num_out);
                    ClientCode.YearCode = yearprefix_out;
                    ClientCode.BranchCode = branchprefix_out;
                    ClientCode.Num = num_out;
                    ClientCode.Code = generatedcode;
                }

                string errormessage = string.Empty;
               
                ClientCodeDA.AddClientCode(ClientCode, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                //   Helpers.NotifierShow.show_notificationMessage(CommonMessages.On_Insert_Successful, MessageBoxImage.Information);


                notificationHelper.showMainNotification(CommonMessages.On_Insert_Successful, MessagesTypes.messagestypes.info, 2);
               


                // reload 
                load_code(ClientCode.Code);

                // refresh collection  .. this is quick solution another is to add the added object to the collection
                Refreshcodes(new object(), new RoutedEventArgs());
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
          



        }


        private void showCodeSearchWindow(bool showoptions)
        {

            // first check if there is a selected code 
            var selectedCodeItem = cmb_clientcode.SelectedItem as ClientCode;
            if (selectedCodeItem != null && showoptions) // show the options dialog to choose action (update , insert , view)
            {
                CodeSearchWindowSearchForDialog codeSearchWindowSearchForDialog = new CodeSearchWindowSearchForDialog(this);
                codeSearchWindowSearchForDialog.ShowDialog();
                //   windowsstate is setted from the CodeSearchWindowSearchForDialog Window Instance
                if (SearchCodeconfirmed)
                {
                    handle_Code_Search_SelectedOption(selectedCodeItem, selectedCodeItem.Code);
                   
                }

                return;
            }



            // search code click
            CodeSearchWindow codeSearchWindow = new CodeSearchWindow();
            codeSearchWindow.ShowDialog();

            // first get the client code object from seach window by its code ....

            ClientCode clientcode_SearchWindow = ClientCodeDA.GetClientCode(codeSearchWindow.searchedCode);
            if (clientcode_SearchWindow == null) { return; }
            //codeSearchWindow.searchedCode
            windowsstate = codeSearchWindow.WindowStatesenum;

            handle_Code_Search_SelectedOption(clientcode_SearchWindow, codeSearchWindow.searchedCode);//the second parameter is not necessary (we can remove it)




        }


        private void handle_Code_Search_SelectedOption(ClientCode clientcode_SearchWindow,string searchedCode)
        {
            if (windowsstate == windowStatesenum.insert) // clear content and load some data from searchwindow chosed code..
            {
                OnclearContent();
                ClientCode.SenderName = clientcode_SearchWindow.SenderName;
                ClientCode.Sender_Tel = clientcode_SearchWindow.Sender_Tel;
                ClientCode.Sender_ID = clientcode_SearchWindow.Sender_ID;
                ClientCode.ReceiverName = clientcode_SearchWindow.ReceiverName;
                ClientCode.Receiver_Tel = clientcode_SearchWindow.Receiver_Tel;

                // populate agent combobox (which has dynamic data) with proper data before binding .....
                cityAgentsoffices = new ObservableCollection<Agent>();
                if (clientcode_SearchWindow.CountryAgentId.HasValue) // in deleted code the country Id dont have a value
                { cityAgentsoffices = new ObservableCollection<Agent>(agentDa.GetAgents((long)clientcode_SearchWindow.CountryAgentId)); }

                // clear post city combobox items...
                cities = new ObservableCollection<string>();

                ClientCode.CountryAgentId = clientcode_SearchWindow.CountryAgentId;
                ClientCode.AgentId = clientcode_SearchWindow.AgentId; // must fetch the agent values (like door to door ,minimun price,....etc)
                // fetch agent data...
                OnAgentUpdated();


                ClientCode.Have_Local_Post = clientcode_SearchWindow.Have_Local_Post;
                ClientCode.CountryPostId = clientcode_SearchWindow.CountryPostId;
                ClientCode.ZipCode = clientcode_SearchWindow.ZipCode;
                ClientCode.CityPost = clientcode_SearchWindow.CityPost;
                ClientCode.Street_Name_No = clientcode_SearchWindow.Street_Name_No;
                ClientCode.Dep_Appar = clientcode_SearchWindow.Dep_Appar;

                windowsstate = windowStatesenum.insert;
                cmb_clientcode.SelectedItem = null;
            }
            else if (windowsstate == windowStatesenum.update) // load the code for editing 
            {
                load_code(searchedCode);
                windowsstate = windowStatesenum.update; // already setted in LoadCode Method
                cmb_clientcode.SelectedItem = null;
            }
            else if (windowsstate == windowStatesenum.view) // load the code but disable edit and add actions
            {
                load_code(searchedCode);
                windowsstate = windowStatesenum.view;
                cmb_clientcode.SelectedItem = null;
            }
        }


        private void checkZipcode()
        {
            // logic
            clientCodeBL.clientCode = ClientCode;
            string errormessgesoutput = string.Empty;
            List<string> matchedcitiesoutput = new List<string>();
            bool isvalid = clientCodeBL.ValidateZipCode(countries.ToList(), out errormessgesoutput, out matchedcitiesoutput);
            ClientCode.zipcodeisvalidating = true;
            ClientCode.zipcodeErrorMessage = errormessgesoutput;
            ClientCode.ZipCode = ClientCode.ZipCode;
            ClientCode.zipcodeisvalidating = false;

            // reset- cities 

            cities = new ObservableCollection<string>(matchedcitiesoutput);
            // set the selected index as first item 
            if (cities.Count > 0)
            {
                cmb_citypost.SelectedIndex = 0;
            }

            if (errormessgesoutput.Length > 0)
            {
                WpfMessageBox.Show("", errormessgesoutput, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                // optional reset ... street and address
                ClientCode.Street_Name_No = string.Empty;
                ClientCode.Dep_Appar = string.Empty;
                return;
            }

            // now show propmp to select street number 
            // logic
            // determine if this is special country that needs street picker 
            clientCodeBL.clientCode = ClientCode;
            bool needstreetpicker = clientCodeBL.need_street_picker(countries.ToList());
            if (needstreetpicker)
            {
                // another logic for getting max and min values for street number
                int minno = 0;
                int maxno = 0;
                clientCodeBL.clientCode = ClientCode;
                clientCodeBL.set_max_min_streetnumbers(out maxno, out minno);
                //    popup_streetpost.PlacementTarget = cmb_citypost;
                //  streetNOPicker.Title = string.Format("Please Enter Value Between {0} and {1} for street number", minno.ToString(), maxno.ToString());
                //   popup_streetpost.IsOpen = true;

                StreetPickWindow spw = new StreetPickWindow(this);
                streetpickLabel= string.Format("Please Enter Value Between {0} and {1} for street number", minno.ToString(), maxno.ToString());
                spw.ShowDialog();

                int enteredvalue = 0;
                Int32.TryParse(streetpickvalue, out enteredvalue);
                if (enteredvalue >= minno && enteredvalue <= maxno)
                {
                    // get street name 
                    string streetname = clientCodeBL.get_streetName();
                    ClientCode.Street_Name_No = string.Format("{0} {1}", streetname, enteredvalue.ToString());
                }
                else
                {
                    
                }


            }
            else
            {
             //   popup_streetpost.IsOpen = false;
            }

        }


        public void assignTXTRemainingColors()
        {
            // if not already changed by animation
            if (ClientCode == null)
            { return; }

            double _Remaining_IQD = ClientCode.Remaining_IQD.HasValue ? (double)ClientCode.Remaining_IQD : 0;
            //  double _TotalPaid_IQD=    ClientCode.TotalPaid_IQD.HasValue ? (double)ClientCode.TotalPaid_IQD : 0;

            if (_Remaining_IQD > 0)
            {
                ColorAnimation animation = new ColorAnimation();
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                animation.To = dcolor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                txt_Remaining_IQD.Background = new SolidColorBrush(((SolidColorBrush)original_lightmodebackground).Color);
                txt_Remaining_IQD.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                ColorAnimation animation = new ColorAnimation();
                animation.To = ((SolidColorBrush)original_lightmodebackground).Color;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                txt_Remaining_IQD.Background = new SolidColorBrush(dcolor);
                txt_Remaining_IQD.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
        }
        public void assign_TXTPaidcolors_Paid()
        {
            ColorAnimation animation = new ColorAnimation();
            Color greencolor = (Color)ColorConverter.ConvertFromString("#00BB7E");
            animation.To = greencolor;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
            txt_TotalPaid_IQD.Background = new SolidColorBrush(((SolidColorBrush)original_lightmodebackground).Color);
            txt_TotalPaid_IQD.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);

           
        }
        public void assignTXTPaidCOlors_Unpaid()
        {
            original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
            Color greencolor = (Color)ColorConverter.ConvertFromString("#00BB7E");
            ColorAnimation animation = new ColorAnimation();
            animation.To = ((SolidColorBrush)original_lightmodebackground).Color;
            animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
            txt_TotalPaid_IQD.Background = new SolidColorBrush(greencolor);
            txt_TotalPaid_IQD.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
        }

        public void assignTXTToPayColors()
        {
            if (ClientCode == null)
            { return; }


            double _EuropaToPay = ClientCode.EuropaToPay.HasValue ? (double)ClientCode.EuropaToPay : 0;
            //  double _TotalPaid_IQD=    ClientCode.TotalPaid_IQD.HasValue ? (double)ClientCode.TotalPaid_IQD : 0;

            if (_EuropaToPay > 0)
            {
                ColorAnimation animation = new ColorAnimation();
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                animation.To = dcolor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                txt_EuropaToPay.Background = new SolidColorBrush(((SolidColorBrush)original_lightmodebackground).Color);
                txt_EuropaToPay.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                ColorAnimation animation = new ColorAnimation();
                animation.To = ((SolidColorBrush)original_lightmodebackground).Color;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                txt_EuropaToPay.Background = new SolidColorBrush(dcolor);
                txt_EuropaToPay.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
        }

        public void assignTXTDiscountColors()
        {
            if (ClientCode == null)
            { return; }


            double _Discount = ClientCode.Discount_Post_Cost_Send.HasValue ? (double)ClientCode.Discount_Post_Cost_Send : 0;
            

            if (_Discount > 0)
            {
                ColorAnimation animation = new ColorAnimation();
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                animation.To = dcolor;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                txt_Discount.Background = new SolidColorBrush(((SolidColorBrush)original_lightmodebackground).Color);
                txt_Discount.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
            else
            {
                original_lightmodebackground = Application.Current.Resources["lightmodebackground"];
                Color dcolor = (Color)ColorConverter.ConvertFromString("#D24E84");
                ColorAnimation animation = new ColorAnimation();
                animation.To = ((SolidColorBrush)original_lightmodebackground).Color;
                animation.Duration = new Duration(TimeSpan.FromSeconds(0.6));
                txt_Discount.Background = new SolidColorBrush(dcolor);
                txt_Discount.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
        }

        public void assigndeletepathcolor()
        {
            setdeletepathfill();
           // deletepath.Fill = (Brush)Application.Current.Resources["LabelTitleForeColor"];
            //LabelTitleForeColor
        }

        private void SetIdentityTypeAfterSelectorCommitt()
        {

            var selectedIdentityType = identityTypeSelectorUC.lstbxContainer.SelectedItem as IdentityType;
            if (selectedIdentityType != null)
            {
                if (ClientCode != null)
                {
                    ClientCode.Sender_ID_Type = selectedIdentityType.Name;
                    ClientCode.Sender_ID_Type_Shortcut = selectedIdentityType.Shortcut;
                }
            }
            else
            {
                ClientCode.Sender_ID_Type = null;
                ClientCode.Sender_ID_Type_Shortcut = null;
            }
            popup_senderIDtypes.IsOpen = false;
            txt_senderID.Focus();

        }
        #endregion

        #region EventHandlers

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // clear content
            OnclearContent();
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            // sender ID click
            // bring up the identity selector
            var senderBtn = sender as Button;
            popup_senderIDtypes.PlacementTarget = senderBtn;
            popup_senderIDtypes.IsOpen = true;
        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            // show sender summary
            SenderSummaryWindow senderSummaryWindow = new SenderSummaryWindow();
            senderSummaryWindow.senderTel = ClientCode.Sender_Tel;
            senderSummaryWindow.ShowDialog();
        }

        private void cmb_clientcode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
 
        }
  
        private void txt_weight_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (dataisloading||DataIsSetting) { return; }
            weightupdated = true;

            calculatewight();
            CalculatePrices(false);
        }

        private void txt_weight_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
            if (weightupdated)
            {
                //calc
                calculatewight();
                CalculatePrices();
                weightupdated = false;
            }
            */
        }
    
        private void txt_additionalWeight_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            additionalweightupdated = true;
            calculatewight();
            CalculatePrices(false);
        }

        private void txt_additionalWeight_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
            if (additionalweightupdated)
            {
                calculatewight();
                CalculatePrices();
                 additionalweightupdated = false;
            }
            */
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            // check weight by size checked .. this property is not mapped and is not affected by loading data....

            // first set the volume factor to default value
            ClientCode.Weight_Vol_Factor = 5000;
            Calculate_Weight_By_Size();
            calculatewight();
            CalculatePrices(false);
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            // check weight by size unchecked
            // first reset values 
            ClientCode.Weight_H_cm = 0;
            ClientCode.Weight_L_cm = 0;
            ClientCode.Weight_W_cm = 0;
            ClientCode.Weight_BySizeValue = 0;
            ClientCode.Weight_Vol_Factor = null;
            ClientCode.AdditionalWeight = 0;
            Calculate_Weight_By_Size(); calculatewight(); CalculatePrices(false);

        }
        
        private void TextBoxEx_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            lengthupdated = true;
            // text Weight_L_cm sourceupdated

            Calculate_Weight_By_Size();

        }

        private void TextBoxEx_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            // text Weight_L_cm lost keyboard focus
            /*
            if (lengthupdated)
            { 
                Calculate_Weight_By_Size(); lengthupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            widthupdated = true;
            Calculate_Weight_By_Size();
        }

        private void TextBoxEx_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {/*
            if(widthupdated)
            {
                Calculate_Weight_By_Size(); widthupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_2(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            heightupdated = true;
            Calculate_Weight_By_Size();
        }

        private void TextBoxEx_LostKeyboardFocus_2(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
            if (heightupdated)
            { Calculate_Weight_By_Size(); heightupdated = false; }
            */
        }

        private void ComboboxEX_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            factorupdated = true;
            Calculate_Weight_By_Size();
        }

        private void ComboboxEX_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
            if (factorupdated)
            { Calculate_Weight_By_Size(); factorupdated = false; }
            */
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // clear content

        }

        private void TextBoxEx_SourceUpdated_12(object sender, DataTransferEventArgs e)
        {
            // box packing source updated
            if (dataisloading || DataIsSetting) { return; }
            boxpackingsourceupdated = true;
            CalculatePrices(false); 
        }

        private void TextBoxEx_LostKeyboardFocus_12(object sender, KeyboardFocusChangedEventArgs e)
        {
            // box packing lost focus
            /*
            if (boxpackingsourceupdated)
            {
                //calc
                CalculatePrices();
                boxpackingsourceupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_3(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            // shipment no source updated
            shipmentnoupdated = true;
        }

        private void TextBoxEx_LostKeyboardFocus_3(object sender, KeyboardFocusChangedEventArgs e)
        {
            // shipment no lost focus
            if (shipmentnoupdated)
            {
                // validate shipment no
                ClientCode.shipmentNoIsvalidating = true;
                ClientCode.Shipment_No = ClientCode.Shipment_No;
                ClientCode.shipmentNoIsvalidating = false;
                shipmentnoupdated = false;
            }
        }

        private void ComboboxEX_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
             agentupdated = true;
            if (agentupdated)
            {
                OnAgentUpdated();
                agentupdated = false;
            }
        }

        private void ComboboxEX_LostKeyboardFocus_1(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
             if (agentupdated)
            {
                OnAgentUpdated();
                agentupdated = false;
            }
             */


            // check if the agent is disabled show error message... the checking is with loaded data from client code , not querying the database..

            /*
            var selectedagent = cmb_agent.SelectedItem as Agent;
            if (selectedagent != null)
            {
                bool selectedAgentIsDisabled = selectedagent.AgentIsDisabled.HasValue ? (bool)selectedagent.AgentIsDisabled : false;
                if (selectedAgentIsDisabled)
                {
                    WpfMessageBox.Show("", "The Selected Agent Is Disabled...", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning);
                }
            }
            */


        }
        
        private void TextBoxEx_SourceUpdated_4(object sender, DataTransferEventArgs e)
        {
            // zipcode source updated
            if (dataisloading || DataIsSetting) { return; }
            zipcodeupdated = true;
        }

        private void TextBoxEx_LostKeyboardFocus_4(object sender, KeyboardFocusChangedEventArgs e)
        {
            /*
            // zipcode lost focus
            if (zipcodeupdated)
            {
                // logic
                clientCodeBL.clientCode = ClientCode;
                string errormessgesoutput = string.Empty;
                List<string> matchedcitiesoutput = new List<string>();
                bool  isvalid = clientCodeBL.ValidateZipCode(countries.ToList(), out errormessgesoutput, out matchedcitiesoutput);
                ClientCode.zipcodeisvalidating = true;
                ClientCode.zipcodeErrorMessage = errormessgesoutput;
                ClientCode.ZipCode = ClientCode.ZipCode;
                ClientCode.zipcodeisvalidating = false;

                // reset- cities 

                cities = new ObservableCollection<string>(matchedcitiesoutput);

                zipcodeupdated = false;
            }

            */
        }

        private void streetNOPicker_Enter_Pressed(object sender, EventArgs e)
        {
            streetNOPicker.set_value();// from textbox in street picker

            // further country checking if netherland may be introduced ....
            int minno = 0;
            int maxno = 0;

            clientCodeBL.clientCode = ClientCode;
            clientCodeBL.set_max_min_streetnumbers(out maxno, out minno);

            // validate min max and integer value is correct?

            int enteredvalue = 0;
            Int32.TryParse(streetNOPicker.Val, out  enteredvalue);
            

            // if validation passed : set the client code street name ...(based on city and zipcode and the entered value....)
            if (enteredvalue >= minno && enteredvalue <= maxno)
            {
                // get street name 
                string streetname= clientCodeBL.get_streetName();
                ClientCode.Street_Name_No = string.Format("{0} {1}", streetname, enteredvalue.ToString());
            }
            else
            {
               // message box or something
            }
            popup_streetpost.IsOpen = false;

        }

        private void streetNOPicker_ESC_Pressed(object sender, EventArgs e)
        {
            popup_streetpost.IsOpen = false;
        }

        private void streetNOPicker_TextKeyPressed(object sender, EventArgs e)
        {
            // retain location of popup
        }

        private void ComboboxEX_SourceUpdated_2(object sender, DataTransferEventArgs e)
        {
            if (dataisloading || DataIsSetting) { return; }
            // city post source updated
            citypostupdated = true;
        }

        private void ComboboxEX_LostKeyboardFocus_2(object sender, KeyboardFocusChangedEventArgs e)
        {

            /*
            // city post lost keyboard focus..
            if (citypostupdated)
            {
                // logic
                // determine if this is special country that needs street picker 
                clientCodeBL.clientCode = ClientCode;
                bool needstreetpicker = clientCodeBL.need_street_picker(countries.ToList());
                if (needstreetpicker)
                {
                    // another logic for getting max and min values for street number
                    int minno = 0;
                    int maxno = 0;
                    clientCodeBL.set_max_min_streetnumbers(out maxno, out minno);
                    popup_streetpost.PlacementTarget = sender as CustomeControls.ComboboxEX;
                    streetNOPicker.Title = string.Format("Please Enter Value Between {0} and {1} for street number", minno.ToString(), maxno.ToString());
                    popup_streetpost.IsOpen = true;
                }
                else
                {
                    popup_streetpost.IsOpen = false;
                }
                citypostupdated = false;
            }
            */
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
           
        }
        
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // test calc

            // calc test 
            // CalculatePrices();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            // have insurance checked
            if (dataisloading || DataIsSetting) { return; }
            double insurancePercentage= ClientCode.Insurance_Percentage.HasValue ? (double)ClientCode.Insurance_Percentage : 0;
            if (insurancePercentage == 0)
            {
                // set the default value for insurance percentage
                ClientCode.Insurance_Percentage = DEFAULT_INSURANCE_PERC;
            }
            CalculatePrices(false);
           
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            // have insurance unchecked
            // we set these here and not in property changed becuase we don't want to override the values (its an UI related thing )


            // if the check box is setted by loading the bound property when the object is loading .... we dont want to do anything
            if (dataisloading || DataIsSetting) { return; }



            ClientCode.Insurance_Percentage = 0.0;
         //   ClientCode.Goods_Value = 0;

            CalculatePrices(false);
        }

        private void ComboboxEX_SourceUpdated_3(object sender, DataTransferEventArgs e)
        {
            // country source updated
            if (dataisloading || DataIsSetting) { return; }
            countryupdated = true;

            if (countryupdated)
            {
                cityAgentsoffices = new ObservableCollection<Agent>();
                // get agents equivalent to selected country
                var selectedcountry = (sender as ComboBox).SelectedItem as Country;
                if (selectedcountry != null)
                {
                    cityAgentsoffices = new ObservableCollection<Agent>(agentDa.GetAgents(selectedcountry.Id));
                }
                countryupdated = false;


                if (agentupdated)
                { OnAgentUpdated(); agentupdated = false; }

            }
        }
        
        private void ComboboxEX_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // country source selection changed
            // set the labels for outer country ....
            try
            {
                var selectedCountry = (sender as CustomeControls.ComboboxEX).SelectedItem as Country;
                if (selectedCountry != null)
                {
                    string continent = selectedCountry.continent;
                    if (continent == StaticData.Continent_Europe)
                    {
                        lbl_totalcost_by_Outer_Currency.Content = "Total Cost By Europe Currency";
                        lbl_unpaid_AndWillPay_inOuterCountry.Content = "Unpaid And Will PayIn Europe";
                    }
                    else
                    {
                        lbl_totalcost_by_Outer_Currency.Content = string.Format("{0} {1} {2}", "Total Cost By", selectedCountry.CountryName, "Currency");
                        lbl_unpaid_AndWillPay_inOuterCountry.Content = string.Format("{0} {1}", "Unpaid And Will PayIn", selectedCountry.CountryName);
                    }
                }
                else
                {
                    // reset...
                    lbl_totalcost_by_Outer_Currency.Content = "Total Cost";
                    lbl_unpaid_AndWillPay_inOuterCountry.Content = "Unpaid And Will PayIn";
                }
            }
            catch (Exception)
            { }
            

        }

        private void ComboboxEX_LostKeyboardFocus_3(object sender, KeyboardFocusChangedEventArgs e)
        {
            // country lost focus
            /*
            if (countryupdated)
            {
                cityAgentsoffices = new ObservableCollection<Agent>();
                // get agents equivalent to selected country
                var selectedcountry = (sender as ComboBox).SelectedItem as Country;
                if (selectedcountry != null)
                {
                    cityAgentsoffices = new ObservableCollection<Agent>(agentDa.GetAgents(selectedcountry.Id));
                }
                countryupdated = false;


                if (agentupdated)
                { OnAgentUpdated(); agentupdated = false; }

            }
            */
        }

        private void TextBoxEx_SourceUpdated_5(object sender, DataTransferEventArgs e)
        {
            //custome cost source updated
            if (dataisloading || DataIsSetting) { return; }
            customecostupdated = true;
            CalculatePrices(false);
        }

        private void TextBoxEx_LostKeyboardFocus_5(object sender, KeyboardFocusChangedEventArgs e)
        {
            // custome cost qomrk lost keyboard focus
            /*
            if (customecostupdated)
            {
                //calc
                CalculatePrices();
                customecostupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_6(object sender, DataTransferEventArgs e)
        {
            // export doc cost source updated
            if (dataisloading || DataIsSetting) { return; }
            exportdoccostupdated = true;
        }

        private void TextBoxEx_LostKeyboardFocus_6(object sender, KeyboardFocusChangedEventArgs e)
        {
            // export doc cost lost focus
            if (exportdoccostupdated)
            {
                //calc
                CalculatePrices(false);
                exportdoccostupdated = false;
            }
        }

        private void TextBoxEx_SourceUpdated_7(object sender, DataTransferEventArgs e)
        {
            // dicount source updated
            if (dataisloading || DataIsSetting) { return; }
            discountupdated = true;
            CalculatePrices(false);
        }

        private void TextBoxEx_LostKeyboardFocus_7(object sender, KeyboardFocusChangedEventArgs e)
        {
            // discount lost keyboard focus
            /*
            if (discountupdated)
            {
                //calc
                CalculatePrices();
                discountupdated = false;
            }
            */
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            isCheckPaidChecked = true;
            assign_TXTPaidcolors_Paid();
            if (dataisloading || DataIsSetting) { return; }
            // is all paid checked 
            ClientCode.TotalPaid_IQD = ClientCode.Total_Post_Cost_IQD;
            CalculatePrices(false);
        }

        private void CheckBox_Unchecked_2(object sender, RoutedEventArgs e)
        {
            // is all paid unchecked


            isCheckPaidChecked = false;
            assignTXTPaidCOlors_Unpaid();
            if (dataisloading || DataIsSetting) { return; }
            ClientCode.TotalPaid_IQD = 0;
            CalculatePrices(false);

        }
        
        private void TextBoxEx_SourceUpdated_8(object sender, DataTransferEventArgs e)
        {
            // paid amount souce updated 
            if (dataisloading || DataIsSetting) { return; }
            paidamountupdated = true;
            CalculatePrices(false);
        }

        private void TextBoxEx_LostKeyboardFocus_8(object sender, KeyboardFocusChangedEventArgs e)
        {
            // paid amount lost keyboard focus
            /*
            if (paidamountupdated)
            {
                //calc
                CalculatePrices();
                paidamountupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_9(object sender, DataTransferEventArgs e)
        {
            // box no source updated
            if (dataisloading || DataIsSetting) { return; }
            boxnoupdated = true;



            // testing ...

            /*
            // reset box packing 
            int BoxCount = 0;
            BoxCount = ClientCode.Box_No.HasValue ? (int)ClientCode.Box_No : 0;
            double BoxPackigingFactor = 0;
            BoxPackigingFactor = ClientCode.BoxPackigingFactor.HasValue ? (double)ClientCode.BoxPackigingFactor : 0;
            ClientCode.Packiging_cost_IQD = BoxCount * BoxPackigingFactor;
            */

            CalculatePrices(true); // use calculated box Packaging cost
        }

        private void TextBoxEx_LostKeyboardFocus_9(object sender, KeyboardFocusChangedEventArgs e)
        {
            // box no lost focus
            /*
            if (boxnoupdated)
            {
                // reset box packing 
                int BoxCount = 0;
                BoxCount = ClientCode.Box_No.HasValue ? (int)ClientCode.Box_No : 0;

                double BoxPackigingFactor = 0;
                BoxPackigingFactor = ClientCode.BoxPackigingFactor.HasValue ? (double)ClientCode.BoxPackigingFactor : 0;

                ClientCode.Packiging_cost_IQD = BoxCount * BoxPackigingFactor;


                CalculatePrices();
                boxnoupdated = false;
            }
            */
        }

        private void TextBoxEx_SourceUpdated_10(object sender, DataTransferEventArgs e)
        {
            // insurance perc source updated
            if (dataisloading || DataIsSetting) { return; }
            insurancepercentageupdated = true;
            CalculatePrices(false);

        }

        private void TextBoxEx_LostKeyboardFocus_10(object sender, KeyboardFocusChangedEventArgs e)
        {

            // insurance perc lost focus
            /*
            if (insurancepercentageupdated)
            {
                CalculatePrices();
                insurancepercentageupdated = false;
            }
            */
        }
        
        private void TextBoxEx_SourceUpdated_11(object sender, DataTransferEventArgs e)
        {
            // goods value source updated
            if (dataisloading || DataIsSetting) { return; }
            goodsvalueupdated = true;
            CalculatePrices(false);
        }

        private void TextBoxEx_LostKeyboardFocus_11(object sender, KeyboardFocusChangedEventArgs e)
        {
            // goods value lost focus
            /*
            if (goodsvalueupdated)
            {
                CalculatePrices();
                goodsvalueupdated = false;
            }
            */
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // add code
            OnAddClientCode();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // update code
            OnUpdateClientCode();
        }
       
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            // delete code
            OnDeleteCode();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // make label
            OnMakeLabel();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            // make invoice
            OnMakeInvoice();
        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
           

            showCodeSearchWindow(true);

        }
        
        private void UiSend_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.Enter))
            {
                try
                {
                    var selecteditem = cmb_clientcode.SelectedItem as ClientCode;
                    if (selecteditem != null)
                    {
                        // ClientCode = selecteditem; Old Approach
                        //  selecteditem.Code
                        load_code(selecteditem.Code);
                    }
                    else
                    {
                        WpfMessageBox.Show("", "No Item Selected", MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error);
                }
                finally
                { e.Handled = true; }
            }

            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.F))
            {
                showCodeSearchWindow(false); // dont show options
                e.Handled = true;
            }

            if (e.Key == Key.Escape)
            {
                if (popup_senderIDtypes.IsOpen)
                {
                    // NUll The Sender Id Type 
                    popup_senderIDtypes.IsOpen = false;
                    if (ClientCode != null)
                    {
                        ClientCode.Sender_ID_Type = null;
                        ClientCode.Sender_ID_Type_Shortcut = null;
                    }
                    e.Handled = true;
                }
               
            }
        }

        private void txt_Remaining_IQD_TextChanged(object sender, TextChangedEventArgs e)
        {
            assignTXTRemainingColors();

        }

        private void txt_EuropaToPay_TextChanged(object sender, TextChangedEventArgs e)
        {
            assignTXTToPayColors();
        }

        private void txt_Discount_TextChanged(object sender, TextChangedEventArgs e)
        {
            assignTXTDiscountColors();
        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            // check zip code
            checkZipcode();
        }

        private void identityTypeSelectorUC_Enter_Pressed(object sender, EventArgs e)
        {
            SetIdentityTypeAfterSelectorCommitt();
        }

        private void identityTypeSelectorUC_ESC_Pressed(object sender, EventArgs e)
        {

        }

        private void identityTypeSelectorUC_MouseClicked_1(object sender, EventArgs e)
        {
            SetIdentityTypeAfterSelectorCommitt();
        }

        private void txt_senderID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (popup_senderIDtypes.IsOpen)
            {
                popup_senderIDtypes.IsOpen = false;
            }
        }
        #endregion
    }
}