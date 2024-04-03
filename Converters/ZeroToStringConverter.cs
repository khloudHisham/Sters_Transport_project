using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StersTransport.Converters
{
   public class ZeroToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is decimal)
                {
                    decimal dvalue = (decimal)value;
                    if (dvalue == 0)
                    {
                        return "ALL-IN";
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is string)
                {
                    string strtvalue = value.ToString();
                    if (strtvalue == "ALL-IN")
                    {
                        return 0;
                    }
                    else
                    {
                        return value;
                    }
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
