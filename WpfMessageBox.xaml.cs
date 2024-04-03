using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace StersTransport
{
    /// <summary>
    /// Interaction logic for WpfMessageBox.xaml
    /// </summary>
    public partial class WpfMessageBox : Window
    {
        public WpfMessageBox()
        {
            InitializeComponent();
        }

        static WpfMessageBox _messageBox;
        static MessageBoxResult _result = MessageBoxResult.No;

        public enum MessageBoxType
        {
            ConfirmationWithYesNo = 0,
            ConfirmationWithYesNoCancel,
            Information,
            Error,
            Warning
        }

        public enum MessageBoxImage
        {
            Warning = 48,
            Question=32,
            Information=64,
            Error=16,
            None=0
        }

        private void w1_Loaded_1(object sender, RoutedEventArgs e)
        {
            // try to apply effect to main window
            try
            {
                 
                MainWindow mainwindow_;
                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;
                        System.Windows.Media.Effects.BlurEffect eff = new System.Windows.Media.Effects.BlurEffect();
                        mainwindow_.Effect = eff;
                        break;
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void w1_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                MainWindow mainwindow_;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(MainWindow))
                    {
                        mainwindow_ = window as MainWindow;

                        mainwindow_.Effect = null;
                        break;
                    }
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        private void seticonblock(string txt)
        {
            txtblockicon.Text = txt;
        }

        private void setborderscolor(Brush color)
        {
            titleborder.Background = color;
            mainborder.BorderBrush = color;
        }

        private void SetImage(string imageName)
        {
            string uri = string.Format("/Resources/images/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);

            // img.Source = new BitmapImage(uriSource);
        }


        public static MessageBoxResult Show
    (string caption, string msg, MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.ConfirmationWithYesNo:
                    return Show(caption, msg, MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                case MessageBoxType.ConfirmationWithYesNoCancel:
                    return Show(caption, msg, MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                case MessageBoxType.Information:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Information);
                case MessageBoxType.Error:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                case MessageBoxType.Warning:
                    return Show(caption, msg, MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                default:
                    return MessageBoxResult.No;
            }
        }
        public static MessageBoxResult Show(string msg, MessageBoxType type)
        {
            return Show(string.Empty, msg, type);
        }
        public static MessageBoxResult Show(string msg)
        {
            return Show(string.Empty, msg,
            MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult Show
        (string caption, string text)
        {
            return Show(caption, text,
            MessageBoxButton.OK, MessageBoxImage.None);
        }
        public static MessageBoxResult Show
        (string caption, string text, MessageBoxButton button)
        {
            return Show(caption, text, button,
            MessageBoxImage.None);
        }
        public static MessageBoxResult Show
        (string caption, string text,
        MessageBoxButton button, MessageBoxImage image)
        {
            _messageBox = new WpfMessageBox { txtMsg = { Text = text }, MessageTitle = { Text = caption } };
            SetVisibilityOfButtons(button);
            SetImageOfMessageBox(image);
            SetSound(image);
            _messageBox.ShowDialog();
            return _result;
        }

        private static void SetSound(MessageBoxImage image)
        {
            SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.Notify);
            switch (image)
            {
                case MessageBoxImage.Warning:
                    // SystemSounds.Exclamation.Play();
                    simpleSound = new SoundPlayer(Properties.Resources.Exclamation);
                    try
                    { simpleSound.Play(); }
                    catch (Exception) { }
                    break;
                case MessageBoxImage.Question://#57617d
                                              // SystemSounds.Exclamation.Play();
                    simpleSound = new SoundPlayer(Properties.Resources.Exclamation);
                    try
                    { simpleSound.Play(); }
                    catch (Exception) { }
                    break;
                case MessageBoxImage.Information:
                    //  SystemSounds.Asterisk.Play();

                    simpleSound = new SoundPlayer(Properties.Resources.Notify);
                    try
                    { simpleSound.Play(); }
                    catch (Exception) { }

                    break;
                case MessageBoxImage.Error:
                    //  SystemSounds.Beep.Play();

                   // Uri uri = new Uri(@"pack://application:,,,/Resources/Error.wav");
                   // StreamResourceInfo sri = Application.GetResourceStream(uri);
                     simpleSound = new SoundPlayer(Properties.Resources.Error);
                    try
                    { simpleSound.Play(); }
                    catch (Exception) { }
                   
                    break;
                default:
                    // _messageBox.img.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private static void SetVisibilityOfButtons(MessageBoxButton button)
        {
            switch (button)
            {
                case MessageBoxButton.OK:
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    // _messageBox.btnOk.Focus();
                    break;
                case MessageBoxButton.OKCancel:
                    _messageBox.btnNo.Visibility = Visibility.Collapsed;
                    _messageBox.btnYes.Visibility = Visibility.Collapsed;
                    //  _messageBox.btnCancel.Focus();
                    break;
                case MessageBoxButton.YesNo:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;
                    _messageBox.btnCancel.Visibility = Visibility.Collapsed;
                    //   _messageBox.btnNo.Focus();
                    break;
                case MessageBoxButton.YesNoCancel:
                    _messageBox.btnOk.Visibility = Visibility.Collapsed;

                    //     _messageBox.btnCancel.Focus();
                    break;
                default:
                    break;
            }
        }
        private static void SetImageOfMessageBox(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Warning:
                    _messageBox.SetImage("Warning.png");//#ccad5e
                    _messageBox.setborderscolor((SolidColorBrush)(new BrushConverter().ConvertFrom("#fcd060")));
                    // _messageBox.seticonblock("\uf071");
                    break;
                case  MessageBoxImage.Question://#57617d
                    _messageBox.SetImage("Question.png");
                    _messageBox.setborderscolor((SolidColorBrush)(new BrushConverter().ConvertFrom("#2362ae")));
                    break;
                case MessageBoxImage.Information:
                    _messageBox.SetImage("Information.png");
                    _messageBox.setborderscolor((SolidColorBrush)(new BrushConverter().ConvertFrom("#2362ae")));
                    //  _messageBox.seticonblock("\uf05a");
                    break;
                case MessageBoxImage.Error:
                    _messageBox.SetImage("Error.png");
                    // _messageBox.setborderscolor(Brushes.Red);//FFdc000c
                    _messageBox.setborderscolor((SolidColorBrush)(new BrushConverter().ConvertFrom("#FFdc000c")));
                    //   _messageBox.seticonblock("\uf06a");
                    break;
                default:
                    // _messageBox.img.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None;
            _messageBox.Close();
            _messageBox = null;
        }

        private void titleborder_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void w1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                try
                {
                    if (btnOk != null)
                    {
                        if (btnOk.Visibility == Visibility.Visible)
                        {
                            _result = MessageBoxResult.OK;
                            _messageBox.Close();
                            _messageBox = null;
                        }
                    }
                }
                catch (Exception) { }
                e.Handled = true;
            }
        }
    }
}
