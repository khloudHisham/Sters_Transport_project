using StersTransport.ReportsModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Helpers
{
   public static class ShipmentNumbersHelper
    {
        public static ObservableCollection<ShipmentNumbers> formShipmentNumbers(ObservableCollection<double?> numbers)
        {
            ObservableCollection<ShipmentNumbers> output = new ObservableCollection<ShipmentNumbers>();
            output.Add(new ShipmentNumbers { Title = "ALL", Value = 0 });
            for (int c = 0; c < numbers.Count; c++)
            {
                if (numbers[c] != null)
                { output.Add(new ShipmentNumbers { Title = numbers[c].ToString(), Value = numbers[c] }); }
               
            }
            return output;
        }

    }
}
