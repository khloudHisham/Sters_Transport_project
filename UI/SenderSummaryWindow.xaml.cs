using StersTransport.DataAccess;
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
using System.Windows.Shapes;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for SenderSummaryWindow.xaml
    /// </summary>
    public partial class SenderSummaryWindow : Window
    {

        private string _senderTel;
        public string senderTel
        {
            get { return _senderTel; }
            set { _senderTel = value; }
        }
        private string _receiverTel;
        public string receiverTel
        {
            get { return _receiverTel; }
            set { _receiverTel = value; }
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

        public SenderSummaryWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // defaults...
            rdSender.IsChecked = true;
            rdSearchPhove.IsChecked = true;

            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight * 0.8;
            CenterWindowOnScreen();
            txt_search.Text = senderTel;
            PerformSearch();
        }

        private void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txt_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformSearch();
            }
           
           
        }

        private void PerformSearch()
        {

            bool issenderMode = true; bool istelMode = true;

            issenderMode = rdSender.IsChecked.HasValue ? rdSender.IsChecked.Value : false;
            istelMode = rdSearchPhove.IsChecked.HasValue ? rdSearchPhove.IsChecked.Value : false;

            try
            {
                DataTable dt = new DataTable();
                ClientCodeDA clientCodeDA = new ClientCodeDA();
                if (issenderMode)
                {
                    if (istelMode)
                    {
                        senderTel = txt_search.Text;
                        dt = clientCodeDA.get_sender_Codes(senderTel);
                    }
                    else
                    {
                        dt = clientCodeDA.get_sender_Codes_BySenderName(txt_search.Text);
                    }
                  
                }
                else
                {
                    if (istelMode)
                    {
                        receiverTel = txt_search.Text;
                        dt = clientCodeDA.get_receiver_Codes(receiverTel); 
                    }
                    else
                    {
                        dt = clientCodeDA.get_receiver_Codes_ByReceiverName(txt_search.Text);
                    }
                  
                }
               
                dg.ItemsSource = dt.DefaultView;
                // make the sums....
                int totalcodesINT = dt.Select().Length;

                object totalboxesOBJ = dt.Compute("Sum(Box_No)", "");
                double totalboxesDBL = totalboxesOBJ == DBNull.Value ? 0 : (double)totalboxesOBJ;

                object totalpalletsOBJ = dt.Compute("Sum(Pallet_No)", "");
                double totalpalletsDBL = totalpalletsOBJ == DBNull.Value ? 0 : (double)totalpalletsOBJ;

                object totalKGOBJ = dt.Compute("Sum(Weight_Total)", "");
                double totalKGDBL = totalKGOBJ == DBNull.Value ? 0 : (double)totalKGOBJ;

                object totalPaidIRQOBJ = dt.Compute("Sum(TotalPaid_IQD)", "");
                double totalPaidIRQ = totalPaidIRQOBJ == DBNull.Value ? 0 : (double)totalPaidIRQOBJ;

                object totalEUTOPAYOBJ = dt.Compute("Sum(EuropaToPay)", "");
                double totalEUTOPAY = totalEUTOPAYOBJ == DBNull.Value ? 0 : (double)totalEUTOPAYOBJ;

                txtTotalCodes.Text = totalcodesINT.ToString();

                txtTotalBoxesPallets.Text = string.Format("{0} Box(s) and {1} Pallet(s) ", totalboxesDBL.ToString(), totalpalletsDBL.ToString());

                txtTotalweight.Text = string.Format("{0:#,0.##}", totalKGDBL);
               
                txtTotalPaid.Text = string.Format("{0:#,0.##}", totalPaidIRQ);  

                txtTotalPaidEurope.Text = string.Format("{0:#,0.##}", totalEUTOPAY); 
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
        }
    }
}
