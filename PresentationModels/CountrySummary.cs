using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.PresentationModels
{
    public class CountrySummary
    {
        public long Id { get; set; }

        public string CountryName { get; set; }

        public double TotalCodes { get; set; }

        public double TotalBoxes { get; set; }

        public double TotalPallets { get; set; }

        public double Total_WeightReal { get; set; }

        public double Total_VolumneWeight { get; set; }

        public double TotalKG { get; set; }

        public double Total_Packiging_cost { get; set; }

        public double Total_Custom_Cost { get; set; }

        public double Total_Post_Door_to_Door { get; set; }

        public double Total_Sub_Post_Cost { get; set; }

        public double Total_Discount_Post_Cost { get; set; }

        public double TotalCashIn { get; set; }

        public decimal TotalComissions { get; set; }

        public double TotalPaidToCompany { get; set; }

        public double TotalPaidInEurope { get; set; }

        public string Currency { get; set; }

    }
}
