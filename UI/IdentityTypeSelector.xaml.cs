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
using StersTransport.Models;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for IdentityTypeSelector.xaml
    /// </summary>
    public partial class IdentityTypeSelector : UserControl
    {

        private List<IdentityType> _identityTypes;
        public List<IdentityType> identityTypes
        {
            get { return _identityTypes; }
            set
            {
                _identityTypes = value;
                lstbxContainer.ItemsSource = _identityTypes;
            }
        }


        public event EventHandler Enter_Pressed;
        public event EventHandler ESC_Pressed;
        public event EventHandler MouseClicked;

        public ListBox ContainerListBox
        {
            get;
            set;
        }

        public string ContainerListBoxName
        {
            get
            {
                return lstbxContainer.Name;
            }
            set
            {

            }
        }


        public IdentityTypeSelector()
        {
            InitializeComponent();
            ContainerListBox = lstbxContainer;
        }

        private void lstbxContainer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (this.Enter_Pressed != null)
                {
                    Enter_Pressed(this, e);
                }
            }
            if (e.Key == Key.Escape)
            {
                if (this.ESC_Pressed != null)
                {
                    ESC_Pressed(this, e);
                }
            }
        }

        

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.MouseClicked != null)
            {
                MouseClicked(this, e);
            }

        }
    }
}
