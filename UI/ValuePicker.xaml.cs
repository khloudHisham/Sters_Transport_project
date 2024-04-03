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
using System.Windows.Threading;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for ValuePicker.xaml
    /// </summary>
    public partial class ValuePicker : UserControl
    {

        public event EventHandler Enter_Pressed;
        public event EventHandler ESC_Pressed;
        public event EventHandler TextKeyPressed;



        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {

                _Title = value;
                lbl_title.Content = Title;
            }
        }

        private string _vla;
        public string Val
        {
            get { return _vla; }
            set
            {

                _vla = value;
                txt_value.Text = Val;
            }
        }



        public ValuePicker()
        {
            InitializeComponent();
            this.IsVisibleChanged += new DependencyPropertyChangedEventHandler
                             (ValuePicker_IsVisibleChanged);
        }

        private void ValuePicker_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    txt_value.Focus();
                }));
            }
        }

        public void set_value()
        {
            string d = string.Empty;
            try
            { d = txt_value.Text; }
            catch (Exception) { }


            Val = d;
        }

        private void txt_value_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.TextKeyPressed != null)
            {
                TextKeyPressed(this, e);
            }
        }

        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
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
    }
}
