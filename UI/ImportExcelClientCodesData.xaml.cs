using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
using StersTransport.Models;
using StersTransport.DataAccess;
using StersTransport.BusinessLogic;

namespace StersTransport.UI
{
    /// <summary>
    /// Interaction logic for ImportExcelClientCodesData.xaml
    /// </summary>
    public partial class ImportExcelClientCodesData : System.Windows.Window
    {



        List<Agent> branches = new List<Agent>();
        List<User> users = new List<User>();
        List<Country> countries = new List<Country>();
        List<Agent> agents = new List<Agent>();



        public ImportExcelClientCodesData()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // open excel file 
            // select flag box
            try
            {
                var ofdlg = new OpenFileDialog()
                {
                    Filter = "Excel Files|*.xls;*.xls;*.xlsx;*.xlsx;*.xlsm"
                };
                if (ofdlg.ShowDialog() == true)
                {
                    txtFilePath.Text = ofdlg.FileName;

                    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

                    Microsoft.Office.Interop.Excel.Workbook xlWorkBook = xlApp.Workbooks.Open(txtFilePath.Text, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                    System.Data.DataTable td = new System.Data.DataTable();
                    System.Data.DataTable dt = impoerexceltodatagrid(xlWorkSheet);
                    grd.ItemsSource = dt.DefaultView;
                    xlWorkBook.Close();
                    xlApp.Quit();
                    Marshal.ReleaseComObject(xlWorkSheet);
                    Marshal.ReleaseComObject(xlWorkBook);
                    Marshal.ReleaseComObject(xlApp);

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
           
        }

        private System.Data.DataTable impoerexceltodatagrid(Microsoft.Office.Interop.Excel.Worksheet ws)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string worksheetName = ws.Name;
            dt.TableName = worksheetName;
            Microsoft.Office.Interop.Excel.Range xlRange = ws.UsedRange;
            object[,] valueArray = (object[,])xlRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
            for (int k = 1; k <= valueArray.GetLength(1); k++)
            {
                dt.Columns.Add((string)valueArray[1, k]);  //add columns to the data table.
            }
            object[] singleDValue = new object[valueArray.GetLength(1)]; //value array first row contains column names. so loop starts from 2 instead of 1
            for (int i = 2; i <= valueArray.GetLength(0); i++)
            {
                for (int j = 0; j < valueArray.GetLength(1); j++)
                {
                    if (valueArray[i, j + 1] != null)
                    {
                        singleDValue[j] = valueArray[i, j + 1].ToString();
                    }
                    else
                    {
                        singleDValue[j] = valueArray[i, j + 1];
                    }
                }
                dt.LoadDataRow(singleDValue, System.Data.LoadOption.PreserveChanges);
            }

            return dt;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string currentCode = string.Empty;
            List<ClientCode> codes = new List<ClientCode>();
            bool errorbuildinglist = false;
            try
            {
                System.Data.DataTable dt = (grd.ItemsSource as System.Data.DataView).ToTable();
                if (dt.Rows.Count == 0) { return; }

                ClientCode clientcode = new ClientCode();

                for (int c = 0; c < dt.Rows.Count; c++)
                {

                    DataRow dr = dt.Rows[c];


                    clientcode = new ClientCode();

                    clientcode.IsImported = true;
                    clientcode.Code = dr["Client_Code"].ToString();
                    currentCode = clientcode.Code;
                    clientcode.BranchCode = dr["BranchCode"].ToString();
                    clientcode.YearCode = dr["YearCode"].ToString();
                    clientcode.Num = Convert.ToInt64( dr["Num"]);
                    clientcode.Shipment_No = Convert.ToDouble( dr["Shipment_No"].ToString());
                    clientcode.SenderName = dr["SenderName"].ToString();

                    if (!dr.IsNull("SenderCompany"))
                    { clientcode.SenderCompany = dr["SenderCompany"].ToString(); }

                    if (!dr.IsNull("Sender_ID"))
                    { clientcode.Sender_ID = dr["Sender_ID"].ToString(); }
                      

                    clientcode.Sender_Tel = dr["Sender_Tel"].ToString();

                    clientcode.ReceiverName = dr["ReceiverName"].ToString();

                    if (!dr.IsNull("ReceiverCompany"))
                    { clientcode.ReceiverCompany = dr["ReceiverCompany"].ToString(); }
                       


                    clientcode.Receiver_Tel = dr["Receiver_Tel"].ToString();

                    clientcode.Goods_Description = dr["Goods_Description"].ToString();

                    double? _Goods_Value = null;
                    if (!dr.IsNull("Goods_Value"))
                    {
                        try
                        { _Goods_Value = Convert.ToDouble(dr["Goods_Value"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Goods_Value = _Goods_Value;

                    clientcode.Have_Insurance = Convert.ToBoolean(dr["Have_Insurance"].ToString());

                    double? _Insurance_Percentage = null;
                    if (!dr.IsNull("Insurance_Percentage"))
                    {
                        try
                        { _Insurance_Percentage = Convert.ToDouble(dr["Insurance_Percentage"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Insurance_Percentage = _Insurance_Percentage;



                    double? _Insurance_Amount = null;
                    if (!dr.IsNull("Insurance_Amount"))
                    {
                        try
                        { _Insurance_Amount = Convert.ToDouble(dr["Insurance_Amount"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Insurance_Amount = _Insurance_Amount;




                    double? _Pallet_No = null;
                    if (!dr.IsNull("Pallet_No"))
                    {
                        try
                        { _Pallet_No = Convert.ToDouble(dr["Pallet_No"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Pallet_No = _Pallet_No;



                    double? _Box_No = null;
                    if (!dr.IsNull("Box_No"))
                    {
                        try
                        { _Box_No = Convert.ToDouble(dr["Box_No"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Box_No = _Box_No;



                    double? _Weight_Kg = null;
                    if (!dr.IsNull("Weight_Kg"))
                    {
                        try
                        { _Weight_Kg = Convert.ToDouble(dr["Weight_Kg"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_Kg = _Weight_Kg;




                    double? _Weight_Vol_Factor = null;
                    if (!dr.IsNull("Weight_Vol_Factor"))
                    {
                        try
                        { _Weight_Vol_Factor = Convert.ToDouble(dr["Weight_Vol_Factor"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_Vol_Factor = _Weight_Vol_Factor;



                    double? _Weight_Vol = null;
                    if (!dr.IsNull("Weight_Vol"))
                    {
                        try
                        { _Weight_Vol = Convert.ToDouble(dr["Weight_Vol"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.AdditionalWeight = _Weight_Vol; // column mapping is different here




                    double? _Weight_Total = null;
                    if (!dr.IsNull("Weight_Total"))
                    {
                        try
                        { _Weight_Total = Convert.ToDouble(dr["Weight_Total"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_Total = _Weight_Total;


                    double? _Custome_Cost_Qomrk = null;
                    if (!dr.IsNull("Custome_Cost_Qomrk"))
                    {
                        try
                        { _Custome_Cost_Qomrk = Convert.ToDouble(dr["Custome_Cost_Qomrk"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Custome_Cost_Qomrk = _Custome_Cost_Qomrk;


                    //[PostDate] 
                    clientcode.PostDate = Convert.ToDateTime(dr["PostDate"]);



                    double? _Packiging_cost_IQD = null;
                    if (!dr.IsNull("Packiging_cost_IQD"))
                    {
                        try
                        { _Packiging_cost_IQD = Convert.ToDouble(dr["Packiging_cost_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Packiging_cost_IQD = _Packiging_cost_IQD;



                    double? _BoxPackigingFactor = null;
                    if (!dr.IsNull("BoxPackigingFactor"))
                    {
                        try
                        { _BoxPackigingFactor = Convert.ToDouble(dr["BoxPackigingFactor"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.BoxPackigingFactor = _BoxPackigingFactor;




                    double? _POST_DoorToDoor_IQD = null;
                    if (!dr.IsNull("POST_DoorToDoor_IQD"))
                    {
                        try
                        { _POST_DoorToDoor_IQD = Convert.ToDouble(dr["POST_DoorToDoor_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.POST_DoorToDoor_IQD = _POST_DoorToDoor_IQD;



                    double? _Sub_Post_Cost_IQD = null;
                    if (!dr.IsNull("Sub_Post_Cost_IQD"))
                    {
                        try
                        { _Sub_Post_Cost_IQD = Convert.ToDouble(dr["Sub_Post_Cost_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Sub_Post_Cost_IQD = _Sub_Post_Cost_IQD;



                    double? _Discount_Post_Cost_Send = null;
                    if (!dr.IsNull("Discount_Post_Cost_Send"))
                    {
                        try
                        { _Discount_Post_Cost_Send = Convert.ToDouble(dr["Discount_Post_Cost_Send"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Discount_Post_Cost_Send = _Discount_Post_Cost_Send;



                    double? _Total_Post_Cost_IQD = null;
                    if (!dr.IsNull("Total_Post_Cost_IQD"))
                    {
                        try
                        { _Total_Post_Cost_IQD = Convert.ToDouble(dr["Total_Post_Cost_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Total_Post_Cost_IQD = _Total_Post_Cost_IQD;


       


                    double? _TotalPaid_IQD = null;
                    if (!dr.IsNull("TotalPaid_IQD"))
                    {
                        try
                        { _TotalPaid_IQD = Convert.ToDouble(dr["TotalPaid_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.TotalPaid_IQD = _TotalPaid_IQD;




                    double? _EuropaToPay = null;
                    if (!dr.IsNull("EuropaToPay"))
                    {
                        try
                        { _EuropaToPay = Convert.ToDouble(dr["EuropaToPay"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.EuropaToPay = _EuropaToPay;



                    double? _Currency_Rate_1_IQD = null;
                    if (!dr.IsNull("Currency_Rate_1_IQD"))
                    {
                        try
                        { _Currency_Rate_1_IQD = Convert.ToDouble(dr["Currency_Rate_1_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Currency_Rate_1_IQD = _Currency_Rate_1_IQD;


                    clientcode.Currency_Type = dr["Currency_Type"].ToString();



                    decimal? _PriceDoorToDoorEach10KG = null;
                    if (!dr.IsNull("PriceDoorToDoorEach10KG"))
                    {
                        try
                        { _PriceDoorToDoorEach10KG = Convert.ToDecimal(dr["PriceDoorToDoorEach10KG"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.PriceDoorToDoorEach10KG = _PriceDoorToDoorEach10KG;


                    double? _Price_KG_IQD = null;
                    if (!dr.IsNull("Price_KG_IQD"))
                    {
                        try
                        { _Price_KG_IQD = Convert.ToDouble(dr["Price_KG_IQD"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Price_KG_IQD = _Price_KG_IQD;


                    double? _StartPrice_1_to_7KG = null;
                    if (!dr.IsNull("StartPrice_1_to_7KG"))
                    {
                        try
                        { _StartPrice_1_to_7KG = Convert.ToDouble(dr["StartPrice_1_to_7KG"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.StartPrice_1_to_7KG = _StartPrice_1_to_7KG;



                    if (!dr.IsNull("Street_Name_No"))
                    { clientcode.Street_Name_No = dr["Street_Name_No"].ToString(); }


                    if (!dr.IsNull("Dep_Appar"))
                    { clientcode.Dep_Appar = dr["Dep_Appar"].ToString(); }

                    if (!dr.IsNull("ZipCode"))
                    { clientcode.ZipCode = dr["ZipCode"].ToString(); }


                    if (!dr.IsNull("CityPost"))
                    { clientcode.CityPost = dr["CityPost"].ToString(); }


                    if (!dr.IsNull("Note_Send"))
                    { clientcode.Note_Send = dr["Note_Send"].ToString(); }




                    clientcode.Have_Local_Post = Convert.ToBoolean(dr["Have_Local_Post"].ToString());



                    long? _Weight_L_cm = null;
                    if (!dr.IsNull("Weight_L_cm"))
                    {
                        try
                        { _Weight_L_cm = Convert.ToInt64(dr["Weight_L_cm"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_L_cm = _Weight_L_cm;


                    long? _Weight_W_cm = null;
                    if (!dr.IsNull("Weight_W_cm"))
                    {
                        try
                        { _Weight_W_cm = Convert.ToInt64(dr["Weight_W_cm"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_W_cm = _Weight_W_cm;



                    long? _Weight_H_cm = null;
                    if (!dr.IsNull("Weight_H_cm"))
                    {
                        try
                        { _Weight_H_cm = Convert.ToInt64(dr["Weight_H_cm"].ToString()); }
                        catch (Exception) { }
                    }
                    clientcode.Weight_H_cm = _Weight_H_cm;


                    // ids....
                    //BranchName

                   string BranchName = dr["BranchName"].ToString();
                    // get id 
                    var branch = branches.Where(x => x.AgentName == BranchName).FirstOrDefault();

                    if (branch != null)
                    {
                        clientcode.BranchId = branch.Id;
                    }

                    //UserName

                    string UserName = dr["UserName"].ToString();
                    // get id 
                    var user = users.Where(x => x.UserName == UserName).FirstOrDefault();

                    if (user != null)
                    {
                        clientcode.UserId = user.Id;
                    }

                    //CountryName

                    string CountryName = dr["CountryName"].ToString();
                    // get id 
                    var countryagent = countries.Where(x => x.CountryName == CountryName).FirstOrDefault();

                    if (countryagent != null)
                    {
                        clientcode.CountryAgentId = countryagent.Id;
                    }

                    //CountryPost
                    string CountryPost = dr["CountryPost"].ToString();
                    // get id 
                    var countrypost = countries.Where(x => x.CountryName == CountryPost).FirstOrDefault();

                    if (countrypost != null)
                    {
                        clientcode.CountryPostId = countrypost.Id;
                    }

                    //AgentName
                    string AgentName = dr["AgentName"].ToString();
                    // get id 
                    var agent = agents.Where(x => x.AgentName == AgentName).FirstOrDefault();

                    if (agent != null)
                    {
                        clientcode.AgentId = agent.Id;
                    }
                    //Person_IN_Charge
                    string Person_IN_Charge = dr["Person_IN_Charge"].ToString();
                    // get id 
                    var person = users.Where(x => x.UserName == Person_IN_Charge).FirstOrDefault();

                    if (person != null)
                    {
                        clientcode.Person_in_charge_Id = person.Id;
                    }



                    codes.Add(clientcode);

                }
            }
            catch (Exception ex)
            {
                errorbuildinglist = true;
                MessageBox.Show("Error Building List "+Environment.NewLine + ex.Message + Environment.NewLine + "Current Code:" + currentCode);
            }


            if (errorbuildinglist) { return; }

            if (codes.Count == 0) { return; }

            // no multi branch 
            var uniqueBranches = codes.Select(c => c.BranchId).Distinct().ToList();
            if (uniqueBranches.Count > 1)
            {
                WpfMessageBox.Show("", "Multi-Branch Import Is Not Supported",
                MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Warning);
                return;
            }
            // check new sequence of shipment numbers 
            BusinessLogic.ClientCodeBL clientCodeBL = new ClientCodeBL();

            long branchid = codes[0].BranchId.HasValue ? (long)codes[0].BranchId : 0;

            if (branchid == 0) { return; }
            AgentDa agentDa = new AgentDa();
            Agent Branch = agentDa.GetAgent(branchid);


            List<double?> newshipmentnumbers = codes.Select(x => x.Shipment_No).ToList();

            //    bool isnewshipmentnumbersSequential= clientCodeBL.IS_new_shipmentNumbers_Order_Sequential_WithNewImported_ShipmentNUmbers(Branch, newshipmentnumbers);

            bool isnewshipmentnumbersSequential = true; // Not Implemented.... skipped for now
            if (!isnewshipmentnumbersSequential)
            {
                WpfMessageBox.Show("", "New Shipment Numbers Are Not Sequential",
                MessageBoxButton.OK, WpfMessageBox.MessageBoxImage.Error);
                return;
            }

            /*
            MessageBoxResult dr1 = WpfMessageBox.Show("","this version of export data don't guarentee that the exported data contains sequential shipment numbers , please make a valid backup of database before proceede . do you want to proceede now ? ",
                MessageBoxButton.YesNo, WpfMessageBox.MessageBoxImage.Warning);

            if (dr1 == MessageBoxResult.No) { return; }
            */

            try
            {
                using (StersDB stersDB = new StersDB())
                {
                    for (int c = 0; c < codes.Count; c++)
                    {
                        ClientCode code = codes[c];
                        stersDB.ClientCode.Add(code);
                    }

                    stersDB.SaveChanges();
                    MessageBox.Show("Done");
                }
            }
            catch (Exception ex) 
            { MessageBox.Show(ex.Message); }


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                AgentDa agentDa = new AgentDa();
                BranchDa branchDa = new BranchDa();
                UserDa userDa = new UserDa();
                CountryDa countryDa = new CountryDa();

                agents = agentDa.GetAgents();
                branches = agentDa.GetAgents();
                users = userDa.GetUsers();
                countries = countryDa.GetCountries();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
