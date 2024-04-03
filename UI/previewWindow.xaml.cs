using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for previewWindow.xaml
    /// </summary>
    public partial class previewWindow : Window
    {
        public IDocumentPaginatorSource Document
        {
            get { return viewer.Document; }
            set { viewer.Document = value; }
        }
        public previewWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pq = LocalPrintServer.GetDefaultPrintQueue(); 
                 var writer = PrintQueue.CreateXpsDocumentWriter(pq);
                var paginator = viewer.Document.DocumentPaginator;
                writer.Write(paginator);
            }
            catch (Exception ex) 
            { MessageBox.Show(ex.Message); }
               
        }
    }
}
