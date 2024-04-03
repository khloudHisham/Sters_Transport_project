using System.Windows;
using System.Windows.Controls;

namespace StersTransport.CustomeControls
{
    public class ComboboxEX : ComboBox
    {
        public static readonly DependencyProperty CBPlaceHolderProperty = DependencyProperty.Register("CBPlaceHolder", typeof(string),
          typeof(ComboboxEX), new PropertyMetadata(""));
        public string CBPlaceHolder
        {
            get { return (string)GetValue(CBPlaceHolderProperty); }
            set
            {
                SetValue(CBPlaceHolderProperty, value);
            }
        }

    }
}
