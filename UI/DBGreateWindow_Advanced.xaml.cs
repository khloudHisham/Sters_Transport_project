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
    /// Interaction logic for DBGreateWindow_Advanced.xaml
    /// </summary>
    public partial class DBGreateWindow_Advanced : Window
    {

        DBGreateWindow dBGreateWindow;
        public DBGreateWindow_Advanced()
        {
            InitializeComponent();
        }

        public DBGreateWindow_Advanced(DBGreateWindow _dBGreateWindow)
        {
            InitializeComponent();
            dBGreateWindow = _dBGreateWindow;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txt.Text.Length > 0 && txt2.Text.Length > 0)
            {
                dBGreateWindow.scriptGeneratedPathmdf = txt.Text;
                dBGreateWindow.scriptGeneratedPathlog = txt2.Text;
                this.Close();
            }
        }
    }
}
