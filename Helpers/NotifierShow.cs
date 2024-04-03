using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using System.Windows;

namespace StersTransport.Helpers
{
    public static class NotifierShow
    {
        public static void show_notificationMessage(string msg, MessageBoxImage img)
        {
            // show message
            Notifier notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomCenter,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

               

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            if (img ==  MessageBoxImage.Information)
            {
                notifier.ShowInformation(msg);
            }
            else if (img == MessageBoxImage.Warning)
            {
                notifier.ShowWarning(msg);
            }
            else if (img == MessageBoxImage.Error)
            {
                notifier.ShowError(msg);

            }
        }
    }


    
}
