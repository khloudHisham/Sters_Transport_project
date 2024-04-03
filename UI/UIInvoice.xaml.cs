using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Printing;
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
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using StersTransport.DataAccess;
using StersTransport.GlobalData;
using StersTransport.Models;
using StersTransport.Presentation;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIInvoice.xaml
    /// </summary>
    public partial class UIInvoice : UserControl
    {

        ClientCodeDA clientCodeDA = new ClientCodeDA();
        public ObservableCollection<ClientCode> Clientcodes
        {
            get;set;
        }
        public UIInvoice()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Clientcodes = new ObservableCollection<ClientCode>(clientCodeDA.GetClientCodes(LoggedData.LogggedBranch.Id,true));
            cmb1.DisplayMemberPath = "Code";
            cmb1.ItemsSource = Clientcodes;
             
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {



      
            

            try
            {
                System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();

                if (pd.ShowDialog() == true)
                {
                 
                    double w= pd.PrintableAreaWidth;
                    double h = pd.PrintableAreaHeight;
                    // print ...


                    //     flowdocument.PageHeight = pd.PrintableAreaHeight;
                    //    flowdocument.PageWidth = pd.PrintableAreaWidth;


                    PrintCapabilities pc = pd.PrintQueue.GetPrintCapabilities(pd.PrintTicket);
                    var Pc1 = pc.PageMediaSizeCapability.Where(x => x.PageMediaSizeName == PageMediaSizeName.ISOA5).FirstOrDefault();
                    //    var xz = pc.PageOrientationCapability[0];
                    if (Pc1 != null)
                    {
                        // landscape invert dimensions...
                        w = (double)Pc1.Height;
                        h = (double)Pc1.Width;
                    }

                    //   flowdocument.PagePadding = new Thickness(12);
                    //     flowdocument.ColumnGap = 0;
                    // avoid multi column
                    //     flowdocument.ColumnWidth = (flowdocument.PageWidth -
                    //      flowdocument.ColumnGap -
                    //   flowdocument.PagePadding.Left -
                    //    flowdocument.PagePadding.Right);



                    
                    if (chk_useDefaultCopyCount.IsChecked ?? true)
                    {
                        pd.PrintTicket.CopyCount = 2; // to be tested on real printer cause virtual printer dont support more than one copy.PrintTicket.CopyCount = 2; // to be tested on real printer cause virtual printer dont support more than one copy.PrintTicket.CopyCount = 2; // to be tested on real printer cause virtual printer dont support more than one copy

                    }



                    //  DocumentPaginator paginator = ((IDocumentPaginatorSource)flowdocument).DocumentPaginator;
                    // to forece and set manually
                    //   flowdocument.PageHeight = 600; flowdocument.PageWidth = 400;


                    Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
                    printHelper.showpreview(w, h, flowdocument);
                    // showpreview(w, h);
                    //  pd.PrintDocument(paginator, "my");




                  
                  
                    
                   
                   
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }
            

           
        }

    

        private void txtcode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.Key == Key.Enter)
            {
                GenerateDocument(cmb1.Text);
            }
            */
        }

        public void GenerateDocument(string code)
        { // just testing things....

            /*
            try
            {
                 
                CodeInvoice_PRSN codeInvoice_PRSN = new CodeInvoice_PRSN();
                double lh = 15;
                double fs = 10;
                codeInvoice_PRSN.generateDocument(flowdocument, code, lh, fs); // we can pass as reference too .. }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            */


            try
            {

                CodeInvoice3PRSN codeInvoice_PRSN = new CodeInvoice3PRSN();
                double lh = 15;
                double fs = 10;
                codeInvoice_PRSN.generateDocument(flowdocument, code, lh, fs); // we can pass as reference too .. }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }


        }

        private void cmb1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                GenerateDocument(cmb1.Text);
            }
        }

        public static implicit operator Window(UIInvoice v)
        {
            throw new NotImplementedException();
        }
    }
}
