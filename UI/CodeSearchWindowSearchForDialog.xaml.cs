using StersTransport.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for CodeSearchWindowSearchForDialog.xaml
    /// </summary>
    public partial class CodeSearchWindowSearchForDialog : Window, INotifyPropertyChanged
    {

        public enum Callingwindow
        {
            send,searchcode
        }

        Callingwindow callingwindow;
        #region INotifyPropertyChanged  implementation ..
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion
        private windowStatesenum _WindowStatesenum;
        public windowStatesenum WindowStatesenum
        {
            get { return _WindowStatesenum; }
            set
            {
                _WindowStatesenum = value;
                //    setActionButtonsEnabledStates();
            }
        }


        CodeSearchWindow codeSearchWindow;
        Send sendwindow;
        public CodeSearchWindowSearchForDialog()
        {
            InitializeComponent();
        }

        public CodeSearchWindowSearchForDialog(CodeSearchWindow _codesearchwindow)
        {
            InitializeComponent();
            callingwindow = Callingwindow.searchcode;
            codeSearchWindow = _codesearchwindow;
            codeSearchWindow.confirmed = false;
        }

        public CodeSearchWindowSearchForDialog(Send _codesearchwindow)
        {
            InitializeComponent();
            callingwindow = Callingwindow.send;
            sendwindow = _codesearchwindow;
             sendwindow.SearchCodeconfirmed = false;
        }



        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            { confirm(); }
        }

        private void confirm()
        {
            if (callingwindow == Callingwindow.searchcode)
            {
                codeSearchWindow.WindowStatesenum = WindowStatesenum;
                codeSearchWindow.confirmed = true;
                this.Close();
            }
            else if (callingwindow == Callingwindow.send)
            {
                sendwindow.windowsstate = WindowStatesenum;
                sendwindow.SearchCodeconfirmed = true;
                this.Close();
            }
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            rd_insert.IsChecked = true;
            confirm();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            // update
             rd_uodate.IsChecked = true;
            confirm();
        }

        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            // view
            rd_view.IsChecked = true;
            confirm();
        }
    }
}
