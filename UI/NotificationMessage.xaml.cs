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
using  StersTransport.Enumerations;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for NotificationMessage.xaml
    /// </summary>
    public partial class NotificationMessage : UserControl
    {
        public event EventHandler OnClose;

        private string _Message;
        private string _Title;
        private MessagesTypes.messagestypes _MessageType;
        public Button CloseButton { get; set; }
        public string Title
        {
            get { return _Title; }
            set { _Title = value; txtblocktitle.Text = _Title; }
        }
        public string Message { get { return _Message; } set { _Message = value; txtblockmessage.Text = _Message; } }
        SolidColorBrush infocolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#236BC9"));
        SolidColorBrush errorcolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#ec2424"));
        SolidColorBrush warningcolor = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E89933"));
        public MessagesTypes.messagestypes MessageType
        {
            get { return _MessageType; }
            set
            {
                _MessageType = value;
                switch (_MessageType)
                {
                    case MessagesTypes.messagestypes.info:
                        {

                            txtblocktitle.Background = infocolor;

                            txtblockicon.Text = "\uf05a";
                            brdr.BorderBrush = infocolor;

                            txtblockicon.Background = infocolor;
                            brdr.Background = infocolor;
                            txtblockmessage.Background = infocolor;
                            break;
                        }
                    case MessagesTypes.messagestypes.warning:
                        {
                            txtblocktitle.Background = warningcolor;

                            txtblockicon.Text = "\uf071";
                            brdr.BorderBrush = warningcolor;
                            txtblockicon.Background = warningcolor;
                            brdr.Background = warningcolor;
                            txtblockmessage.Background = warningcolor;

                            break;
                        }
                    case MessagesTypes.messagestypes.error:
                        {
                            txtblocktitle.Background = errorcolor;

                            txtblockicon.Text = "\uf06a";
                            brdr.BorderBrush = errorcolor;
                            txtblockicon.Background = errorcolor;
                            brdr.Background = errorcolor;
                            txtblockmessage.Background = errorcolor;
                            break;
                        }
                }
            }
        }


        public NotificationMessage()
        {
            InitializeComponent();
            CloseButton = btnclose;
            MessageType = MessagesTypes.messagestypes.info;
            Title = string.Empty; Message = string.Empty;
        }

        private void btnclose_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.OnClose != null)
            {
                OnClose(this, e);
            }
        }
    }
}
