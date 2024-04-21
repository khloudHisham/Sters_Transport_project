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
    public partial class LabelPreviewWindow : Window
    {
        public string Code { get; set; }
        public bool IsPOST { get; set; }

        public LabelPreviewWindow()
        {
            InitializeComponent();
        }

        public LabelPreviewWindow(string _code,bool _ispost)
        {
            InitializeComponent(); 
            
            Code = _code;
            IsPOST = _ispost;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateDocument(Code,IsPOST);
        }

        public void GenerateDocument(string code,bool ispost)
        {
            try
            {
                if (ispost)
                {
                    //CodeLabelPost_PRSN codeLabel_PRSN = new CodeLabelPost_PRSN();
                    //codeLabel_PRSN.generateDocument(flowdocument, code);

                    CodeLabelPost_PRSN_2 codeLabel_PRSN_2 = new CodeLabelPost_PRSN_2();
                    codeLabel_PRSN_2.generateDocument(label, code);
                }
                else
                {
                    CodeLabelOffice_PRSN codeLabelOffice_PRSN = new CodeLabelOffice_PRSN();
                    //codeLabelOffice_PRSN.generateDocument(flowdocument, code);

                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.ToString()); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    PrintDialog printDialog = new PrintDialog();
            //    if (printDialog.ShowDialog() == false) 
            //        return; 

            //    double w = printDialog.PrintableAreaWidth;
            //    double h = printDialog.PrintableAreaHeight;

            //    Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
            //    printHelper.showpreview(w, h, flowdocument);
            //}
            //catch (Exception ex)
            //{ 
            //    WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); 

            //} 
            
                PrintDialog dialog = new PrintDialog();
                dialog.PrintTicket.PageMediaSize = new PageMediaSize(PageMediaSizeName.ISOA5);

                // Set printer page orientation to portrait
                dialog.PrintTicket.PageOrientation = PageOrientation.Portrait;

                if (dialog.ShowDialog() == true)
                {
                    double a5Width = dialog.PrintableAreaWidth;
                    double a5Height = dialog.PrintableAreaHeight;

                    // Calculate the scale factor to fit the content within the A5 dimensions
                    double scaleX = a5Width / label.ActualWidth;
                    double scaleY = a5Height / label.ActualHeight;
                    double scale = Math.Min(scaleX, scaleY); // Choose the smaller scale factor to fit within both dimensions

                    // Apply the scale transform to the visual content
                    label.LayoutTransform = new ScaleTransform(scale, scale);
                    dialog.PrintVisual(label, "");
                }
            

        }
    }
}