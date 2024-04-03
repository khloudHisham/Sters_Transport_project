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
    /// Interaction logic for InvoicePreviewWindow.xaml
    /// </summary>
    public partial class InvoicePreviewWindow : Window
    {
        public string Code { get; set; }
        public InvoicePreviewWindow()
        {
            InitializeComponent();
        }
        public InvoicePreviewWindow(string _code)
        {
            InitializeComponent();
            Code = _code;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateDocument(Code);
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
               // CodeInvoice2PRSN codeInvoice_PRSN = new CodeInvoice2PRSN();
                double lh = 15;
                double fs = 10;
                codeInvoice_PRSN.generateDocument(flowdocument, code, lh, fs); // we can pass as reference too .. }

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
             
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                // print ...
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == false) { return; };



                double w = printDialog.PrintableAreaWidth;
                double h = printDialog.PrintableAreaHeight;



           //     flowdocument.PageHeight = printDialog.PrintableAreaHeight;
             //   flowdocument.PageWidth = printDialog.PrintableAreaWidth;




                /*
                PrintCapabilities pc = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                var Pc1 = pc.PageMediaSizeCapability.Where(x => x.PageMediaSizeName == PageMediaSizeName.ISOA5).FirstOrDefault();
                if (Pc1 != null)
                {
                    // landscape invert dimensions...
                    w = (double)Pc1.Height;
                    h = (double)Pc1.Width;
                }
                */




                /*
                  if (Pc1 != null)
                  {
                     // flowdocument.PageHeight = (double)Pc1.Height;
                    //  flowdocument.PageWidth = (double)Pc1.Width;

                      // invert the dimensions to memic oreintation landscape
                      // not the best .. try to replace later
                      flowdocument.PageHeight = (double)Pc1.Width;
                      flowdocument.PageWidth = (double)Pc1.Height;
                  }

                */


                //  lanscape
                //  PrintTicket printTicket = printDialog.PrintQueue.UserPrintTicket;
                //  printTicket.PageOrientation = PageOrientation.Landscape; ;


                //   flowdocument.PagePadding = new Thickness(12);
                //   flowdocument.ColumnGap = 0;

                // avoid multi column

                //  flowdocument.ColumnWidth = (flowdocument.PageWidth -flowdocument.ColumnGap -flowdocument.PagePadding.Left - flowdocument.PagePadding.Right);

                //  flowdocument.ColumnWidth = (flowdocument.PageWidth - flowdocument.ColumnGap - flowdocument.PagePadding.Left-2 -flowdocument.PagePadding.Right-2);

                if (chk_useDefaultCopyCount.IsChecked ?? true)
                {
                    printDialog.PrintTicket.CopyCount = 2; // to be tested on real printer cause virtual printer dont support more than one copy 
                }
               


                Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
                printHelper.showpreview(w, h, flowdocument);
                //showpreview(w, h);
                //    printDialog.PrintDocument(
                //        ((IDocumentPaginatorSource)flowdocument).DocumentPaginator,
                //     "A Flow Document");
            }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }
            
        }
     
    }
}
