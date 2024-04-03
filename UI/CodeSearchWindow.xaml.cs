using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using StersTransport.Models;
using StersTransport.DataAccess;
using StersTransport.Enumerations;
using System.Data;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for CodeSearchWindow.xaml
    /// </summary>
    public partial class CodeSearchWindow : Window, INotifyPropertyChanged
    {


        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion


        public string searchedCode { get; set; }

        public bool confirmed = false;


        private windowStatesenum _WindowStatesenum;
        public windowStatesenum WindowStatesenum
        {
            get { return _WindowStatesenum; }
            set 
            {
                _WindowStatesenum = value;
            //    setActionButtonsEnabledStates();
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

        

        public struct searchoption
        {
            public string  displayname { get; set; }
            public string  feildname { get; set; }
        }


        private DataTable dtcodes;
        public DataTable DTCodes
        {
            get { return dtcodes; }
            set
            {
                dtcodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DTCodes"));
            }
        }


        public DataView DataViewCodes { get; set; }
        
        private ObservableCollection<searchoption> searchoptions;
        
        public ObservableCollection<searchoption> SearchOptions
        {
            get { return searchoptions; }
            set
            {
                searchoptions = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SearchOptions"));
            }
        }

        private ObservableCollection<Agent> _branches;
        public ObservableCollection<Agent> Branches
        {
            get { return _branches; }
            set
            {
                _branches = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Branches"));
            }
        }


        private ObservableCollection<Agent> _agents;
        public ObservableCollection<Agent> Agents
        {
            get { return _agents; }
            set
            {
                _agents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Agents"));
            }
        }

        BranchDa branchda = new BranchDa();

        AgentDa AgentDa = new AgentDa();



        public bool dataisloading;

        public CodeSearchWindow()
        {
            InitializeComponent();
        }
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = this.Width;
            double windowHeight = this.Height;
            this.Left = (screenWidth / 2) - (windowWidth / 2);
            this.Top = (screenHeight / 2) - (windowHeight / 2);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.8;
            CenterWindowOnScreen();
            dataisloading = true;


            Branches =new ObservableCollection<Agent> ( AgentDa.GetAgents());
           
       
            Agent brannchAll = new Agent()
            {
                Id = 0,
                AgentName="ALL"
                
            };
            Branches.Insert(0,brannchAll);
 

            cmb_branch.ItemsSource = Branches;

            Agents = new ObservableCollection<Agent>(AgentDa.GetAgents());

            Agent AgentAll = new Agent()
            {
                Id=0,AgentName="ALL"
            }
            ;
            Agents.Insert(0,AgentAll);

            cmb_agent.ItemsSource = Agents;


       
            SearchOptions = new ObservableCollection<searchoption>();
            SearchOptions.Add(new searchoption { displayname = "Sender Name", feildname = "SenderName" });
            SearchOptions.Add(new searchoption { displayname = "Sender Telephone", feildname = "Sender_Tel" });
            SearchOptions.Add(new searchoption { displayname = "Receiver Name", feildname = "ReceiverName" });
            SearchOptions.Add(new searchoption { displayname = "Receiver Telephone", feildname = "Receiver_Tel" });
            SearchOptions.Add(new searchoption { displayname = "Goods Discription", feildname = "Goods_Description" });
            //Client_Code
            SearchOptions.Add(new searchoption { displayname = "Client_Code", feildname = "Client_Code" });

            /*
          lstbx_searchoptions.ItemsSource = SearchOptions;
         */


            this.Dispatcher.Invoke(() =>
            {
                getcodes();
            });
           
         

        }

        private async void getcodes()
        {
          

            adorner1.IsAdornerVisible = true;
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            await Task.Run(() => DTCodes = clientCodeDA.GetClientCodesView1());

            DataViewCodes = new DataView(DTCodes);

            grid_codes.ItemsSource = DataViewCodes;
            adorner1.IsAdornerVisible = false;
            dataisloading = false;


            // try to set the current logged branch as selected 
            try
            {
                selectedBranch = Branches.Where(x => x.Id == GlobalData.LoggedData.LogggedBranch.Id).FirstOrDefault();
            }
            catch (Exception)
            { }

            txt_search.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            cmb_branch.SelectedIndex = -1;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            cmb_agent.SelectedIndex = -1;
        }

        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            selectCode();
        }

        private void selectCode()
        {
            searchedCode = string.Empty;
            DataRowView dr = grid_codes.SelectedItem as DataRowView;
            if (dr != null)
            {
                searchedCode = dr["Client_Code"].ToString();

            }
            CodeSearchWindowSearchForDialog codeSearchWindowSearchForDialog = new CodeSearchWindowSearchForDialog(this);
            codeSearchWindowSearchForDialog.ShowDialog();

            if (confirmed)
            { this.Close(); }
           
        }

        private void cmb_branch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }
            Applyfilter();
        }

        private  void Applyfilter()
        {
            try
            {
                string RowFilterString = "1=1"; // initial ....


                var selectedBranch = cmb_branch.SelectedItem as Agent;
                if (selectedBranch != null)
                {
                    if (selectedBranch.Id != 0)
                    { RowFilterString += " And BranchId='" + selectedBranch.Id + "'"; }
                  
                }

                var selectedAgent = cmb_agent.SelectedItem as Agent;

                if (selectedAgent != null)
                {
                    if (selectedAgent.Id != 0)
                    {   RowFilterString += " And AgentId='" + selectedAgent.Id + "'";}
                  
                }

                string searchtext = txt_search.Text;

                // get the selected items 
                //  var selectedfeilds = lstbx_searchoptions.SelectedItems;

                string stringfeildcreteria = string.Empty;

                if (searchtext.Length>0)
                {
                    for (int c = 0; c < SearchOptions.Count; c++)
                    {

                        //  var so = (searchoption)selectedfeilds[c];
                        var so = SearchOptions[c];
                        if (c == 0)
                        { stringfeildcreteria += " " + so.feildname + " Like '%" + searchtext + "%' "; }
                        else
                        {
                            stringfeildcreteria += "  OR " + so.feildname + " Like '%" + searchtext + "%' ";
                        }
                    }
                }
               

                if (stringfeildcreteria.Length > 0)
                {
                    RowFilterString += string.Format(" And ({0})", stringfeildcreteria);
                }


                DataViewCodes.RowFilter = RowFilterString;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void cmb_agent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataisloading) { return; }

            Applyfilter();



        }

        private void grid_codes_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                selectCode();
                e.Handled = true;
            }
        }

        private void grid_codes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectCode();
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dataisloading) { return; }
            Applyfilter();
        }

        private void grid_codes_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "AgentId"||
                e.Column.Header.ToString() == "CountryAgentId" ||
                e.Column.Header.ToString() == "Person_in_charge_Id" ||
                e.Column.Header.ToString() == "UserName"||
                 e.Column.Header.ToString() == "BranchId" ||
                  e.Column.Header.ToString() == "BranchName"
                )
            {
                e.Cancel = true;
            }
        }
    }
}
