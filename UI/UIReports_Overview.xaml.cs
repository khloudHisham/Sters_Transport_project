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
using StersTransport.PresentationModels;
using System.Runtime.InteropServices;
using System.Globalization;
using Microsoft.Win32;
using System.IO;
using StersTransport.ReportsModels;
using System.Diagnostics;
using System.Threading;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIReports_Overview.xaml
    /// </summary>
    public partial class UIReports_Overview : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


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


        private ObservableCollection<Agent> _branchesQuick;
        public ObservableCollection<Agent> branchesQuick
        {
            get { return _branchesQuick; }
            set
            {
                _branchesQuick = value;
                OnPropertyChanged(new PropertyChangedEventArgs("branchesQuick"));
            }
        }


        private ObservableCollection<Agent> _branchesDaily;
        public ObservableCollection<Agent> branchesDaily
        {
            get { return _branchesDaily; }
            set
            {
                _branchesDaily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("branchesDaily"));
            }
        }



        private ObservableCollection<ShipmentNumbers> _shipmentnumbers;
        public ObservableCollection<ShipmentNumbers> shipmentnumbers
        {
            get { return _shipmentnumbers; }
            set
            {
                _shipmentnumbers = value;
                OnPropertyChanged(new PropertyChangedEventArgs("shipmentnumbers"));
            }
        }

        private ObservableCollection<ShipmentNumbers> _shipmentnumbersto;
        public ObservableCollection<ShipmentNumbers> shipmentnumbersto
        {
            get { return _shipmentnumbersto; }
            set
            {
                _shipmentnumbersto = value;
                OnPropertyChanged(new PropertyChangedEventArgs("shipmentnumbersto"));
            }
        }


        private ObservableCollection<ShipmentNumbers> _shipmentnumbersquick;
        public ObservableCollection<ShipmentNumbers> shipmentnumbersquick
        {
            get { return _shipmentnumbersquick; }
            set
            {
                _shipmentnumbersquick = value;
                OnPropertyChanged(new PropertyChangedEventArgs("shipmentnumbersquick"));
            }
        }



        private double _totalcodes;
        public double totalcodes
        {
            get { return _totalcodes; }
            set
            {
                _totalcodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalcodes"));
            }
        }

        private double _totalboxes;
        public double totalboxes
        {
            get { return _totalboxes; }
            set
            {
                _totalboxes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalboxes"));
            }
        }

        private double _totalpallets;
        public double totalpallets
        {
            get { return _totalpallets; }
            set
            {
                _totalpallets = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalpallets"));
            }
        }

        private double _totalKG;
        public double totalKG
        {
            get { return _totalKG; }
            set
            {
                _totalKG = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalKG"));
            }
        }

        private double _totalcashIN;
        public double totalcashIN
        {
            get { return _totalcashIN; }
            set
            {
                _totalcashIN = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalcashIN"));
            }
        }

        private decimal _totalcomissions;
        public decimal totalcomissions
        {
            get { return _totalcomissions; }
            set
            {
                _totalcomissions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalcomissions"));
            }
        }



       
        public List<string> distinctCurrencies
        {
            get;set;
        }

        
        public List<double> outside_toPay_percurrency
        {
            get;set;
        }



        private double _dailycodes;
        public double dailycodes
        {
            get { return _dailycodes; }
            set
            {
                _dailycodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailycodes"));
            }
        }

        private double _dailyboxes;
        public double dailyboxes
        {
            get { return _dailyboxes; }
            set
            {
                _dailyboxes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailyboxes"));
            }
        }

        private double _dailypallets;
        public double dailypallets
        {
            get { return _dailypallets; }
            set
            {
                _dailypallets = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailypallets"));
            }
        }

        private double _dailyKG;
        public double dailyKG
        {
            get { return _dailyKG; }
            set
            {
                _dailyKG = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailyKG"));
            }
        }

        private double _dailycashIN;
        public double dailycashIN
        {
            get { return _dailycashIN; }
            set
            {
                _dailycashIN = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailycashIN"));
            }
        }

        private decimal _dailycomissions;
        public decimal dailycomissions
        {
            get { return _dailycomissions; }
            set
            {
                _dailycomissions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("dailycomissions"));
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


        private Agent _selectedBranchQuick;
        public Agent selectedBranchQuick
        {
            get { return _selectedBranchQuick; }
            set
            {
                _selectedBranchQuick = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedBranchQuick"));
            }
        }



        private Agent _selectedBranchDaily;
        public Agent selectedBranchDaily
        {
            get { return _selectedBranchDaily; }
            set
            {
                _selectedBranchDaily = value;
                OnPropertyChanged(new PropertyChangedEventArgs("selectedBranchDaily"));
            }
        }




        ClientCodeDA clientCodeDA = new ClientCodeDA();
        BranchDa branchDa = new BranchDa();
        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        AgentDa agentDa = new AgentDa();




        bool windowisloaded;


        exportHelper exporthelper = new exportHelper();
        StringHelper stringHelper = new StringHelper();
        public UIReports_Overview()
        {
            InitializeComponent();

            totalcodes = 0;
            totalboxes = 0;
            totalpallets = 0;
            totalKG = 0;
            totalcashIN = 0;
            totalcomissions = 0;


            dailycodes = 0;
            dailyboxes = 0;
            dailypallets = 0;
            dailyKG = 0;
            dailycashIN = 0;
            dailycomissions = 0;

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
                selectedBranchQuick = branchesQuick.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();
                selectedBranchDaily = branchesDaily.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();


                // last shipment number
                try
                {
                    cmb_TruckNo2.SelectedIndex = 1;
                }
                catch (Exception) { }

                chk_date.IsChecked = true;
                dtpick.SelectedDate = DateTime.Now;
            }
            catch (Exception)
            { }
        }


        private void refreshcollections()
        {
            branches = new ObservableCollection<Agent>(agentDa.GetAgents());
            Agent brannchAll = new Agent()
            {
                Id = 0,
                AgentName = "ALL"
            };
           // branches.Insert(0, brannchAll);


            branchesDaily = new ObservableCollection<Agent>(agentDa.GetAgents());
            Agent brannchAll2 = new Agent()
            {
                Id = 0,
                AgentName = "ALL"
            };
          //  branchesDaily.Insert(0, brannchAll2);


            branchesQuick = new ObservableCollection<Agent>(agentDa.GetAgents());
            Agent brannchAll3 = new Agent()
            {
                Id = 0,
                AgentName = "ALL"
            };
           // branchesQuick.Insert(0, brannchAll3);







            countries = new ObservableCollection<Country>(countryDa.GetCountries(true));

            Country countryALL = new Country()
            {
                Id = 0,
                CountryName = "ALL"
            };
            countries.Insert(0, countryALL);


            agents = new ObservableCollection<Agent>();
            cities = new ObservableCollection<City>();

            // shipmentnumbers = new ObservableCollection<double?>();

            //   shipmentnumbersquick = new ObservableCollection<double?>();

            //   shipmentnumbersto = new ObservableCollection<double?>();

             shipmentnumbers = new ObservableCollection<ShipmentNumbers>();

             shipmentnumbersquick = new ObservableCollection<ShipmentNumbers>();

             shipmentnumbersto = new ObservableCollection<ShipmentNumbers>();

        }

        private void cmb_officeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // branch  selection changes
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            if (selectedbranch == null)
            {
                // reset 
                //  codes = new ObservableCollection<ClientCode>();
                shipmentnumbers = new ObservableCollection<ShipmentNumbers>();
                shipmentnumbersto = new ObservableCollection<ShipmentNumbers>();
            }
            else
            {
                shipmentnumbers = new ObservableCollection<ShipmentNumbers>();
                List<ClientCode> _codes = clientCodeDA.GetClientCodes();
                ObservableCollection<double?>numbers= new ObservableCollection<double?>(_codes.Where(c => c.BranchId == selectedbranch.Id).OrderByDescending(xx => xx.Shipment_No).Select(x => x.Shipment_No).Distinct().ToList());
                shipmentnumbers = ShipmentNumbersHelper.formShipmentNumbers(numbers);

                ObservableCollection<double?> numbersto= new ObservableCollection<double?>(_codes.Where(c => c.BranchId == selectedbranch.Id).OrderByDescending(xx => xx.Shipment_No).Select(x => x.Shipment_No).Distinct().ToList());
                shipmentnumbersto = ShipmentNumbersHelper.formShipmentNumbers(numbersto);
            }
        }

      

        private void cmb_EUCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            if (selectedcountry == null)
            {
                agents = new ObservableCollection<Agent>();
            }
            else
            {

                agents = new ObservableCollection<Agent>(agentDa.GetAgents((long)selectedcountry.Id));
                Agent agentAll = new Agent()
                {
                    Id = 0,
                    AgentName = "ALL"
                };
                agents.Insert(0, agentAll);


                if (selectedcountry.CountryName == "ALL")
                {
                    cmb_AgentCity.SelectedIndex = 0;//ALL
                }
            }
        }

        private void cmb_officeName_SelectionChangedquick(object sender, SelectionChangedEventArgs e)
        {
            // branch  selection changes
            var selectedbranch = cmb_officeNameQuick.SelectedItem as Agent;
            if (selectedbranch == null)
            {
                // reset 
                //  codes = new ObservableCollection<ClientCode>();
                shipmentnumbersquick = new ObservableCollection<ShipmentNumbers>();

            }
            else
            {
                shipmentnumbersquick = new ObservableCollection<ShipmentNumbers>();
                List<ClientCode> _codes = clientCodeDA.GetClientCodes();
                ObservableCollection<double?> numbersQuick  = new ObservableCollection<double?>(_codes.Where(c => c.BranchId == selectedbranch.Id).OrderByDescending(xx => xx.Shipment_No).Select(x => x.Shipment_No).Distinct().ToList());
                shipmentnumbersquick = ShipmentNumbersHelper.formShipmentNumbers(numbersQuick);

            }


            // generate quick report

            double shipmentNo = cmb_TruckNo2.SelectedItem == null ? 0 : (double)cmb_TruckNo2.SelectedItem;

            DataTable dt = clientCodeDA.Generate_Quick_Report_View(selectedbranch, shipmentNo);
            // foreach currency get the outside (europe to pay )
            Assign_QuickReportSummaryValues(dt);
        }

        private void Assign_QuickReportSummaryValues(DataTable dtsource)
        {

            // get distinct currencies 
            DataView view_currency = new DataView(dtsource);
            DataTable distinct_CurrenciesTable = view_currency.ToTable(true, "Currency_Type");
            distinctCurrencies = new List<string>();
            outside_toPay_percurrency = new List<double>();
            for (int c = 0; c < distinct_CurrenciesTable.Rows.Count; c++)
            {
                distinctCurrencies.Add(distinct_CurrenciesTable.Rows[c]["Currency_Type"].ToString());
                // get the sum

                object CSOBJ = dtsource.Compute("Sum(EuropaToPay)", "Currency_Type='" + distinct_CurrenciesTable.Rows[c]["Currency_Type"].ToString() + "'");
                double CS_dbl = CSOBJ == DBNull.Value ? 0 : (double)CSOBJ;
                outside_toPay_percurrency.Add(CS_dbl);
            }

            // total codes..
            int totalcodes_int = dtsource.Rows.Count;
            object totalboxesOBJ = dtsource.Compute("Sum(Box_No)", "");
            double totalboxes_dbl = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;

            object totalpalletsOBJ = dtsource.Compute("Sum(Pallet_No)", "");
            double totalpallets_dbl = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;

            object totalKGOBJ = dtsource.Compute("Sum(Weight_Total)", "");
            double totalKG_dbl = totalKGOBJ == DBNull.Value ? 0 : (double)totalKGOBJ;

            object totalCashInOBJ = dtsource.Compute("Sum(Total_Post_Cost_IQD)", "");
            double totalCashIn_dbl = totalCashInOBJ == DBNull.Value ? 0 : (double)totalCashInOBJ;

            object totalCommisionBoxOBJ = dtsource.Compute("Sum(CommissionBox)", "");
            decimal totalCommisionBox_dbl = totalCommisionBoxOBJ == DBNull.Value ? 0 : (decimal)totalCommisionBoxOBJ;

            object totalCommisionKGOBJ = dtsource.Compute("Sum(CommissionKG)", "");
            decimal totalCommisionKG_dbl = totalCommisionKGOBJ == DBNull.Value ? 0 : (decimal)totalCommisionKGOBJ;

            decimal totalCommission_dbl = totalCommisionKG_dbl + totalCommisionBox_dbl;


            totalcodes = totalcodes_int;
            totalboxes = totalboxes_dbl;
            totalpallets = totalpallets_dbl;
            totalKG = totalKG_dbl;
            totalcashIN = totalCashIn_dbl;
            totalcomissions = totalCommission_dbl;




        }
        private void Assign_DailyReportSummaryValues(DataTable dtsource)
        {
            // total codes..
            int totalcodes_int = dtsource.Rows.Count;
            object totalboxesOBJ = dtsource.Compute("Sum(Box_No)", "");
            double totalboxes_dbl = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;

            object totalpalletsOBJ = dtsource.Compute("Sum(Pallet_No)", "");
            double totalpallets_dbl = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;

            object totalKGOBJ = dtsource.Compute("Sum(Weight_Total)", "");
            double totalKG_dbl = totalKGOBJ == DBNull.Value ? 0 : (double)totalKGOBJ;

            object totalCashInOBJ = dtsource.Compute("Sum(Total_Post_Cost_IQD)", "");
            double totalCashIn_dbl = totalCashInOBJ == DBNull.Value ? 0 : (double)totalCashInOBJ;

            object totalCommisionBoxOBJ = dtsource.Compute("Sum(CommissionBox)", "");
            decimal totalCommisionBox_dbl = totalCommisionBoxOBJ == DBNull.Value ? 0 : (decimal)totalCommisionBoxOBJ;

            object totalCommisionKGOBJ = dtsource.Compute("Sum(CommissionKG)", "");
            decimal totalCommisionKG_dbl = totalCommisionKGOBJ == DBNull.Value ? 0 : (decimal)totalCommisionKGOBJ;

            decimal totalCommission_dbl = totalCommisionKG_dbl + totalCommisionBox_dbl;


            dailycodes = totalcodes_int;
            dailyboxes = totalboxes_dbl;
            dailypallets = totalpallets_dbl;
            dailyKG = totalKG_dbl;
            dailycashIN = totalCashIn_dbl;
            dailycomissions = totalCommission_dbl;


        }

        private void cmb_TruckNo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // generate quick report
            // another approach is to load all data when window is loaded and then filter it based on controls  ....
            var selectedbranch = cmb_officeNameQuick.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo2.SelectedItem == null ? 0 : (double)cmb_TruckNo2.SelectedValue;
            DataTable dt= clientCodeDA.Generate_Quick_Report_View(selectedbranch, shipmentNo);
            Assign_QuickReportSummaryValues(dt);
        }

        private void cmb_officeName_SelectionChangedDaily(object sender, SelectionChangedEventArgs e)
        {
            generate_dailyReport();
        }
        private void generate_dailyReport()
        {
            // branch  selection changes
            var selectedbranch = cmb_officeNameDaily.SelectedItem as Agent;

            // generate daily report....
            DateTime? dt = DateTime.Now;
            if (chk_date.IsChecked.HasValue)
            {
                if ((bool)chk_date.IsChecked.Value)
                {
                    dt = dtpick.SelectedDate;
                }
            }

            DataTable dtsource = clientCodeDA.Generate_Daily_Report_View(selectedbranch, dt);
            Assign_DailyReportSummaryValues(dtsource);
        }

        public void generate_Main_report(out DataTable dtOut)
        {
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
            double shipmentNoto = cmb_TruckNoto.SelectedItem == null ? 0 : (double)cmb_TruckNoto.SelectedValue;
            DataTable dtsource = clientCodeDA.Generate_Country_Report_View(selectedbranch, selectedcountry, selectedagent, shipmentNo, shipmentNoto);
            dtOut = dtsource;
        }

        public void generate_Main_report_FewerColumns(out DataTable dtOut)
        {
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
            double shipmentNoto = cmb_TruckNoto.SelectedItem == null ? 0 : (double)cmb_TruckNoto.SelectedValue;
            DataTable dtsource = clientCodeDA.Generate_Country_Report_View_FewerColumns(selectedbranch, selectedcountry, selectedagent, shipmentNo, shipmentNoto);
            dtOut = dtsource;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                // check if comboboxes have selected values 
                List<string> MustSelectValues = new List<string>();
                if (cmb_officeName.SelectedIndex == -1)
                {
                    MustSelectValues.Add("Office Name");
                }

                if (cmb_TruckNo.SelectedIndex == -1)
                {
                    MustSelectValues.Add("From Truck NO");
                }

                if (cmb_TruckNoto.SelectedIndex == -1)
                {
                    MustSelectValues.Add("To Truck NO");
                }

                if (cmb_EUCountry.SelectedIndex == -1)
                {
                    MustSelectValues.Add("EU Country");
                }

                if (cmb_AgentCity.SelectedIndex == -1)
                {
                    MustSelectValues.Add("Agent City");
                }


               string mustselectvaluesstring=  stringHelper.generatestringFromList(MustSelectValues);

              
                if (mustselectvaluesstring.Length > 0)
                {
                    string errmessage = "Please Select The Following Values:" + Environment.NewLine;
                    errmessage += mustselectvaluesstring;
                    WpfMessageBox.Show("", errmessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning);
                    return;
                }


                DataTable dt = new DataTable();
                generate_Main_report(out dt);

                // form the file name ....

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //  string outputfilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + formFileName(".xls");
                        string outputfilename = fbd.SelectedPath + "\\" + formFileName(".xls");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }

                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }


                        // exporthelper.Export_To_Excel(dt, outputfilename,true,true);
                        //  Thread t = new Thread(() => exporthelper.Export_To_Excel(dt, outputfilename, true, true)) ;
                        //   t.SetApartmentState(ApartmentState.STA);
                        //   t.Start();
                        adorner1.IsAdornerVisible = true;
                        Task.Run(() => exporthelper.Export_To_Excel(dt, outputfilename, true, true)).ContinueWith
                              (task =>
                              {
                                  adorner1.IsAdornerVisible = false;
                                  //. this will run back on the UI thread once the task has finished
                                  if (File.Exists(outputfilename))
                                  {
                                      try
                                      { Process.Start("explorer.exe", outputfilename); }
                                      catch (Exception) { }

                                  }
                              }, System.Threading.CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());

                        /*
                        if (File.Exists(outputfilename))
                        {
                            try
                            { Process.Start("explorer.exe", outputfilename); }
                            catch (Exception) { }

                        }
                        */
                    }
                }


              
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }


        }

        private string formFileName(string extension)
        {
            string result = string.Empty;
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
            double shipmentNoto = cmb_TruckNoto.SelectedItem == null ? 0 : (double)cmb_TruckNoto.SelectedValue;

            result += selectedbranch.AgentName+"_";
            result += "Truck_";
            if (shipmentNo == shipmentNoto)
            {
                string shipmentNostr = shipmentNo == 0 ? "All" : shipmentNo.ToString();
                result += shipmentNostr;
            }
            else
            {
                string shipmentNostr = shipmentNo == 0 ? "All" : shipmentNo.ToString();
                string shipmentNoTostr = shipmentNoto == 0 ? "All" : shipmentNoto.ToString();
                result += shipmentNostr + "To" + shipmentNoTostr;
            }
           
            if (selectedcountry != null)
            {
                result += "_Country_";
                result += selectedcountry.CountryName+"_";
            }
            if (selectedagent != null)
            {
                result += "_Agent_";
                result += selectedagent.AgentName + "_";
            }
            result += extension;

            return result;
        }

        private string formFileNameQuick(string extension)
        {
            string result = string.Empty;
            var selectedbranch =  cmb_officeNameQuick.SelectedItem as Agent;
            double shipmentNo =  cmb_TruckNo2.SelectedItem == null ? 0 : (double)cmb_TruckNo2.SelectedValue;
         
            result += selectedbranch.AgentName + "_";
            result += "Truck_";
            string shipmentNostr = shipmentNo == 0 ? "All" : shipmentNo.ToString();
            result += shipmentNostr;

            result += extension;

            return result;
        }

        private string formFileNameDaily(string extension)
        {
            string result = string.Empty;
            var selectedbranch = cmb_officeNameDaily.SelectedItem as Agent;
            
            result += selectedbranch.AgentName + "_";

            string dtstr = dtpick.SelectedDate.HasValue ? dtpick.SelectedDate.Value.ToString("dd_MM_yyyy") : string.Empty;


            result += dtstr;

            result += extension;

            return result;
        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // export pdf
            try
            {
                DataTable dt = new DataTable();
                generate_Main_report_FewerColumns(out dt);

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        //  string outputfilename = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + formFileName(".pdf");
                        string outputfilename = fbd.SelectedPath + "\\" + formFileName(".pdf");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }
                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }

                        // the header 
                        string header = string.Empty;
                        var selectedcountry = cmb_EUCountry.SelectedItem as Country;
                        var selectedbranch = cmb_officeName.SelectedItem as Agent;
                        var selectedagent = cmb_AgentCity.SelectedItem as Agent;
                        double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
                        double shipmentNoto = cmb_TruckNoto.SelectedItem == null ? 0 : (double)cmb_TruckNoto.SelectedValue;

                        if (selectedbranch != null)
                        {
                            header += string.Format("{0} : {1} ", "Branch", selectedBranch.AgentName);
                        }
                        if (selectedcountry != null)
                        {
                            if (header.Length > 0)
                            {
                                header += Environment.NewLine;
                            }
                            header += string.Format("{0} : {1} ", "Country", selectedcountry.CountryName);
                        }
                        if (selectedagent != null)
                        {
                            if (header.Length > 0)
                            {
                                header += Environment.NewLine;
                            }
                            header += string.Format("{0} : {1} ", "Agent", selectedagent.AgentName);
                        }
                        if (shipmentNo > 0||shipmentNoto>0)
                        {
                            if (header.Length > 0)
                            {
                                header += Environment.NewLine;
                            }

                            header += "Shipment NO: ";
                            if (shipmentNo > 0)
                            {
                                header += string.Format("{0} : {1} ", "From", shipmentNo.ToString());
                            }
                            if (shipmentNoto > 0)
                            {
                                header += string.Format("{0} : {1} ", "To", shipmentNoto.ToString());
                            }
                        }
                        float[] widthes = new float[] { 7, 5, 9, 9, 9, 9, 10, 5, 5, 4, 4, 6, 6, 6, 6 };
                        exporthelper.exporttopdf(header,dt, true, outputfilename,widthes);
                    }
                }
                    
 


            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        }

        private void dtpick_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            generate_dailyReport();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable dt = formOutputTableFromQuickReport();

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string outputfilename = fbd.SelectedPath + "\\" + formFileNameQuick(".xls");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }
                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }

                        exporthelper.Export_To_Excel(dt, outputfilename,false,false);
                        if (File.Exists(outputfilename))
                        {
                            try
                            { Process.Start("explorer.exe", outputfilename); }
                            catch (Exception) { }

                        }
                    }
                }
                   
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        
        }

        private DataTable formOutputTableFromQuickReport()
        {
            // excel export main report
            DataTable dt = new DataTable();
            dt.Columns.Add("Key", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            DataRow dr;

            dr = dt.NewRow();
            dr[0] = "Office Name";
            dr[1] = selectedBranchQuick.AgentName;
            dt.Rows.Add(dr);


            double shipmentNo = cmb_TruckNo2.SelectedItem == null ? 0 : (double)cmb_TruckNo2.SelectedValue;
            dr = dt.NewRow();
            dr[0] = "Truck NO";
            dr[1] = shipmentNo.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total Codes";
            dr[1] = totalcodes.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total Pallet";
            dr[1] = totalpallets.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total Box";
            dr[1] = totalboxes.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total KG";
            dr[1] = totalKG.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total Cash In";
            dr[1] = totalcashIN.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total Comission";
            dr[1] = totalcomissions.ToString();
            dt.Rows.Add(dr);

            for (int c = 0; c < distinctCurrencies.Count; c++)
            {
                dr = dt.NewRow();
                dr[0] = distinctCurrencies[c];
                dr[1] = outside_toPay_percurrency[c].ToString();
                dt.Rows.Add(dr);
            }

            return dt;
        }


        private DataTable formOutputTableFromDailyReport()
        {
            // excel export main report
            DataTable dt = new DataTable();
            dt.Columns.Add("Key", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            DataRow dr;

            dr = dt.NewRow();
            dr[0] = "Office Name";
            dr[1] = selectedBranchDaily.AgentName;
            dt.Rows.Add(dr);

            if (dtpick.SelectedDate.HasValue&&chk_date.IsChecked.HasValue)
            {
                if ((bool)chk_date.IsChecked)
                {
                    dr = dt.NewRow();
                    dr[0] = "Date";
                    dr[1] = dtpick.SelectedDate.Value.ToShortDateString();
                    dt.Rows.Add(dr);
                }
              
            }
            


            dr = dt.NewRow();
            dr[0] = "Daily Codes";
            dr[1] = dailycodes.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Daily Pallet";
            dr[1] = dailypallets.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Daily Box";
            dr[1] = dailyboxes.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Total KG";
            dr[1] = dailyKG.ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Daily Cash In";
            dr[1] = dailycashIN. ToString();
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "Daily Comission";
            dr[1] =dailycomissions.ToString();
            dt.Rows.Add(dr);

            return dt;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
            // export quick report to pdf
            DataTable dt = formOutputTableFromQuickReport();
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string outputfilename = fbd.SelectedPath + "\\" + formFileNameQuick(".pdf");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }
                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }
                        string header = string.Empty;
                         
                        var selectedbranch = cmb_officeNameQuick.SelectedItem as Agent;
                        
                        double shipmentNo = cmb_TruckNo2.SelectedItem == null ? 0 : (double)cmb_TruckNo2.SelectedValue;
                        if (selectedbranch != null)
                        {
                            header += string.Format("{0} : {1} ", "Branch", selectedBranch.AgentName);
                        }
                        if (selectedbranch != null)
                        {
                            header += string.Format("{0} : {1} ", "Branch", selectedBranch.AgentName);
                        }
                        if (shipmentNo > 0  )
                        {
                            if (header.Length > 0)
                            {
                                header += Environment.NewLine;
                            }

                            header += "Shipment NO: ";
                            if (shipmentNo > 0)
                            {
                                header += string.Format("{0} : {1} ", "", shipmentNo.ToString());
                            }
                            
                        }
                        float[] widthes = new float[] { 50, 50 };
                        exporthelper.exporttopdf(header,dt, false, outputfilename,widthes);
                    }
                }
         
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // exprot daily report to excel
            try
            {
                DataTable dt = formOutputTableFromDailyReport();
             

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string outputfilename = fbd.SelectedPath + "\\" + formFileNameDaily(".xls");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }
                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }

                        exporthelper.Export_To_Excel(dt, outputfilename,false,false);
                        if (File.Exists(outputfilename))
                        {
                            try
                            { Process.Start("explorer.exe", outputfilename); }
                            catch (Exception) { }

                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            
            
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            // exprot daily report to pdf
            try
            {
                DataTable dt = formOutputTableFromDailyReport();

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        string outputfilename = fbd.SelectedPath + "\\" + formFileNameDaily(".pdf");
                        if (File.Exists(outputfilename))
                        {
                            MessageBoxResult msgbr = WpfMessageBox.Show("", "The File" + outputfilename + "Already Exists Do You Want To Replace It?", MessageBoxButton.YesNo,
                                 WpfMessageBox.MessageBoxImage.Warning);
                            if (msgbr == MessageBoxResult.No) { return; }
                        }
                        if (File.Exists(outputfilename))
                        {
                            // delete the file
                            File.Delete(outputfilename);
                        }

                        string header = string.Empty;
                        var selectedbranch = cmb_officeNameDaily.SelectedItem as Agent;
                        DateTime dtt = dtpick.SelectedDate.Value;
                        if (selectedbranch != null)
                        {
                            header += string.Format("{0} : {1} ", "Branch", selectedBranch.AgentName);
                        }
                        if (header.Length > 0)
                        {
                            header += Environment.NewLine;
                        }
                        // dtt.ToString("dd/MM/yyyy");
                        header += string.Format("{0} : {1} ", "Date", dtt.ToString("dd/MM/yyyy"));
                        float[] widthes = new float[] { 50, 50 };
                        exporthelper.exporttopdf(header,dt, false, outputfilename, widthes);
                    }
                }
                    
         
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
          
            
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            // import 
            ImportExcelClientCodesData importExcelClient = new ImportExcelClientCodesData();
            importExcelClient.ShowDialog();
        }

        private void chk_date_Checked(object sender, RoutedEventArgs e)
        {
            generate_dailyReport();
        }

        private void chk_date_Unchecked(object sender, RoutedEventArgs e)
        {
            generate_dailyReport();
        }
    }
}
