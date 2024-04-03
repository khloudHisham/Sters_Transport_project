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
    /// Interaction logic for UILabel.xaml
    /// </summary>
    public partial class UILabel : UserControl
    {
        ClientCodeDA clientCodeDA = new ClientCodeDA();
        public ObservableCollection<ClientCode> Clientcodes
        {
            get; set;
        }


        public bool IsPOST { get; set; }
        public UILabel()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Clientcodes = new ObservableCollection<ClientCode>(clientCodeDA.GetClientCodes(LoggedData.LogggedBranch.Id, true));
            cmb1.DisplayMemberPath = "Code";
            cmb1.ItemsSource = Clientcodes;

        }



        private void txtcode_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            
        }

        public void GenerateDocument(string code,bool ispost)
        {
            try
            {
                //  string code = txtcode.Text;
                //    CodeLabel_PRSN codeLabel_PRSN = new CodeLabel_PRSN();
                if (ispost)
                {
                    CodeLabelPost_PRSN codeLabel_PRSN = new CodeLabelPost_PRSN();
                    codeLabel_PRSN.generateDocument(flowdocument, code);
                }
                else
                {
                    CodeLabelOffice_PRSN codelblofficeprsn = new CodeLabelOffice_PRSN();
                    codelblofficeprsn.generateDocument(flowdocument, code); 
                    // we can pass as reference too .. }
                }
              

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            { // print ...
                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == false) { return; };

                double w = printDialog.PrintableAreaWidth;
                double h = printDialog.PrintableAreaHeight;

              //   flowdocument.PageHeight = printDialog.PrintableAreaHeight;
              //  flowdocument.PageWidth = printDialog.PrintableAreaWidth;

                // try to hard code width and height if this not work

           
                if (IsPOST)
                {
                    PrintCapabilities pc = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                    var Pc1 = pc.PageMediaSizeCapability.Where(x => x.PageMediaSizeName == PageMediaSizeName.ISOA5).FirstOrDefault();
                    if (Pc1 != null)
                    {
                        h = (double)Pc1.Width;
                        w =  (double)Pc1.Height;
                    }
                }
                else
                {
                    PrintCapabilities pc = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);
                    var Pc1 = pc.PageMediaSizeCapability.Where(x => x.PageMediaSizeName == PageMediaSizeName.ISOA6).FirstOrDefault();
                    if (Pc1 != null)
                    {
                        h = (double)Pc1.Height;
                        w = (double)Pc1.Width;
                    }

                }


                Helpers.PrintHelper printHelper = new Helpers.PrintHelper();
                printHelper.showpreview(w, h, flowdocument);
               // showpreview(w, h);
                //    flowdocument.PagePadding = new Thickness(12);

                //    flowdocument.ColumnGap = 0;
                // avoid multi column
                //    flowdocument.ColumnWidth = (flowdocument.PageWidth -
                //        flowdocument.ColumnGap -
                //        flowdocument.PagePadding.Left -
                //        flowdocument.PagePadding.Right);
                //      printDialog.PrintDocument(
                //     ((IDocumentPaginatorSource)flowdocument).DocumentPaginator,
                //     "A Flow Document");
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
            }

           
        }


    

        private void cmb1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var selectedcode = cmb1.SelectedItem as ClientCode;
                if (selectedcode == null) { return; }

                bool ispost = selectedcode.Have_Local_Post.HasValue ? (bool)selectedcode.Have_Local_Post : false;
                IsPOST = ispost;
                GenerateDocument(selectedcode.Code, ispost);
            }
        }
    }
}
