using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StersTransport.CustomeControls
{



  


    /// <summary>
    /// Text box with two additional properties (PlaceHolder And Isoptional)
    /// </summary>
    public class TextBoxEx :TextBox
    {


        // abetter approach is to do this via enumeration ... later...
      
        public bool IsOnlyNumeric { get; set; } = false;
        public bool IsOnlyDouble { get; set; } = false;
        public bool IsOnlyText { get; set; } = false;
      

        /*
         public bool IsOnlyNumeric { get; set; } 
        public bool IsOnlyDouble { get; set; } 
        public bool IsOnlyText { get; set; } 

        */
        public static readonly DependencyProperty PlaceHolderProperty = DependencyProperty.Register("PlaceHolder", typeof(string),
            typeof(TextBoxEx), new PropertyMetadata(""));
        public string PlaceHolder
        {
            get { return  (string)GetValue(PlaceHolderProperty); }
            set
            {
                SetValue(PlaceHolderProperty, value);
            }
        }

        public bool IsOptional
        {
            get
            {
                return (bool)GetValue(IsOptionalProperty);
            }
            set { SetValue(IsOptionalProperty, value); }
        }
        public static readonly DependencyProperty
            IsOptionalProperty =
            DependencyProperty.Register("IsOptional",
            typeof(bool), typeof(TextBoxEx),
            new PropertyMetadata(false));


     

    
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            // Note : e.Text Represent the currentlty pressed charachter...
            if (IsOnlyNumeric)
            {
                e.Handled = (!char.IsDigit(e.Text, e.Text.Length - 1));
            }
            else if (IsOnlyDouble)
            {
           
                var src = e.Source as TextBox;
            
                if (e.Text==".")
                {
                    int count = src.Text.Count(f => f == '.');
                    if (count == 1)
                    {
                        e.Handled = true;
                    }
                }
                else
                {
                       foreach (char ch in e.Text)
                        if (!(Char.IsDigit(ch)))
                        {
                            e.Handled = true;
                        }
                }
           
            }
            else if (IsOnlyText)
            {
                foreach (char ch in e.Text)
                    if (!(Char.IsLetter(ch)))
                {
                    e.Handled = true;

                }
            }
            base.OnPreviewTextInput(e);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (IsOnlyNumeric|| IsOnlyDouble)
            {
                var src = e.Source as TextBox;

                src.Dispatcher.BeginInvoke(new Action(() => src.SelectAll()));
            }
            base.OnGotFocus(e);
        }
     








    }
}
