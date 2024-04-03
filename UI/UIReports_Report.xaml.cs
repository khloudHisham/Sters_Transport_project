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
using StersTransport.ReportsModels;
using System.IO;
using System.Diagnostics;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIReports_Report.xaml
    /// </summary>
    public partial class UIReports_Report : UserControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        SolidColorBrush firstcolumnbrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#00D1FA"));

        public ObservableCollection<ClientCode> codes
        { get; set; }

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

        private double _totalpaidincompany;
        public double totalpaidincompany
        {
            get { return _totalpaidincompany; }
            set
            {
                _totalpaidincompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalpaidincompany"));
            }
        }


        private double _totalpaidinEurope;
        public double totalpaidinEurope
        {
            get { return _totalpaidinEurope; }
            set
            {
                _totalpaidinEurope = value;
                OnPropertyChanged(new PropertyChangedEventArgs("totalpaidinEurope"));
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



        ClientCodeDA clientCodeDA = new ClientCodeDA();
        BranchDa branchDa = new BranchDa();
        CountryDa countryDa = new CountryDa();
        CityDa cityDa = new CityDa();
        AgentDa agentDa = new AgentDa();


        exportHelper exporthelper = new exportHelper();

        bool windowisloaded;
        bool dataisloading;
        public UIReports_Report()
        {
            windowisloaded = false;
            InitializeComponent();
            totalcodes = 0;
            totalboxes = 0;
            totalpallets = 0;
            totalKG = 0;
            totalcashIN = 0;
            totalcomissions = 0;
            totalpaidincompany = 0;
            totalpaidinEurope = 0;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            

            try
            {
                dataisloading = true;
                if (!windowisloaded)
                {
                    refreshcollections();
                   
                }
                selectedBranch = branches.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();// doesnt fire selection changes (only fired by user )

                Perform_Office_SelectionChanged();

                // set the shipment no to the heighest one 
                if (cmb_TruckNo.Items.Count > 0)
                {
                    cmb_TruckNo.SelectedIndex = 1; // ordered by descending...
                }

                generateSummary();
            }
            catch (Exception)
            {
                
            }
            finally
            {
                windowisloaded = true;
                dataisloading = false;
            }

        }

        private void refreshcollections()
        {
            branches =new ObservableCollection<Agent> ( agentDa.GetAgents());
            Agent brannchAll = new Agent()
            {
                Id = 0,
                AgentName = "ALL"
            };
          //  branches.Insert(0, brannchAll);

            countries = new ObservableCollection<Country>(countryDa.GetCountries(true));

            Country countryALL = new Country()
            {
                Id = 0,
                CountryName="ALL"
            };
            countries.Insert(0, countryALL);


            agents = new ObservableCollection<Agent>();
            cities = new ObservableCollection<City>();

            shipmentnumbers = new ObservableCollection<ShipmentNumbers>();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            generateSummary();
        }

        private void generateSummary()
        {
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
            DateTime? sd = dt_postdateStart.SelectedDate;
            DateTime? ed = dt_postdateEnd.SelectedDate;

            DataTable dtsource = clientCodeDA.Generate_Country_Summary_View(selectedbranch, selectedcountry, selectedagent, shipmentNo, sd, ed);
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
                int totalcodesINT = dtsource.Select("CountryAgentId='" + countryid + "'").Length;
                object totalboxesOBJ = dtsource.Compute("Sum(Box_No)", "CountryAgentId='" + countryid + "'");
                double totalboxesDBL = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;

                object totalpalletsOBJ = dtsource.Compute("Sum(Pallet_No)", "CountryAgentId='" + countryid + "'");
                double totalpalletsDBL = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;

                object totalKGOBJ = dtsource.Compute("Sum(Weight_Total)", "CountryAgentId='" + countryid + "'");
                double totalKGDBL = totalKGOBJ == DBNull.Value ? 0 : (double)totalKGOBJ;

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
                    TotalCodes = totalcodesINT,
                    TotalBoxes = totalboxesDBL,
                    TotalPallets = totalpalletsDBL,
                    TotalKG = totalKGDBL,
                    TotalCashIn = totalCashIn,
                    TotalComissions = totalCommission,
                    TotalPaidToCompany = totalPaidIRQ,
                    TotalPaidInEurope = totalEUTOPAY
                };

                countrySummaries.Add(countrySummary);
            }

            totalcodes = countrySummaries.Sum(x => x.TotalCodes);
            totalboxes = countrySummaries.Sum(x => x.TotalBoxes);
            totalpallets = countrySummaries.Sum(x => x.TotalPallets);
            totalKG = countrySummaries.Sum(x => x.TotalKG);
            totalcashIN = countrySummaries.Sum(x => x.TotalCashIn);
            totalcomissions = countrySummaries.Sum(x => x.TotalComissions);
            totalpaidincompany = countrySummaries.Sum(x => x.TotalPaidToCompany);
            totalpaidinEurope = countrySummaries.Sum(x => x.TotalPaidInEurope);
        }


        private DataTable ConvertfromCountrySummaryTODatatable()
        {
            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("Country",typeof(System.String));
            dtresult.Columns.Add("Total_Codes", typeof(System.Double));
            dtresult.Columns.Add("Total_Boxes", typeof(System.Double));
            dtresult.Columns.Add("Total_Pallets", typeof(System.Double));
            dtresult.Columns.Add("TotalCashIn", typeof(System.Double));
            dtresult.Columns.Add("Total_KG", typeof(System.Double));
            dtresult.Columns.Add("Total_Comission", typeof(System.Decimal));
            dtresult.Columns.Add("Total_Paid_To_Company", typeof(System.Double));
            dtresult.Columns.Add("Total_Paid_Europe", typeof(System.Double)); // not necessary Europe Maybe we need to change to : total Paid OUtside or export country


            DataRow dr;

            for (int c = 0; c < countrySummaries.Count; c++)
            {
                dr = dtresult.NewRow();
                dr["Country"] = countrySummaries[c].CountryName;
                dr["Total_Codes"] = countrySummaries[c].TotalCodes;
                dr["Total_Boxes"] = countrySummaries[c].TotalBoxes;
                dr["Total_Pallets"] = countrySummaries[c].TotalPallets;
                dr["TotalCashIn"] = countrySummaries[c].TotalCashIn;
                dr["Total_KG"] = countrySummaries[c].TotalKG;
                dr["Total_Comission"] = countrySummaries[c].TotalComissions;
                dr["Total_Paid_To_Company"] = countrySummaries[c].TotalPaidToCompany;
                dr["Total_Paid_Europe"] = countrySummaries[c].TotalPaidInEurope;
                
                dtresult.Rows.Add(dr);
            }

            // add sum row 

            dr = dtresult.NewRow();
            dr["Country"] = "Sum";
            dr["Total_Codes"] = totalcodes;
            dr["Total_Boxes"] = totalboxes;
            dr["Total_Pallets"] = totalpallets;
            dr["TotalCashIn"] = totalcashIN;
            dr["Total_KG"] = totalKG;
            dr["Total_Comission"] = totalcomissions;
            dr["Total_Paid_To_Company"] = totalpaidincompany;
            dr["Total_Paid_Europe"] = totalpaidinEurope;

            dtresult.Rows.Add(dr);



            return dtresult;
        }


        private DataTable GenerateExportedData_SourceTable()
        {
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
            DateTime? sd = dt_postdateStart.SelectedDate;
            DateTime? ed = dt_postdateEnd.SelectedDate;

            DataTable dtsource = clientCodeDA.Generate_Country_Summary_View(selectedbranch, selectedcountry, selectedagent, shipmentNo, sd, ed);
            DataView dv_source = new DataView(dtsource);
            DataTable dt_distinctCurrencies = dv_source.ToTable(true, "Currency_Type");
            var dt_distinctCurrencies_Names = dt_distinctCurrencies.AsEnumerable().Select(r => r.Field<string>("Currency_Type")).ToList();


            DataTable dtresult = new DataTable();
            dtresult.Columns.Add("TruckNO", typeof(System.String));
            dtresult.Columns["TruckNO"].Caption = "Truck NO";
            dtresult.Columns.Add("AgentName", typeof(System.String));
            dtresult.Columns["AgentName"].Caption = "Agent Kurdistan / IQ";
          //  dtresult.Columns.Add("Country", typeof(System.String));
          //  dtresult.Columns.Add("Total_Codes", typeof(System.Double));
            
            dtresult.Columns.Add("Total_Boxes", typeof(System.Double));
            dtresult.Columns["Total_Boxes"].Caption = "Box NO";
            dtresult.Columns.Add("Total_Pallets", typeof(System.Double));
            dtresult.Columns["Total_Pallets"].Caption = "Pallet NO";



            dtresult.Columns.Add("Total_Weight_Kg", typeof(System.Double));
            dtresult.Columns["Total_Weight_Kg"].Caption = "Weight Real KG";


            dtresult.Columns.Add("Total_Weight_Vol", typeof(System.Double));
            dtresult.Columns["Total_Weight_Vol"].Caption = "Volume Weight KG";


            dtresult.Columns.Add("Total_Weight_Total", typeof(System.Double));
            dtresult.Columns["Total_Weight_Total"].Caption = "Total Weight KG";

            dtresult.Columns.Add("Total_Packiging_cost_IQD", typeof(System.Double));
            dtresult.Columns["Total_Packiging_cost_IQD"].Caption = "Packiging cost IQD";

            dtresult.Columns.Add("Total_Custome_Cost_Qomrk", typeof(System.Double));
            dtresult.Columns["Total_Custome_Cost_Qomrk"].Caption = "Custom Cost IQD";


            dtresult.Columns.Add("Total_POST_DoorToDoor_IQD", typeof(System.Double));
            dtresult.Columns["Total_POST_DoorToDoor_IQD"].Caption = "POST Door ToDoor IQD";


            dtresult.Columns.Add("Total_Sub_Post_Cost_IQD", typeof(System.Double));
            dtresult.Columns["Total_Sub_Post_Cost_IQD"].Caption = "Sub Post Cost IQD";

            dtresult.Columns.Add("Total_Discount_Post_Cost_Send", typeof(System.Double));
            dtresult.Columns["Total_Discount_Post_Cost_Send"].Caption = "Discount Post Cost Send";



            dtresult.Columns.Add("TotalCashIn", typeof(System.Double));
            dtresult.Columns["TotalCashIn"].Caption = "Total Post Cost IQD";


         //   dtresult.Columns.Add("Total_Comission", typeof(System.Decimal));
            dtresult.Columns.Add("Total_Paid_To_Company", typeof(System.Double));//Total Received IQD
            dtresult.Columns["Total_Paid_To_Company"].Caption = "Total Received IQD";



            // now for each distinct currency we want to add a datatable column
            for (int c = 0; c < dt_distinctCurrencies_Names.Count; c++)
            {
                // add datatable column
                dtresult.Columns.Add(dt_distinctCurrencies_Names[c], typeof(System.Double));//Total Received IQD
                dtresult.Columns[dt_distinctCurrencies_Names[c]].Caption = dt_distinctCurrencies_Names[c] + " Europe To Pay";

            }


         //   dtresult.Columns.Add("Total_Paid_Europe", typeof(System.Double)); // not necessary Europe Maybe we need to change to : total Paid OUtside or export country

            

            DataRow dr;
            dr = dtresult.NewRow();
            // add data 


            dr["TruckNO"] = shipmentNo.ToString();
            dr["AgentName"] = selectedbranch.AgentName;


            object totalpalletsOBJ = dtsource.Compute("Sum(Pallet_No)", "");
            double totalpalletsDBL = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;
            dr["Total_Pallets"] = totalpalletsDBL.ToString();

            object totalboxesOBJ = dtsource.Compute("Sum(Box_No)", "");
            double totalboxesDBL = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;
            dr["Total_Boxes"] = totalboxesDBL.ToString();


            object totalWeight_KgOBJ = dtsource.Compute("Sum(Weight_Kg)", "");
            double totalWeight_KgDBL = totalWeight_KgOBJ == DBNull.Value ? 0 : (double)totalWeight_KgOBJ;
            dr["Total_Weight_Kg"] = totalWeight_KgDBL.ToString();

            object totalWeight_VolOBJ = dtsource.Compute("Sum(Weight_Vol)", "");
            double totalWeight_VolDBL = totalWeight_VolOBJ == DBNull.Value ? 0 : (double)totalWeight_VolOBJ;
            dr["Total_Weight_Vol"] = totalWeight_VolDBL.ToString();


            object totalWeight_TotalOBJ = dtsource.Compute("Sum(Weight_Total)", "");
            double totalWeight_TotalDBL = totalWeight_TotalOBJ == DBNull.Value ? 0 : (double)totalWeight_TotalOBJ;
            dr["Total_Weight_Total"] = totalWeight_TotalDBL.ToString();


            object totalPackiging_cost_IQDOBJ = dtsource.Compute("Sum(Packiging_cost_IQD)", "");
            double totalPackiging_cost_IQDDBL = totalPackiging_cost_IQDOBJ == DBNull.Value ? 0 : (double)totalPackiging_cost_IQDOBJ;
            dr["Total_Packiging_cost_IQD"] = totalPackiging_cost_IQDDBL.ToString();


            object totalCustome_Cost_QomrkOBJ = dtsource.Compute("Sum(Custome_Cost_Qomrk)", "");
            double totalCustome_Cost_QomrkDBL = totalCustome_Cost_QomrkOBJ == DBNull.Value ? 0 : (double)totalCustome_Cost_QomrkOBJ;
            dr["Total_Custome_Cost_Qomrk"] = totalCustome_Cost_QomrkDBL.ToString();


            object totalPOST_DoorToDoor_IQDOBJ = dtsource.Compute("Sum(POST_DoorToDoor_IQD)", "");
            double totalPOST_DoorToDoor_IQDDBL = totalPOST_DoorToDoor_IQDOBJ == DBNull.Value ? 0 : (double)totalPOST_DoorToDoor_IQDOBJ;
            dr["Total_POST_DoorToDoor_IQD"] = totalPOST_DoorToDoor_IQDDBL.ToString();


            object totalSub_Post_Cost_IQDOBJ = dtsource.Compute("Sum(Sub_Post_Cost_IQD)", "");
            double totalSub_Post_Cost_IQDDBL = totalSub_Post_Cost_IQDOBJ == DBNull.Value ? 0 : (double)totalSub_Post_Cost_IQDOBJ;
            dr["Total_Sub_Post_Cost_IQD"] = totalSub_Post_Cost_IQDDBL.ToString();


            object totalDiscount_Post_Cost_SendOBJ = dtsource.Compute("Sum(Discount_Post_Cost_Send)", "");
            double totalDiscount_Post_Cost_SendDBL = totalDiscount_Post_Cost_SendOBJ == DBNull.Value ? 0 : (double)totalDiscount_Post_Cost_SendOBJ;
            dr["Total_Discount_Post_Cost_Send"] = totalDiscount_Post_Cost_SendDBL.ToString();


            object totalTotal_Post_Cost_IQDOBJ = dtsource.Compute("Sum(Total_Post_Cost_IQD)", "");
            double totalTotal_Post_Cost_IQDDBL = totalTotal_Post_Cost_IQDOBJ == DBNull.Value ? 0 : (double)totalTotal_Post_Cost_IQDOBJ;
            dr["TotalCashIn"] = totalTotal_Post_Cost_IQDDBL.ToString();


            object totalTotalPaid_IQDOBJ = dtsource.Compute("Sum(TotalPaid_IQD)", "");
            double totalTotalPaid_IQDDBL = totalTotalPaid_IQDOBJ == DBNull.Value ? 0 : (double)totalTotalPaid_IQDOBJ;
            dr["Total_Paid_To_Company"] = totalTotalPaid_IQDDBL.ToString();

            // currency columns
            for (int c = 0; c < dt_distinctCurrencies_Names.Count; c++)
            {
                object totalEuropaToPayOBJ = dtsource.Compute("Sum(EuropaToPay)", "Currency_Type='"+ dt_distinctCurrencies_Names [c]+ "'");
                double totalEuropaToPayDBL = totalEuropaToPayOBJ == DBNull.Value ? 0 : (double)totalEuropaToPayOBJ;
                dr[dt_distinctCurrencies_Names[c]] = totalEuropaToPayDBL.ToString();
            }

            dtresult.Rows.Add(dr);



            // sum row 

            #region sum row
            dr = dtresult.NewRow();
            //  


            dr["TruckNO"] = string.Empty;
            dr["AgentName"] = "Total Values";


            totalpalletsOBJ = dtresult.Compute("Sum(Total_Pallets)", "");
            totalpalletsDBL = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;
            dr["Total_Pallets"] = totalpalletsDBL.ToString();

             totalboxesOBJ = dtresult.Compute("Sum(Total_Boxes)", "");
             totalboxesDBL = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;
            dr["Total_Boxes"] = totalboxesDBL.ToString();


             totalWeight_KgOBJ = dtresult.Compute("Sum(Total_Weight_Kg)", "");
             totalWeight_KgDBL = totalWeight_KgOBJ == DBNull.Value ? 0 : (double)totalWeight_KgOBJ;
            dr["Total_Weight_Kg"] = totalWeight_KgDBL.ToString();

             totalWeight_VolOBJ = dtresult.Compute("Sum(Total_Weight_Vol)", "");
             totalWeight_VolDBL = totalWeight_VolOBJ == DBNull.Value ? 0 : (double)totalWeight_VolOBJ;
            dr["Total_Weight_Vol"] = totalWeight_VolDBL.ToString();


             totalWeight_TotalOBJ = dtresult.Compute("Sum(Total_Weight_Total)", "");
             totalWeight_TotalDBL = totalWeight_TotalOBJ == DBNull.Value ? 0 : (double)totalWeight_TotalOBJ;
            dr["Total_Weight_Total"] = totalWeight_TotalDBL.ToString();


             totalPackiging_cost_IQDOBJ = dtresult.Compute("Sum(Total_Packiging_cost_IQD)", "");
             totalPackiging_cost_IQDDBL = totalPackiging_cost_IQDOBJ == DBNull.Value ? 0 : (double)totalPackiging_cost_IQDOBJ;
            dr["Total_Packiging_cost_IQD"] = totalPackiging_cost_IQDDBL.ToString();


             totalCustome_Cost_QomrkOBJ = dtresult.Compute("Sum(Total_Custome_Cost_Qomrk)", "");
             totalCustome_Cost_QomrkDBL = totalCustome_Cost_QomrkOBJ == DBNull.Value ? 0 : (double)totalCustome_Cost_QomrkOBJ;
            dr["Total_Custome_Cost_Qomrk"] = totalCustome_Cost_QomrkDBL.ToString();


             totalPOST_DoorToDoor_IQDOBJ = dtresult.Compute("Sum(Total_POST_DoorToDoor_IQD)", "");
             totalPOST_DoorToDoor_IQDDBL = totalPOST_DoorToDoor_IQDOBJ == DBNull.Value ? 0 : (double)totalPOST_DoorToDoor_IQDOBJ;
            dr["Total_POST_DoorToDoor_IQD"] = totalPOST_DoorToDoor_IQDDBL.ToString();


             totalSub_Post_Cost_IQDOBJ = dtresult.Compute("Sum(Total_Sub_Post_Cost_IQD)", "");
             totalSub_Post_Cost_IQDDBL = totalSub_Post_Cost_IQDOBJ == DBNull.Value ? 0 : (double)totalSub_Post_Cost_IQDOBJ;
            dr["Total_Sub_Post_Cost_IQD"] = totalSub_Post_Cost_IQDDBL.ToString();


             totalDiscount_Post_Cost_SendOBJ = dtresult.Compute("Sum(Total_Discount_Post_Cost_Send)", "");
             totalDiscount_Post_Cost_SendDBL = totalDiscount_Post_Cost_SendOBJ == DBNull.Value ? 0 : (double)totalDiscount_Post_Cost_SendOBJ;
            dr["Total_Discount_Post_Cost_Send"] = totalDiscount_Post_Cost_SendDBL.ToString();


             totalTotal_Post_Cost_IQDOBJ = dtresult.Compute("Sum(TotalCashIn)", "");
             totalTotal_Post_Cost_IQDDBL = totalTotal_Post_Cost_IQDOBJ == DBNull.Value ? 0 : (double)totalTotal_Post_Cost_IQDOBJ;
            dr["TotalCashIn"] = totalTotal_Post_Cost_IQDDBL.ToString();


             totalTotalPaid_IQDOBJ = dtresult.Compute("Sum(Total_Paid_To_Company)", "");
             totalTotalPaid_IQDDBL = totalTotalPaid_IQDOBJ == DBNull.Value ? 0 : (double)totalTotalPaid_IQDOBJ;
            dr["Total_Paid_To_Company"] = totalTotalPaid_IQDDBL.ToString();

            // currency columns
            for (int c = 0; c < dt_distinctCurrencies_Names.Count; c++)
            {
                string expression = string.Format("{0}({1})","sum", dt_distinctCurrencies_Names[c]);
                object totalEuropaToPayOBJ = dtresult.Compute(expression, "");
                double totalEuropaToPayDBL = totalEuropaToPayOBJ == DBNull.Value ? 0 : (double)totalEuropaToPayOBJ;
                dr[dt_distinctCurrencies_Names[c]] = totalEuropaToPayDBL.ToString();
            }

            dtresult.Rows.Add(dr);
            #endregion


            return dtresult;
        }

        private void cmb_officeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }

            Perform_Office_SelectionChanged();
            generateSummary();
        }

        private void Perform_Office_SelectionChanged()
        {
            // branch  selection changes
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            if (selectedbranch == null)
            {
                // reset 
                //  codes = new ObservableCollection<ClientCode>();
                shipmentnumbers = new ObservableCollection<ShipmentNumbers>();
            }
            else
            {
                shipmentnumbers = new ObservableCollection<ShipmentNumbers>();
                List<ClientCode> _codes = clientCodeDA.GetClientCodes();
                 ObservableCollection<double?>numbers= new ObservableCollection<double?>(_codes.Where(c => c.BranchId == selectedbranch.Id).OrderByDescending(xx => xx.Shipment_No).Select(x => x.Shipment_No).Distinct().ToList());
                 shipmentnumbers = ShipmentNumbersHelper.formShipmentNumbers(numbers);
            }
        }

        private void cmb_EUCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }
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
                    Id = 0, AgentName = "ALL"
                };
                agents.Insert(0, agentAll);

                if (selectedcountry.CountryName == "ALL")
                {
                    cmb_AgentCity.SelectedIndex = 0;//ALL
                }
            }


            generateSummary();
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            if (e.Column.Header.ToString() == "Id"|| e.Column.Header.ToString() == "Total_WeightReal" || e.Column.Header.ToString() == "Total_VolumneWeight"
                || e.Column.Header.ToString() == "Total_Packiging_cost" || e.Column.Header.ToString() == "Total_Custom_Cost"
                 || e.Column.Header.ToString() == "Total_Post_Door_to_Door" || e.Column.Header.ToString() == "Total_Sub_Post_Cost"
                  || e.Column.Header.ToString() == "Total_Discount_Post_Cost" || e.Column.Header.ToString() == "Currency")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString()== "CountryName")
            {
               
                e.Column.CellStyle = new Style(typeof(DataGridCell));
                e.Column.CellStyle.Setters.Add(new Setter(DataGridCell.BackgroundProperty, firstcolumnbrush));
                e.Column.CellStyle.Setters.Add(new Setter(DataGridCell.HorizontalContentAlignmentProperty,HorizontalAlignment.Center));
                e.Column.CellStyle.Setters.Add(new Setter(DataGridCell.FontWeightProperty, FontWeights.Bold));
            }

            if (e.PropertyType == typeof(double))
            {
                DataGridTextColumn dataGridTextColumn = e.Column as DataGridTextColumn;
                if (dataGridTextColumn != null)
                {
                    dataGridTextColumn.Binding.StringFormat = "{0:#,0.####}";
                }
            }

        }

        private void cmb_TruckNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }
            generateSummary();
        }

        private void cmb_AgentCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }
            generateSummary();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // export to excel

            try
            {
                //    DataTable dt = ConvertfromCountrySummaryTODatatable();
                DataTable dt = GenerateExportedData_SourceTable();
                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
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

                        // exporthelper.Export_To_Excel(dt, outputfilename,false,false);
                        exporthelper.Export_Summary_To_Excel(dt, outputfilename, false, false,true);
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

        private string formFileName(string extension)
        {
            string result = string.Empty;
            var selectedcountry = cmb_EUCountry.SelectedItem as Country;
            var selectedbranch = cmb_officeName.SelectedItem as Agent;
            var selectedagent = cmb_AgentCity.SelectedItem as Agent;
            double shipmentNo = cmb_TruckNo.SelectedItem == null ? 0 : (double)cmb_TruckNo.SelectedValue;
         

            result += selectedbranch.AgentName + "_";
            result += "Truck_";


            string shipmentNostr = shipmentNo == 0 ? "All" : shipmentNo.ToString();
            result += shipmentNostr;

            if (selectedcountry != null)
            {
                result += "_Country_";
                result += selectedcountry.CountryName + "_";
            }
            if (selectedagent != null)
            {
                result += "_Agent_";
                result += selectedagent.AgentName + "_";
            }
            result += extension;

            return result;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // export to pdf
            try
            {
                // DataTable dt = ConvertfromCountrySummaryTODatatable();
                DataTable dt = GenerateExportedData_SourceTable();

                using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
                {
                    System.Windows.Forms.DialogResult result = fbd.ShowDialog();
                    if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
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

                        if (selectedbranch != null)
                        {
                            header += string.Format("{0} : {1} ", "Agent Kurdistan / IQ", selectedBranch.AgentName);
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
                        header += Environment.NewLine;
                        header += Environment.NewLine;

                        // exporthelper.exporttopdf(header,dt, false, outputfilename);

                        //exportSummarytopdf
                        exporthelper.exportSummarytopdf(header, dt, true, outputfilename);
              
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
    }
}
