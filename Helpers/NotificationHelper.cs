using StersTransport.Enumerations;
using StersTransport.GlobalData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StersTransport.Helpers
{
    public class NotificationHelper
    {
        public void showMainNotification(string message, MessagesTypes.messagestypes messagestypes,int ms)
        {
            try
            {
                MainWindow mainwindow_;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        mainwindow_.show_notification_message("", message, messagestypes, ms);
                        break;
                    }
                }
            }
            catch (Exception ex) { WpfMessageBox.Show("", ex.Message, MessageBoxButton.OK, (WpfMessageBox.MessageBoxImage)MessageBoxImage.Error); }
        }

    }
}
