using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.GlobalData
{
    public class StaticData
    {
        public static int BoxPavkingCost_Equivalent = 3000;

        public static DataTable DE_ZipCodes;
        public static DataTable NL_ZipCode;
        public static DataTable Special_Countries;
        public static List<string> continents;

        public static List<Models.IdentityType> IdentityTypes;


        public static string Netherland_Special_Country_Code = "NTH";
        public static string Germany_Special_Country_Code = "GER";

        public static DataTable Labels;


        // continents
        public static string Continent_Asia = "Asia";
        public static string Continent_Europe = "Europe";
        public static string Continent_North_America = "North America";
        public static string Continent_South_America = "South America";
        public static string Continent_Africa = "Africa";
        public static string Continent_Australia = "Australia";


    }
}
