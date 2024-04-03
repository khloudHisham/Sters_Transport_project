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

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIAdmin.xaml
    /// </summary>
    public partial class UIAdmin : UserControl
    {

        bool windowisloaded = false;
        public UIAdmin()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!windowisloaded)
            {
                tab_agent.Content = new UIAgent();
               // tab_branch.Content = new UIBranches();
                tab_country.Content = new UICountry();
                tab_city.Content = new UICity();
                tab_currency.Content = new UICurrency();
                tab_user.Content = new UIUser();
                windowisloaded = true;
            }
    
        }
    }
}
