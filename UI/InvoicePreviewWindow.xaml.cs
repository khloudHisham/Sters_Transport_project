using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using StersTransport.DataAccess;
using StersTransport.GlobalData;
using StersTransport.Helpers;
using StersTransport.Models;
using StersTransport.Presentation;
using PrintDialog = System.Windows.Controls.PrintDialog;



namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for InvoicePreviewWindow.xaml
    /// </summary>
    public partial class InvoicePreviewWindow : Window
    {

        public string Code { get; set; }
        public bool IsPost {  get; set;}
        public string Lang { get; set; }
        public InvoicePreviewWindow()
        {
            InitializeComponent();
        }
        public InvoicePreviewWindow(string _code)
        {
            InitializeComponent();
            Code = _code;

        }

        public InvoicePreviewWindow(string _code,bool _ispost,string lang )
        {
            InitializeComponent();
            Code = _code;
            IsPost = _ispost;
            Lang = lang;
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
                 //CodeInvoice3PRSN codeInvoice_PRSN = new CodeInvoice3PRSN();
                 //CodeInvoice4PRSN codeInvoice_PRSN = new CodeInvoice4PRSN();


                // CodeInvoice2PRSN codeInvoice_PRSN = new CodeInvoice2PRSN();
                //CodeInvoice_PRSN codeInvoice_PRSN = new CodeInvoice_PRSN();

                //CodeLabel_PRSN codeInvoice_PRSN = new CodeLabel_PRSN();
                double lh = 15;
                double fs = 10;

                // codeInvoice_PRSN.generateDocument(flowdocument, code, lh, fs); // we can pass as reference too .. }

                if (IsPost)
                {
                    if(Lang != "En" || Lang == null)
                    {
                        CodeInvoicePost_4PRSN codeInvoice_PRSN = new CodeInvoicePost_4PRSN();
                        codeInvoice_PRSN.GenerateDocument(frame, code);
                    }
                    else
                    {
                        CodeInvoicePost_4PRSNEnglish codeInvoice_PRSN = new CodeInvoicePost_4PRSNEnglish();
                        codeInvoice_PRSN.GenerateDocument(frame, code);
                    }
                    

                }
                else
                {
                    if (Lang != "En" || Lang == null)
                    {
                        CodeInvoiceOffice_4PRSN codeInvoice_PRSN = new CodeInvoiceOffice_4PRSN();
                        codeInvoice_PRSN.GenerateDocument(frame, code);
                    }
                    else
                    {
                        CodeInvoiceOffice_4PRSNEnglish codeInvoice_PRSN = new CodeInvoiceOffice_4PRSNEnglish();
                        codeInvoice_PRSN.GenerateDocument(frame, code);
                    }
                }



            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.ToString()); }

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



                //    flowdocument.PageHeight = printDialog.PrintableAreaHeight;
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


                //new comment

                if (chk_useDefaultCopyCount.IsChecked ?? true)
                {
                    printDialog.PrintTicket.CopyCount = 2; // to be tested on real printer cause virtual printer dont support more than one copy 
                }

                ///////////////////



                Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
                //new comment 
                //printHelper.showpreview(w, h, flowdocument);

                ////////

                //showpreview(w, h);
                //printDialog.PrintDocument(
                //    ((IDocumentPaginatorSource)frame).DocumentPaginator,
                // "A Flow Document");
            }
            catch (Exception ex)
            { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); }
            
        }

      

        private void print(object sender, RoutedEventArgs e)
        {

            PrintDialog dialog = new PrintDialog();
            dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA4);

            // Set printer page orientation to portrait
            dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;
            if (dialog.ShowDialog() == true)
            {
                double a4Width = dialog.PrintableAreaWidth;
                double a4Height = dialog.PrintableAreaHeight;

                //Calculate the scale factor to fit the content within the A4 dimensions
                double scaleX = a4Width / frame.ActualWidth;
                double scaleY = a4Height / frame.ActualHeight;
                double scale = Math.Min(scaleX, scaleY); // Choose the smaller scale factor to fit within both dimensions

                // Apply the scale transform to the visual content
                frame.LayoutTransform = new ScaleTransform(scale, scale);
                dialog.PrintVisual(frame, "");
            }
            
        }



    }
}
