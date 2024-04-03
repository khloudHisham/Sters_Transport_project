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
using System.Windows.Shapes;

namespace StersTransport.UI
{
    
    /// <summary>
    /// Interaction logic for StreetPickWindow.xaml
    /// </summary>
    public partial class StreetPickWindow : Window
    {
        Send Send;
        public StreetPickWindow()
        {
            InitializeComponent();
        }

        public StreetPickWindow(Send _send)
        {
            InitializeComponent();
            Send = _send;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtblck.Text = Send.streetpickLabel;
            txtbx.Focus();
            Send.streetpickvalue = string.Empty;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            { 
               
                choosevalue();
                e.Handled = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            choosevalue();
        }

        void choosevalue()
        {
            Send.streetpickvalue = txtbx.Text;
            this.Close();
        }
    }
}
