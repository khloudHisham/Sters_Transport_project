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
using System.Threading;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIReports_DashBoard.xaml
    /// </summary>
    public partial class UIReports_DashBoard : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion



        private ObservableCollection<CountrySummary> _countrysummaries;
        public ObservableCollection<CountrySummary> countrySummaries
        {
            get { return _countrysummaries; }
            set
            {
                _countrysummaries = value;
                OnPropertyChanged(new PropertyChangedEventArgs("countrySummaries"));
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





        ClientCodeDA clientCodeDA = new ClientCodeDA();
        BranchDa branchDa = new BranchDa();
        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        AgentDa agentDa = new AgentDa();


        bool windowisloaded;
        public UIReports_DashBoard()
        {
            InitializeComponent();
        }

        private void performDataLoad(DateTime? sd,DateTime?ed)
        {
            try
            {
                cmb_date.IsEnabled = false;
               
                Country selectedcountry = null;
                Agent selectedagent = null;
                Double shipmentNo = 0;

             


                DataTable dtsource = clientCodeDA.Generate_Country_Summary_View(selectedBranch, selectedcountry, selectedagent, shipmentNo, sd, ed);

                // foreach distinct country in datatable ... 
                DataView dv_source = new DataView(dtsource);
                DataTable dt_distinctCountries = dv_source.ToTable(true, "CountryAgentId");
                var distinctCountries_ids = dt_distinctCountries.AsEnumerable().Select(r => r.Field<long?>("CountryAgentId")).ToList();

                countrySummaries = new ObservableCollection<CountrySummary>();
                for (int c = 0; c < distinctCountries_ids.Count; c++)
                {
                    // get the totals and sums values ...
                    long countryid = (long)distinctCountries_ids[c];

                    var dataRow = dtsource.Select("CountryAgentId='" + countryid + "'").FirstOrDefault();
                    string countryname = string.Empty;
                    if (dataRow != null)
                    { countryname = dataRow["CountryName"].ToString(); }

                    // total codes..
                    int totalcodes = dtsource.Select("CountryAgentId='" + countryid + "'").Length;
                    object totalboxesOBJ = dtsource.Compute("Sum(Box_No)", "CountryAgentId='" + countryid + "'");
                    double totalboxes = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;

                    object totalpalletsOBJ = dtsource.Compute("Sum(Pallet_No)", "CountryAgentId='" + countryid + "'");
                    double totalpallets = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;

                    object totalKGOBJ = dtsource.Compute("Sum(Weight_Total)", "CountryAgentId='" + countryid + "'");
                    double totalKG = totalKGOBJ == DBNull.Value ? 0 : (double)totalKGOBJ;

                    object totalCashInOBJ = dtsource.Compute("Sum(Total_Post_Cost_IQD)", "CountryAgentId='" + countryid + "'");
                    double totalCashIn = totalCashInOBJ == DBNull.Value ? 0 : (double)totalCashInOBJ;

                    object totalCommisionBoxOBJ = dtsource.Compute("Sum(CommissionBox)", "CountryAgentId='" + countryid + "'");
                    decimal totalCommisionBox = totalCommisionBoxOBJ == DBNull.Value ? 0 : (decimal)totalCommisionBoxOBJ;

                    object totalCommisionKGOBJ = dtsource.Compute("Sum(CommissionKG)", "CountryAgentId='" + countryid + "'");
                    decimal totalCommisionKG = totalCommisionKGOBJ == DBNull.Value ? 0 : (decimal)totalCommisionKGOBJ;

                    decimal totalCommission = totalCommisionKG + totalCommisionBox;

                    object totalPaidIRQOBJ = dtsource.Compute("Sum(TotalPaid_IQD)", "CountryAgentId='" + countryid + "'");
                    double totalPaidIRQ = totalPaidIRQOBJ == DBNull.Value ? 0 : (double)totalPaidIRQOBJ;

                    object totalEUTOPAYOBJ = dtsource.Compute("Sum(EuropaToPay)", "CountryAgentId='" + countryid + "'");
                    double totalEUTOPAY = totalEUTOPAYOBJ == DBNull.Value ? 0 : (double)totalEUTOPAYOBJ;

                    CountrySummary countrySummary = new CountrySummary()
                    {
                        CountryName = countryname,
                        TotalCodes = totalcodes,
                        TotalBoxes = totalboxes,
                        TotalPallets = totalpallets,
                        TotalKG = totalKG,
                        TotalCashIn = totalCashIn,
                        TotalComissions = totalCommission,
                        TotalPaidToCompany = totalPaidIRQ,
                        TotalPaidInEurope = totalEUTOPAY
                    };

                    countrySummaries.Add(countrySummary);
                }

               
            }
            catch
            (Exception ex) 
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            finally
            { 
                 
                cmb_date.IsEnabled = true;
            }
        
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

           
            try
            {
                if (!windowisloaded)
                {
                    branches = new ObservableCollection<Agent>(agentDa.GetAgents());
                    windowisloaded = true;
                }

                selectedBranch = branches.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();
                cmb_date.SelectedIndex = 0;

            }
            catch (Exception)
            { }


            
       
           
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             DateTime date = DateTime.Now;
            if (cmb_date.SelectedIndex == 0) // no date filter
            {
                DateTime? sd = null;
                DateTime? ed = null;
                performDataLoad(sd,ed);
            }
            else if (cmb_date.SelectedIndex == 1) // this month
            {
                
                var sd = new DateTime(date.Year, date.Month, 1);
                var ed = sd.AddMonths(1).AddDays(-1);

                performDataLoad(sd, ed);
            }
            else if (cmb_date.SelectedIndex == 2) // previous month
            {
                var prev_month_date = date.AddMonths(-1);
                var sd = new DateTime(prev_month_date.Year, prev_month_date.Month, 1);
                var ed = sd.AddMonths(1).AddDays(-1);
                performDataLoad(sd, ed);
            }
            else if (cmb_date.SelectedIndex == 3) // this year
            {
                 
                var sd = new DateTime(date.Year, 1, 1);
                var ed = new DateTime(date.Year, 12, 31);
                performDataLoad(sd, ed);
            }
            else if (cmb_date.SelectedIndex == 4) // previous year
            {
                int prevYear = date.Year - 1;
                var sd = new DateTime(prevYear, 1, 1);
                var ed = new DateTime(prevYear, 12, 31);
                performDataLoad(sd, ed);
            }
        }
    }
}
