using System;
using System.Collections.Generic;
using System.Data;
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
using StersTransport.DataAccess;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for LabelsManagement.xaml
    /// </summary>
    /// 

    
    public partial class LabelsManagement : UserControl
    {

        LabelsDa labelsDa = new LabelsDa();
        public LabelsManagement()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // save labels 
            DataView dv = dg.ItemsSource as DataView;

          //  DataTable dt = dv.Table;
            DataTable dt = dv.ToTable();
            string errors = string.Empty;
            bool comitted=  labelsDa.update_labels(dt, out errors);
            if (comitted)
            { WpfMessageBox.Show("", GlobalData.CommonMessages.On_Update_Successful, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Information); }
            else
            { WpfMessageBox.Show("", errors, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }


            // refresh data
            OnLoad();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            OnLoad();
        }

        private void OnLoad()
        {
            try
            {
                DataTable dt = labelsDa.Get_Labels();
                dg.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        }

        private void dg_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Keyword")
            {
                e.Column.IsReadOnly = true;
            }
        }
    }
}
