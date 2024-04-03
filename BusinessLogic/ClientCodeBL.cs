using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StersTransport.Models;
using StersTransport.GlobalData;
using StersTransport.DataAccess;
using System.Data;
using System.Text.RegularExpressions;

namespace StersTransport.BusinessLogic
{
    public class ClientCodeBL
    {
         public ClientCode clientCode { get; set; }

        public void Calculate_Weight()
        {
            double weight = clientCode.Weight_Kg.HasValue ? (double)clientCode.Weight_Kg : 0.0;
            double additionalweight = clientCode.AdditionalWeight.HasValue ? (double)clientCode.AdditionalWeight : 0.0;
            clientCode.Weight_Total = weight + additionalweight;
        }

        public void Calculate_Weight_By_Size()
        {
            double l = clientCode.Weight_L_cm.HasValue ? (double)clientCode.Weight_L_cm : 0.0;
            double w = clientCode.Weight_W_cm.HasValue ? (double)clientCode.Weight_W_cm : 0.0;
            double h = clientCode.Weight_H_cm.HasValue ? (double)clientCode.Weight_H_cm : 0.0;
            double f= clientCode.Weight_Vol_Factor.HasValue ? (double)clientCode.Weight_Vol_Factor : 0.0;

            if (f!=0)
            { 
                clientCode.Weight_BySizeValue = (l * w * h) / clientCode.Weight_Vol_Factor;
            }
           
        }
        public void Set_IsWeight_By_SizeChecked()
        {
            double l = clientCode.Weight_L_cm.HasValue ? (double)clientCode.Weight_L_cm : 0.0;
            double w = clientCode.Weight_W_cm.HasValue ? (double)clientCode.Weight_W_cm : 0.0;
            double h = clientCode.Weight_H_cm.HasValue ? (double)clientCode.Weight_H_cm : 0.0;
            if (l > 0 || w > 0 || h > 0)
            {
                clientCode.Weight_By_Size_Is_Checked = true;
            }
            else { clientCode.Weight_By_Size_Is_Checked = false; }
        }


        public void Calculate_Prices(bool UseCalculatedPackiging_cost)
        {

            double InsurancePercentage = clientCode.Insurance_Percentage.HasValue ? (double)clientCode.Insurance_Percentage : 0.0;
            double GoodsValue = clientCode.Goods_Value.HasValue ? (double)clientCode.Goods_Value : 0.0;
            double Weight = clientCode.Weight_Total.HasValue ? (double)clientCode.Weight_Total : 0.0;
            double MinimunPrice = clientCode.StartPrice_1_to_7KG.HasValue ? (double)clientCode.StartPrice_1_to_7KG : 0.0;
            double currencyRate = clientCode.Currency_Rate_1_IQD.HasValue ? (double) clientCode.Currency_Rate_1_IQD : 0.0;
            double BoxPackigingFactor = clientCode.BoxPackigingFactor.HasValue ? (double)clientCode.BoxPackigingFactor : 0;
           

            clientCode.Insurance_Amount = GoodsValue * (InsurancePercentage / 100.0);
            clientCode.Custom_Cost_IQD = clientCode.Custom_Cost_IQD.HasValue ? clientCode.Custom_Cost_IQD.Value : 0.0; // omitted from calculation
            clientCode.Admin_ExportDoc_Cost = clientCode.Admin_ExportDoc_Cost.HasValue ? clientCode.Admin_ExportDoc_Cost.Value : 0.0; // omitted from calculation
            clientCode.Custome_Cost_Qomrk = clientCode.Custome_Cost_Qomrk.HasValue ? clientCode.Custome_Cost_Qomrk.Value : 0.0;

            int BoxCount = 0;
            BoxCount = clientCode.Box_No.HasValue ? (int)clientCode.Box_No : 0;



            // ClientCode.Packiging_cost_IQD = BoxCount * StaticData.BoxPavkingCost_Equivalent;
            // double BoxPackigingFactor = clientCode.BoxPackigingFactor.HasValue ? (double)clientCode.BoxPackigingFactor : 0;


            // clientCode.Packiging_cost_IQD = clientCode.Packiging_cost_IQD.HasValue ? clientCode.Packiging_cost_IQD : 0.0;
            if (UseCalculatedPackiging_cost)
            { clientCode.Packiging_cost_IQD = BoxCount * BoxPackigingFactor; }
            else
            {
                clientCode.Packiging_cost_IQD = clientCode.Packiging_cost_IQD.HasValue ? (double)clientCode.Packiging_cost_IQD : 0.0;
            }
          

            bool HaveLocalPost = false;
            if (clientCode.Have_Local_Post.HasValue)
            {
                if (clientCode.Have_Local_Post.Value==true)
                { HaveLocalPost = true; }
            }

            decimal PriceDoorToDoorEach10KG = clientCode.PriceDoorToDoorEach10KG.HasValue ? (decimal)clientCode.PriceDoorToDoorEach10KG : 0;
            clientCode.POST_DoorToDoor_IQD = 0;
            if (HaveLocalPost)
            {
                clientCode.POST_DoorToDoor_IQD = Math.Ceiling(Weight / 10.0) * (double)PriceDoorToDoorEach10KG;
            }

            clientCode.Sub_Post_Cost_IQD = 0;
            clientCode.Price_KG_IQD = clientCode.Price_KG_IQD.HasValue ? (double)clientCode.Price_KG_IQD : 0.0;

            clientCode.Sub_Post_Cost_IQD = Weight * (double)clientCode.Price_KG_IQD;

            if (clientCode.Sub_Post_Cost_IQD < MinimunPrice)
            {
                clientCode.Sub_Post_Cost_IQD = MinimunPrice;
            }

            clientCode.Discount_Post_Cost_Send = clientCode.Discount_Post_Cost_Send.HasValue ? (double)clientCode.Discount_Post_Cost_Send : 0.0;

            clientCode.Total_Post_Cost_IQD = (double)clientCode.Insurance_Amount + (double)clientCode.Custome_Cost_Qomrk +
                 (double)clientCode.Packiging_cost_IQD + (double)clientCode.POST_DoorToDoor_IQD + (double)clientCode.Sub_Post_Cost_IQD
               - (double)clientCode.Discount_Post_Cost_Send;


            clientCode.TotalPaid_IQD = clientCode.TotalPaid_IQD.HasValue ? (double)clientCode.TotalPaid_IQD : 0.0;
            clientCode.Remaining_IQD = (double)clientCode.Total_Post_Cost_IQD - (double)clientCode.TotalPaid_IQD;

            // currency ....
            clientCode.Total_Paid_EUR = Math.Round((double)clientCode.TotalPaid_IQD / currencyRate, 2);
            clientCode.EuropaToPay = Math.Round((double)clientCode.Remaining_IQD / currencyRate, 2);





        }


        public string generate_new_code(Int64 W_branchID,out string yearprefix,out string branchprefix,out long num)
        {
            string newcode = string.Empty;
            AgentDa agentDa = new AgentDa();
            Agent agent= agentDa.GetAgent(W_branchID);
            yearprefix = agent.YearPrefix;
            branchprefix = agent.CharactersPrefix;
            int numberofdigits = (int)agent.NumberOfDigits;
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            long maxnumber= clientCodeDA.get_max_number(W_branchID,branchprefix, yearprefix);
            maxnumber += 1;
            num = maxnumber;
            string leadingzerostringformatparameter= string.Format("{0}{1}", "D", numberofdigits.ToString());
            string numberportion = maxnumber.ToString(leadingzerostringformatparameter);
            newcode = branchprefix + "-" + yearprefix + numberportion;
            return newcode;
        }


        public double get_last_shipmentNO(Int64 W_BranchID,string branchCode,string yearCode)
        {
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            double lastnum = 0;
            lastnum = clientCodeDA.get_last_shipmentNo(W_BranchID, branchCode, yearCode);
            return lastnum;
        }

        public double get_last_shipmentNOForSpecificYear(Int64 W_BranchID,int year)
        {
            double lastnum = 0;
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            lastnum = clientCodeDA.get_last_shipmentNoForSpecificYear(W_BranchID, year);
            return lastnum;
        }


        public bool ValidateZipCode(List<Country> countries, out string errormessage,out List<string> matchedCities)
        {
            matchedCities = new List<string>();
            errormessage = string.Empty;
            bool isvalid = false;
            // trim all white spaces first 
           
            clientCode.ZipCode= Regex.Replace(clientCode.ZipCode, @"\s", "");
            string zipcode = clientCode.ZipCode;
            Int64 countryid = clientCode.CountryPostId.HasValue ? (Int64)clientCode.CountryPostId : 0;

            // check country case ....

            var countryItem = countries.Where(c => c.Id == countryid).FirstOrDefault();
            if (countryItem == null)
            {
                errormessage = "Country Is Not Defined Or Selected...";
                return false;
            }

            // getting country data...

            int countryDigitscount = countryItem.Zip_Code_Digit_1.HasValue ? (int)countryItem.Zip_Code_Digit_1 : 0;
            int countrycharscount = countryItem.Zip_Code_TXT.HasValue? (int)countryItem.Zip_Code_TXT : 0;
            int ? specialcaseindex = countryItem.Special_Index;


            int user_entered_Digitscount = zipcode.Count(Char.IsDigit);
            int user_entered_charscount = zipcode.Length - user_entered_Digitscount; // any charachter that is not digit

            if (countryDigitscount == 0 && countrycharscount == 0) { return true; }

            if (user_entered_Digitscount != countryDigitscount || user_entered_charscount != countrycharscount)
            {
                errormessage = string.Format("You Must Enter ZipCode consist of {0} digit(s) and {1} charachter(s)", countryDigitscount.ToString(),
                    countrycharscount.ToString());
                return false;
            }

            if (specialcaseindex != null) // special country case 
            {
                
                //string specialcasetitle
                DataRow DR_SpecialINdex = GlobalData.StaticData.Special_Countries.Select("SC_Index='" +
                    (int)specialcaseindex
                    + "'").FirstOrDefault();
                if (DR_SpecialINdex == null)
                {
                    errormessage = "Country Is Not Defined Or Selected Or Have No Entries In Related Tables...";
                    return false;
                }

                // netherland country case 
                if (DR_SpecialINdex["SC_Title"].ToString() == StaticData.Netherland_Special_Country_Code)
                {
                    var matchedrows = StaticData.NL_ZipCode.Select("ZipCode='" + zipcode + "'");
                    var distinctmatchedcities = matchedrows.Select(x => x.Field<string>("Plaats")).Distinct().ToList();
                    if (distinctmatchedcities.Count > 0)
                    {
                        // set the matched cities
                        matchedCities = distinctmatchedcities;
                    }
                    else
                    {
                        errormessage = "No Data Found For The Entered ZipCode";
                        return false;
                    }
                }
                // germany country case 
                else if (DR_SpecialINdex["SC_Title"].ToString() == StaticData.Germany_Special_Country_Code)
                {
                    // get the cities matched by the entered zip code....
                   var matchedrows=  StaticData.DE_ZipCodes.Select("ZipCode='" + zipcode + "'");
                   var distinctmatchedcities =   matchedrows.Select(x => x.Field<string>("City_Name")).Distinct().ToList();
                    if (distinctmatchedcities.Count > 0)
                    {
                        // set the matched cities
                        matchedCities = distinctmatchedcities;
                    }
                    else
                    {
                        errormessage = "No Data Found For The Entered ZipCode";
                        return false;
                    }
                }
            }
            




            return isvalid;
        }


        public bool? Validate_Maximum_weight_eachBox(List<Country> countries)
        {
            // note : this methode should be called as final stage because it dependes on multiple controls(properties) values
            // best way is to call it when saving data....
            bool? isvalid = null;

            if (clientCode.Have_Local_Post.HasValue)
            {
                if ((bool)clientCode.Have_Local_Post)
                {
                    Int64 countryid = clientCode.CountryPostId.HasValue ? (Int64)clientCode.CountryPostId : 0;

                    // note : the following line could be done in data access layer and we could remove then the parameter :List<Country> countries
                    var countryItem = countries.Where(c => c.Id == countryid).FirstOrDefault();
                    if (countryItem == null)
                    { return null; }

                    bool checkMaximumWeighBox = countryItem.CheckMaximumWeighBox.HasValue ? (bool)countryItem.CheckMaximumWeighBox : false;
                    decimal maximumWeighBox = countryItem.MaximumWeighBox.HasValue ? (decimal)countryItem.MaximumWeighBox : 0;

                    if (checkMaximumWeighBox)
                    {
                        if (maximumWeighBox == 0)
                        { isvalid = false; }

                        double weight_Total = clientCode.Weight_Total.HasValue ? (double)clientCode.Weight_Total : 0.0;
                        double box_No = clientCode.Box_No.HasValue ? (double)clientCode.Box_No : 0.0;


                        //this assume that the boxes are equal in specifications and weight (not sure this is correct in real life...)
                        double weightforeachbox = weight_Total / box_No;
                        if (maximumWeighBox < (decimal)weightforeachbox)
                        {
                            isvalid = false;
                        }
                        else
                        {
                            isvalid = true;
                        }
                    }



                }
                else { return true; }
            }
            else { return true; }
            

            return isvalid;
        }


        public bool need_street_picker(List<Country> countries)
        {
            bool need = false;
            Int64 countryid = clientCode.CountryPostId.HasValue ? (Int64)clientCode.CountryPostId : 0;
            var countryItem = countries.Where(c => c.Id == countryid).FirstOrDefault();
            if (countryItem == null)
            {
                return false;
            }
            int? specialcaseindex = countryItem.Special_Index;
            if (specialcaseindex == null) // special country case 
            {
                return false;
            }
            DataRow DR_SpecialINdex = GlobalData.StaticData.Special_Countries.Select("SC_Index='" +
                (int)specialcaseindex
                + "'").FirstOrDefault();
            if (DR_SpecialINdex == null)
            {
                 
                return false;
            }
            if (DR_SpecialINdex["SC_Title"].ToString() == StaticData.Netherland_Special_Country_Code)
            {
                return true;
            }
            return need;
        }


        public void set_max_min_streetnumbers(out int maxno,out int minno)
        {
            minno = maxno = 0;
           var FilteredRow=  GlobalData.StaticData.NL_ZipCode.Select("Plaats='" + clientCode.CityPost + "' And ZipCode='" + clientCode.ZipCode + "'").FirstOrDefault();
            if (FilteredRow != null)
            {
                minno = Convert.ToInt32(FilteredRow["MinNummer"]);
                maxno = Convert.ToInt32(FilteredRow["MaxNummer"]);
            }
        }

        public string get_streetName()
        {
            string street = string.Empty;
            var FilteredRow = GlobalData.StaticData.NL_ZipCode.Select("Plaats='" + clientCode.CityPost + "' And ZipCode='" + clientCode.ZipCode + "'").FirstOrDefault();
            if (FilteredRow != null)
            {
                street = FilteredRow["Straat"].ToString();
            }
             return street;
        }


        // will check if the new order of shipment numbers is valid (no gaps betweens numbers....)
        public bool IS_new_shipmentNumbers_Order_Sequential(Agent branch,string branchCode,string yearCode, ClientCode excludedClientcode,double new_shipmentNUmber)
        {
            // get a list of shipment numbers from database for the branch excluding the client code parameter
            using (StersDB stersDB = new StersDB())
            {
                List<double?> all_branchShipmentNUmebrs = stersDB.ClientCode.Where( x => x.Code != excludedClientcode.Code&&x.BranchId== branch.Id&&
                x.BranchCode==branchCode&&x.YearCode==yearCode).Select(p => p.Shipment_No).ToList();
                // insert the new shipment number ...
                all_branchShipmentNUmebrs.Add(new_shipmentNUmber);

                // remove nulls
                all_branchShipmentNUmebrs.RemoveAll(x => x == null);

                // get distinct codes...
                List<double?> all_brnach_Distinct_ShipmentNUmebrs = all_branchShipmentNUmebrs.Distinct().ToList();
                //order....

                var orderdbranch_Distinct_ShipmentNUmebrs = all_brnach_Distinct_ShipmentNUmebrs.OrderBy(d => d);

                // check if there gaps
                bool sequential = orderdbranch_Distinct_ShipmentNUmebrs.Zip(orderdbranch_Distinct_ShipmentNUmebrs.Skip(1), (a, b) => (a + 1) == b).All(x => x);
                return sequential;
            }      
        }

        public bool IS_new_shipmentNumbers_Order_SequentialPerYear(Agent branch, ClientCode excludedClientcode, double new_shipmentNUmber)
        {
            using (StersDB stersDB = new StersDB())
            {
                // first get the original Year for The Code Being Updated from The Database
                ClientCodeDA clientCodeDA = new ClientCodeDA();
                var originalCode = clientCodeDA.GetClientCode(excludedClientcode.Code);
                int originalYear = 0;
                if (originalCode != null)
                {
                    originalYear = originalCode.PostYear.HasValue ? (int)originalCode.PostYear : 0;
                }
                int newyear = 0;
                newyear = excludedClientcode.PostDate.HasValue ? excludedClientcode.PostDate.Value.Year : 0; // notice that PostYear is computed column thus it has no value before storing to database so we use PostDate Property Instead

                if (newyear != originalYear)
                {
                    // for originalYear...

                    // exclude the code from the list and check sequence
                    // get a list of shipment numbers from database for the branch excluding the client code parameter
                    List<double?> all_branchShipmentNUmebrsOriginalYear = stersDB.ClientCode.Where(x => x.Code != excludedClientcode.Code && x.BranchId == branch.Id &&
                   x.PostYear == originalYear).Select(p => p.Shipment_No).ToList();
                    bool originalYearIsSequential=  check_numbers_sequential(all_branchShipmentNUmebrsOriginalYear);


                    // for new year  add the code to the list  and check sequence
                    List<double?> all_branchShipmentNUmebrsNewYear = stersDB.ClientCode.Where(x => x.Code != excludedClientcode.Code && x.BranchId == branch.Id &&
                   x.PostYear == newyear).Select(p => p.Shipment_No).ToList();
                    // insert the new shipment number ...
                    all_branchShipmentNUmebrsNewYear.Add(new_shipmentNUmber);

                    bool NewYearIsSequential = check_numbers_sequential(all_branchShipmentNUmebrsNewYear);

                    if (NewYearIsSequential && originalYearIsSequential)
                    { return true; }
                    else
                    { return false; }
                }
                else
                {
                    // exclude the code from the list and then add the new code shipment number to the list and check in one operation
                    List<double?> all_branchShipmentNUmebrsNewYear = stersDB.ClientCode.Where(x => x.Code != excludedClientcode.Code && x.BranchId == branch.Id &&
                    x.PostYear == newyear).Select(p => p.Shipment_No).ToList();
                    // insert the new shipment number ...
                    all_branchShipmentNUmebrsNewYear.Add(new_shipmentNUmber);
                    bool NewYearIsSequential = check_numbers_sequential(all_branchShipmentNUmebrsNewYear);
                    return NewYearIsSequential;
                }




                 
            }
        }




        public bool IS_new_shipmentNumbers_Order_SequentialPerYear_SetPropertiesNullMode(Agent branch, ClientCode excludedClientcode)
        {
            using (StersDB stersDB = new StersDB())
            {
                // first get the original Year for The Code Being Updated from The Database
                ClientCodeDA clientCodeDA = new ClientCodeDA();
                var originalCode = clientCodeDA.GetClientCode(excludedClientcode.Code);
                int originalYear = 0;
                if (originalCode != null)
                {
                    originalYear = originalCode.PostYear.HasValue ? (int)originalCode.PostYear : 0;
                }
                // exclude the code from the list and then add the new code shipment number to the list and check in one operation
                List<double?> all_branchShipmentNUmebrsNewYear = stersDB.ClientCode.Where(x => x.Code != excludedClientcode.Code && x.BranchId == branch.Id &&
                x.PostYear == originalYear).Select(p => p.Shipment_No).ToList();
                bool NewYearIsSequential = check_numbers_sequential(all_branchShipmentNUmebrsNewYear);
                return NewYearIsSequential;
            }
        }

        private bool check_numbers_sequential(List<double?> numbers)
        {
            bool sequential = false;
            // remove nulls
            numbers.RemoveAll(x => x == null);

            // get distinct codes...
            List<double?> all_distinctNUmbers= numbers.Distinct().ToList();


            //order....

            var orderedDistinctNumbers = all_distinctNUmbers.OrderBy(d => d);
            sequential = orderedDistinctNumbers.Zip(orderedDistinctNumbers.Skip(1), (a, b) => (a + 1) == b).All(x => x);

            return sequential;
        }


        public bool IS_new_shipmentNumbers_Order_Sequential_WithNewImported_ShipmentNUmbers(Agent branch, List<double?> new_shipmentNUmbers)
        {
            // get a list of shipment numbers from database for the branch excluding the client code parameter
            using (StersDB stersDB = new StersDB())
            {
                List<double?> all_branchShipmentNUmebrs = stersDB.ClientCode.Where(x => x.BranchId == branch.Id).Select(p => p.Shipment_No).ToList();



                // insert the new shipment numbers ...
                for (int c = 0; c < new_shipmentNUmbers.Count; c++)
                { all_branchShipmentNUmebrs.Add(new_shipmentNUmbers[c]); }
               

                // remove nulls
                all_branchShipmentNUmebrs.RemoveAll(x => x == null);

                // get distinct codes...
                List<double?> all_brnach_Distinct_ShipmentNUmebrs = all_branchShipmentNUmebrs.Distinct().ToList();
                //order....

                var orderdbranch_Distinct_ShipmentNUmebrs = all_brnach_Distinct_ShipmentNUmebrs.OrderBy(d => d);

                // check if there gaps
                bool sequential = orderdbranch_Distinct_ShipmentNUmebrs.Zip(orderdbranch_Distinct_ShipmentNUmebrs.Skip(1), (a, b) => (a + 1) == b).All(x => x);
                return sequential;



            }

        }


        public bool Is_ShipmentNUmber_Changed(string originalcode,double newShipmentNumber)

        {
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            ClientCode originalclientcode = clientCodeDA.GetClientCode(originalcode);
            if (originalclientcode.Shipment_No != newShipmentNumber)
            {
                return true;
            }
            else
            { return false; }
        }


        public bool Is_Year_Changed(string originalcode, int newYear)
        {
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            ClientCode originalclientcode = clientCodeDA.GetClientCode(originalcode);
            if (originalclientcode.PostYear != newYear)
            {
                return true;
            }
            else
            { return false; }

        }

        public bool Is_Agent_Disabled(long AgentId,string code)
        {
            bool IsDisabledResult = false;

            bool isAgentDisabledProperty = false;
            long original_AgentId = 0;
            AgentDa agentDa = new AgentDa();
            ClientCodeDA clientCodeDA = new ClientCodeDA();
            Agent agent= agentDa.GetAgent(AgentId);
            ClientCode clientCode = clientCodeDA.GetClientCode(code);


            if (clientCode != null)
            {
                original_AgentId= clientCode.AgentId.HasValue ? (long)clientCode.AgentId : 0;
            }

            if (agent != null)
            {
                isAgentDisabledProperty = agent.AgentIsDisabled.HasValue ? (bool)agent.AgentIsDisabled : false;
                if (isAgentDisabledProperty && agent.Id != original_AgentId)// new Agent ...
                {
                    IsDisabledResult = true;
                }
            }

            

            return IsDisabledResult;
        }

    }
}
