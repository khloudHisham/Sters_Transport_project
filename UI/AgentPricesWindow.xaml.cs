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
using System.Reflection;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for AgentPricesWindow.xaml
    /// </summary>
    public partial class AgentPricesWindow : Window, INotifyPropertyChanged
    {



        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion



        private long? _AgentID;
        public long? AgentID
        {
            get { return _AgentID; }
            set
            {
                _AgentID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AgentID"));
            }
        }


        private ObservableCollection<Agent_Prices> _agentprices;

        public ObservableCollection<Agent_Prices> agentprices
        {
            get { return _agentprices; }
            set
            {
                _agentprices = value;
                OnPropertyChanged(new PropertyChangedEventArgs("agentprices"));
            }
        }


        private ObservableCollection<Agent> _destinationAgents;

        public ObservableCollection<Agent> destinationAgents
        {
            get { return _destinationAgents; }
            set
            {
                _destinationAgents = value;
                OnPropertyChanged(new PropertyChangedEventArgs("destinationAgents"));
            }
        }



        AgentDa AgentDa = new AgentDa();
        Agent_PricesDa agent_PricesDa = new Agent_PricesDa();


        public AgentPricesWindow()
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

        private void UI_AgentPrices_Loaded(object sender, RoutedEventArgs e)
        {
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.8;
            CenterWindowOnScreen();
            // get the agent prices of the agent id
            try
            {
                Load_Agent_Prices();
                Load_DestinationAgents();
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); 
            }
          
        }


        public void Load_Agent_Prices()
        {
            if (AgentID == null)
            { return; }

            agentprices = new ObservableCollection<Agent_Prices>(agent_PricesDa.GetAgent_Prices((long)AgentID));

        }

        public void Load_DestinationAgents()
        {
            // get the country of the loaded agent ...
            if (AgentID == null)
            { return; }

            var agent = AgentDa.GetAgent((long)AgentID);
            if (agent == null) { return; }


            long countryID= agent.CountryId.HasValue ? (long)agent.CountryId : 0;
            if (countryID == 0) { return; }


            //    destinationAgents = new ObservableCollection<Agent>(AgentDa.GetAgents_ExludedCountry(countryID));

            destinationAgents = new ObservableCollection<Agent>(AgentDa.GetAgents_ExludedAgent((long)AgentID));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // save agent prices 

            // when saving make sure that all the destination agent really exists in the database .... cause whe dont make a relation ...
            // no repeated  destination agent..

            try
            {
                if (AgentID.HasValue == false) { return; }


                validate();
                if (agentprices.Any(x => x.HasErrors))
                {
                    return;
                }

                // no repeated values in the list 
                 var agentpriceslst = agentprices.ToList();
                if (agentpriceslst.Count != agentpriceslst.Select(x=>x.Agent_Id_Destination).Distinct().Count())
                {
                    WpfMessageBox.Show("", "List Contains Duplicate Entries...", MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }


                // proceede...
                //assign the agent id 
                for (int c = 0; c < agentprices.Count; c++)
                {
                    agentprices[c].Agent_Id = AgentID;
                }
                string errormessage = string.Empty;
                agent_PricesDa.Update_AgentPrices(agentprices.ToList(), (long)AgentID, out errormessage);
                if (errormessage.Length > 0)
                {
                    WpfMessageBox.Show("", errormessage, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                    return;
                }
                WpfMessageBox.Show("", GlobalData.CommonMessages.On_Update_Successful, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information);
                // reload .....
                Load_Agent_Prices();

            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }



        }

        private void validate()
        {

            foreach (var agentprice in agentprices)
            {
                agentprice.isvalidating = true;
                PropertyInfo[] properties = typeof(Agent_Prices).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    try
                    { property.SetValue(agentprice, property.GetValue(agentprice, null)); }
                    catch (Exception) { }

                }
                agentprice.isvalidating = false;

                
            }
            
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {

            // cancel 

            if (e.Column.Header.ToString() == "virtual_Agent")
            {
                e.Cancel = true;
            }
            if (e.Column.Header.ToString() == "virtual_Agent1")
            {
                e.Cancel = true;
            }
            


            // hiddern...
            if (e.Column.Header.ToString() == "HasErrors")
            {
                e.Column.Visibility = System.Windows.Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "isvalidating")
            {
                e.Column.Visibility = System.Windows.Visibility.Hidden;
            }

          
            if (e.Column.Header.ToString() == "ID")
            {
                e.Column.Visibility = System.Windows.Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "Agent_Id")
            {
                e.Column.Visibility = System.Windows.Visibility.Hidden;
            }
            if (e.Column.Header.ToString() == "Agent_Id_Destination")
            {
                e.Column.Visibility = System.Windows.Visibility.Hidden;
            }
       


        }

        private void DataGrid_AutoGeneratedColumns(object sender, EventArgs e)
        {
            // display index
            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Destination") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Destination").DisplayIndex = 3;

            }

            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Delete") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Delete").DisplayIndex = grd.Columns.Count - 1;

            }


            // headers 

            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "CurrencyEQ") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "CurrencyEQ").Header = "Currency Equalizer";
            }

            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "BoxPackaging") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "BoxPackaging").Header = "Box Packaging Cost";
            }


            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Price1to5_7KG") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "Price1to5_7KG").Header = "Minimum Price";
            }

            if (grd.Columns.FirstOrDefault(x => x.Header.ToString() == "PriceKG") != null)
            {
                grd.Columns.FirstOrDefault(x => x.Header.ToString() == "PriceKG").Header = "Price Per KG";
            }


        }

        private void DataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
          //  e.NewItem = new Agent_Prices { Agent_Id = AgentID };
        }

        private void DataGrid_AddingNewItem_1(object sender, AddingNewItemEventArgs e)
        {
               e.NewItem = new Agent_Prices { ID=0,Agent_Id=AgentID };
        }

        private void grd_Selected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                var currentItem = grd.CurrentItem as Agent_Prices;
                agentprices.Remove(currentItem);
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        }
    }
}
