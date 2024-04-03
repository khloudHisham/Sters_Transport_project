using StersTransport.GlobalData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for UIDash.xaml
    /// </summary>
    public partial class UIDash : UserControl
    {
        public UIDash()
        {
            InitializeComponent();
        }

        private void UIDash_Loaded(object sender, RoutedEventArgs e)
        {
             lbl_officeName.Text= LoggedData.LogggedBranch.AgentName + "  Office";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow mainwindow_;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.showsendwindow();
                        //  mainwindow_.tabitemexviewmodel.closetab(this);   // optional
                        break;
                    }
                }

            }
            catch (Exception ex) { WpfMessageBox.Show("",ex.Message,MessageBoxButton.OK,(WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // show reports 
            try
            {
                MainWindow mainwindow_;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.show_reports_ui();
                        //  mainwindow_.tabitemexviewmodel.closetab(this);   // optional
                        break;
                    }
                }

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            // show admin window
            try
            {
                MainWindow mainwindow_;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.show_admin_ui();
                        //  mainwindow_.tabitemexviewmodel.closetab(this);   // optional
                        break;
                    }
                }

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // show settings window
            try
            {
                MainWindow mainwindow_;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.showSettingsUI();
                        //  mainwindow_.tabitemexviewmodel.closetab(this);   // optional
                        break;
                    }
                }

            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }
    }
}
