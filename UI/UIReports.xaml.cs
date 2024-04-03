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
    /// Interaction logic for UIReports.xaml
    /// </summary>
    public partial class UIReports : UserControl
    {
        public UIReports()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            tab_overview.Content = new UIReports_Overview();
            tab_reports.Content = new UIReports_Report();
            tab_dashboard.Content = new UIReports_DashBoard();
            tab_Eureports.Content = new UIReports_EUReport();

        }
    }
}
