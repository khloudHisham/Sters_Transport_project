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
                    CodeLabelPost_PRSN codeLabel_PRSN = new CodeLabelPost_PRSN();
                    codeLabel_PRSN.generateDocument(flowdocument, code);
                }
                else
                {
                    CodeLabelOffice_PRSN codeLabelOffice_PRSN = new CodeLabelOffice_PRSN();
                    codeLabelOffice_PRSN.generateDocument(flowdocument, code);
                }
            }
            catch (Exception ex) 
            { 
                MessageBox.Show(ex.ToString()); 
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == false) 
                    return; 

                double w = printDialog.PrintableAreaWidth;
                double h = printDialog.PrintableAreaHeight;

                Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
                printHelper.showpreview(w, h, flowdocument);
            }
            catch (Exception ex)
            { 
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error); 
            } 
        }
    }
}