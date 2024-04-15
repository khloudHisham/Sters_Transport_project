namespace StersTransport.Models
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Text.RegularExpressions;
    using StersTransport.GlobalData;



    // Note: This Class Is Names Under Models 
    // But Its Actually Behave Like View Model Cause It Contains Validations And Setting Some Proerties Based On other Properties 
    // This is Intended...
    // when we need a base model for entity we can create on ....

    [Table("CODE_LIST")]
    public partial class ClientCode : INotifyPropertyChanged, INotifyDataErrorInfo
    {


        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region INotifyDataErrorInfo Implementation
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
        public bool HasErrors
        {
            get
            {
                // Indicate whether the entire Product object is error-free.
                return (errors.Count > 0);
            }
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            // Clear any errors that already exist for this property.
            errors.Remove(propertyName);
            // Add the list collection for the specified property.
            errors.Add(propertyName, propertyErrors);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        private void ClearErrors(string propertyName)
        {
            // Remove the error list for this property.
            errors.Remove(propertyName);
            // Raise the error-notification event.
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                // Provide all the error collections.
                return (errors.Values);
            }
            else
            {
                // Provice the error collection for the requested property
                // (if it has errors).
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion



        public ClientCode()
        {
            Have_Insurance = false;
            Have_Local_Post = false;
            Weight_By_Size_Is_Checked = false;
            MustValidateNotesLength = false;
            MustValidateStreetNameForDigitsAndChars = false;
        }

        [NotMapped]
        public bool isvalidating { get; set; }

        [NotMapped]
        public bool shipmentNoIsvalidating { get; set; }

        [NotMapped]
        public bool zipcodeisvalidating { get; set; }

        [NotMapped]
        public string zipcodeErrorMessage { get; set; }

        [NotMapped]
        public bool MustValidateNotesLength { get; set; }

        [NotMapped]
        public bool MustValidateStreetNameForDigitsAndChars { get; set; }

        private int _AutoNumber;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoNumber
        {
            get { return _AutoNumber; }
            set
            {
                _AutoNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AutoNumber"));
            }
        }

        private string _Code;
        [Key]
        [StringLength(255)]
        [Column("Client_Code")]
        public string Code
        {
            get { return _Code; }
            set { 
                _Code = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Code"));
            }
        }

        private string _BranchCode;

        [StringLength(50)]
        public string BranchCode
        {
            get { return _BranchCode; }
            set
            {
                _BranchCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BranchCode"));
            }
        }

        private string _YearCode;
        [StringLength(50)]
        public string YearCode
        {
            get { return _YearCode; }
            set
            {
                _YearCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YearCode"));
            }
        }

        private double? _Shipment_No;
        public double? Shipment_No 
        {
            get { return _Shipment_No; }
            set 
            { _Shipment_No = value;

                if (shipmentNoIsvalidating)
                {
                  
                    if (_Shipment_No.HasValue)
                    {
                        if (_Shipment_No > BranchLastShipmentNO + 1)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueExceedeMaxAllowed);
                            SetErrors("Shipment_No", errors);
                        }
                        else if (_Shipment_No <= 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueLessMinAllowed);
                            SetErrors("Shipment_No", errors);
                        }
                        else
                        {

                            ClearErrors("Shipment_No");
                        }
                    }
                    else
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Shipment_No", errors);
                    }
                    // must not exceede the last shipment number (this must be revalidate when inserting and update and fetch the real data from database)


                }
                if (isvalidating)
                {
                    
                }
              
                OnPropertyChanged(new PropertyChangedEventArgs("Shipment_No"));
            }
        }



        [NotMapped]
        public double BranchLastShipmentNO
        { get; set; }

        private string _SenderName;
        [StringLength(255)]
        public string SenderName 
        {
            get { return _SenderName; }
            set
            {
                _SenderName = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_SenderName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("SenderName", errors);
                    }
                    else
                    {
                        ClearErrors("SenderName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("SenderName"));
            }
        }


        private string _SenderCompany;
        [StringLength(255)]
        public string SenderCompany
        {
            get { return _SenderCompany; }
            set
            {
                _SenderCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("SenderCompany"));
            }
        }

        private string _Sender_ID;
        [StringLength(255)]
        public string Sender_ID
        {
            get { return _Sender_ID; }
            set
            {
                _Sender_ID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sender_ID"));
            }
        }

        private string _Sender_ID_Type;

        public string Sender_ID_Type {
            get { return _Sender_ID_Type; }
            set
            {
                _Sender_ID_Type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sender_ID_Type"));
            }
        }

        private string _Sender_ID_Type_Shortcut;

        [NotMapped]
        public string Sender_ID_Type_Shortcut
        {
            get { return _Sender_ID_Type_Shortcut; }
            set
            {
                _Sender_ID_Type_Shortcut = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sender_ID_Type_Shortcut"));
            }
        }


        private string _Sender_Tel;
        [StringLength(255)]
        public string Sender_Tel {
            get { return _Sender_Tel; }
            set
            {
                _Sender_Tel = value;
                if (isvalidating)
                {
                    List<string> errors = new List<string>();
                    if (string.IsNullOrEmpty(_Sender_Tel))
                    {
                        errors.Add(CommonMessages.RequiredValue);

                        SetErrors("Sender_Tel", errors);
                    }
                    else if (_Sender_Tel.Length < 11) // this must be variable and related to each country rules.....
                    {
                        errors.Add("This Feild Must Have At Lease 11 Charachters");
                        SetErrors("Sender_Tel", errors);
                    }
                    else
                    {
                        ClearErrors("Sender_Tel");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Sender_Tel"));
            }
        }


        private string _ReceiverName;
        [StringLength(255)]
        public string ReceiverName
        {
            get { return _ReceiverName; }
            set
            {
                _ReceiverName = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_ReceiverName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("ReceiverName", errors);
                    }
                    else
                    {
                        ClearErrors("ReceiverName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ReceiverName"));
            }
        }


        private string _ReceiverCompany;
        [StringLength(255)]
        public string ReceiverCompany
        {
            get { return _ReceiverCompany; }
            set
            {
                _ReceiverCompany = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ReceiverCompany"));
            }
        }


       
        private string _Receiver_Tel;
        [StringLength(255)]
        public string Receiver_Tel
        {
            get { return _Receiver_Tel; }
            set
            {
                _Receiver_Tel = value;
                if (isvalidating)
                {
                    List<string> errors = new List<string>();
                    if (string.IsNullOrEmpty(_Receiver_Tel))
                    {
                        errors.Add(CommonMessages.RequiredValue);

                        SetErrors("Receiver_Tel", errors);
                    }
                    
                    else
                    {
                        ClearErrors("Receiver_Tel");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Receiver_Tel"));
            }
        }



        private string _Goods_Description;
        [StringLength(255)]
        public string Goods_Description {
            get { return _Goods_Description; }
            set
            {
                _Goods_Description = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_Goods_Description))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Goods_Description", errors);
                    }
                    else
                    {
                        ClearErrors("Goods_Description");
                    }
                }


                OnPropertyChanged(new PropertyChangedEventArgs("Goods_Description"));
            }
        }


        private double? _Goods_Value;
        public double? Goods_Value
        {
            get { return _Goods_Value; }
            set
            {
                _Goods_Value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Goods_Value"));
            }
        }


        // not used....
        [StringLength(255)]
        public string Insurance_Yes_No { get; set; }




        private bool? _Have_Insurance;

        public bool? Have_Insurance
        {
            get { return _Have_Insurance; }
            set
            {
                _Have_Insurance = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Have_Insurance"));
            }
        }



        private double? _Insurance_Percentage;
        public double? Insurance_Percentage
        {
            get { return _Insurance_Percentage; }
            set
            {
                _Insurance_Percentage = value;
                if (_Insurance_Percentage.HasValue)
                {
                    if (_Insurance_Percentage < 0) { _Insurance_Percentage = 0; }
                    if (_Insurance_Percentage > 100) { _Insurance_Percentage = 100; }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Insurance_Percentage"));
            }
        }



        private double? _Insurance_Amount;
        public double? Insurance_Amount
        {
            get { return _Insurance_Amount; }
            set
            {
                _Insurance_Amount = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Insurance_Amount"));
            }
        }

        private double? _Pallet_No;

        public double? Pallet_No {
            get { return _Pallet_No; }
            set
            {
                _Pallet_No = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Pallet_No"));
            }
        }


        private double? _Box_No;

        public double? Box_No
        {
            get { return _Box_No; }
            set
            {
                _Box_No = value;
                 if (isvalidating)
                {
                    if (!_Box_No.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Box_No", errors);
                    }
                    else if (_Box_No == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("Box_No", errors);
                    }
                    else
                    {
                        ClearErrors("Box_No");
                    }
                }
             
                OnPropertyChanged(new PropertyChangedEventArgs("Box_No"));
            }
        }


        private double? _Weight_Kg;

        public double? Weight_Kg {
            get { return _Weight_Kg; }
            set
            {
                _Weight_Kg = value;


                if (isvalidating)
                {
                    if (!_Weight_Kg.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Weight_Kg", errors);
                    }
                    else if (_Weight_Kg == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("Weight_Kg", errors);
                    }
                    else
                    {
                        ClearErrors("Weight_Kg");
                    }
                }
                
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_Kg"));

                
                 
            }
        }


       /*
        [StringLength(255)]
        public string Weight_L_W_H_cm { get; set; }
        */

        private double? _Weight_Vol_Factor;
        public double? Weight_Vol_Factor {
            get { return _Weight_Vol_Factor; }
            set
            {
                _Weight_Vol_Factor = value;
                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        if (_Weight_Vol_Factor.HasValue == false)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("Weight_Vol_Factor", errors);
                        }
                        else
                        { ClearErrors("Weight_Vol_Factor"); }
                    }
                    else
                    { ClearErrors("Weight_Vol_Factor"); }
                   
                }
               
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_Vol_Factor"));
            }
        }


      
        private double? _AdditionalWeight;
        [Column("Weight_Vol")]
        public double? AdditionalWeight
        {
            get { return _AdditionalWeight; }
            set
            {
                _AdditionalWeight = value;
                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        if (_AdditionalWeight.HasValue == false) // must be greater than zero too 
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("AdditionalWeight", errors);
                        }
                        else if (_AdditionalWeight == 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("AdditionalWeight", errors);
                        }
                         
                        else
                        {
                            // additional condition: 
                            //  suppose : volumneWeight-realWeight=diff
                            // now if additionalWeight<diff-2 : error
                            if (GlobalData.CompanyData.weight_difference != null)
                            {


                                double weight_difference = (double)GlobalData.CompanyData.weight_difference;

                                double add_w = _AdditionalWeight.HasValue ? (double)_AdditionalWeight : 0;
                                double real_w = _Weight_Kg.HasValue ? (double)_Weight_Kg : 0;
                                double vol_w = _Weight_BySizeValue.HasValue ? (double)_Weight_BySizeValue : 0;

                                if (add_w + real_w < vol_w - weight_difference)
                                {
                                    List<string> errors = new List<string>();
                                    errors.Add(string.Format("{0} {1}", "The Minimun Value Allowed For Additional Weight Is :", (vol_w - real_w - weight_difference).ToString()));
                                    SetErrors("AdditionalWeight", errors);
                                }
                                else
                                {
                                    ClearErrors("AdditionalWeight");
                                }
                            }
                            else
                            { ClearErrors("AdditionalWeight"); }

                        }
                    }
                    else
                    { ClearErrors("AdditionalWeight"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("AdditionalWeight"));
            }
        }


        private double? _Weight_Total;
        public double? Weight_Total
        {
            get { return _Weight_Total; }
            set
            {
                _Weight_Total = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_Total"));
            }
        }


        private double? _Admin_ExportDoc_Cost;
        public double? Admin_ExportDoc_Cost
        {
            get { return _Admin_ExportDoc_Cost; }
            set
            {
                _Admin_ExportDoc_Cost = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Admin_ExportDoc_Cost"));
            }
        }



        private double? _Custome_Cost_Qomrk;
        public double? Custome_Cost_Qomrk
        {
            get { return _Custome_Cost_Qomrk; }
            set
            {
                _Custome_Cost_Qomrk = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Custome_Cost_Qomrk"));
            }
        }



        /*
        [StringLength(255)]
        public string DatePost { get; set; }

        [StringLength(255)]
        public string TimePost { get; set; }
        */

        private DateTime? _PostDate;
        [Column(TypeName = "smalldatetime")]
        public DateTime? PostDate
        {
            get { return _PostDate; }
            set
            {
                _PostDate = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PostDate"));
            }
        }


        // added...
        private int? _PostYear;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? PostYear
        {
            get { return _PostYear; }
            set { _PostYear = value; }
        }






        private double? _Packiging_cost_IQD;
        public double? Packiging_cost_IQD
        {
            get { return _Packiging_cost_IQD; }
            set
            {
                _Packiging_cost_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Packiging_cost_IQD"));
            }
        }


        private double? _Custom_Cost_IQD;
        public double? Custom_Cost_IQD
        {
            get { return _Custom_Cost_IQD; }
            set
            {
                _Custom_Cost_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Custom_Cost_IQD"));
            }
        }


        private double? _POST_DoorToDoor_IQD;
        public double? POST_DoorToDoor_IQD
        {
            get { return _POST_DoorToDoor_IQD; }
            set
            {
                _POST_DoorToDoor_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("POST_DoorToDoor_IQD"));
            }
        }

        private double? _Sub_Post_Cost_IQD;
        public double? Sub_Post_Cost_IQD
        {
            get { return _Sub_Post_Cost_IQD; }
            set
            {
                _Sub_Post_Cost_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Sub_Post_Cost_IQD"));
            }
        }


        private double? _Discount_Post_Cost_Send;
        public double? Discount_Post_Cost_Send
        {
            get { return _Discount_Post_Cost_Send; }
            set
            {
                _Discount_Post_Cost_Send = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Discount_Post_Cost_Send"));
            }
        }


        private double? _Total_Post_Cost_IQD;
        public double? Total_Post_Cost_IQD
        {
            get { return _Total_Post_Cost_IQD; }
            set
            {
                _Total_Post_Cost_IQD = value;
                IsAllPaid = _TotalPaid_IQD == _Total_Post_Cost_IQD;
                OnPropertyChanged(new PropertyChangedEventArgs("Total_Post_Cost_IQD"));
                set_remaining_iraq_dinar();
                set_total_psot_Cost_eur();

                if (isvalidating)
                {
                    if (!_Total_Post_Cost_IQD.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Total_Post_Cost_IQD", errors);
                    }
                    else
                    {
                        if ((double)_Total_Post_Cost_IQD <= 0) // by setting discount a greater value this can be minuus zero
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("Total_Post_Cost_IQD", errors);
                        }
                        else
                        {
                            ClearErrors("Total_Post_Cost_IQD");
                        }
                    }
                } // if is validating...

            }
        }

        private double? _TotalPaid_IQD;
        public double? TotalPaid_IQD
        {
            get { return _TotalPaid_IQD; }
            set
            {
                _TotalPaid_IQD = value;

                IsAllPaid = _TotalPaid_IQD == _Total_Post_Cost_IQD;


                OnPropertyChanged(new PropertyChangedEventArgs("TotalPaid_IQD"));
                set_remaining_iraq_dinar();
                set_total_paid_eur();
            }
        }

        private void set_total_psot_Cost_eur()
        {

            if (Total_Post_Cost_IQD.HasValue && Currency_Rate_1_IQD.HasValue && Currency_Rate_1_IQD != 0)
            {
                Total_Post_Cost_EUR = Math.Round((double)_Total_Post_Cost_IQD / (double)_Currency_Rate_1_IQD, 2);
            }
            else
            {
                Total_Post_Cost_EUR = 0;
            }
          
        }
        private void set_total_paid_eur()
        {
            if (TotalPaid_IQD.HasValue && Currency_Rate_1_IQD.HasValue && Currency_Rate_1_IQD != 0)
            {
                Total_Paid_EUR = Math.Round((double)_TotalPaid_IQD / (double)_Currency_Rate_1_IQD, 2);
            }
            else
            { Total_Paid_EUR = 0; }
        }

        private void set_remaining_iraq_dinar()
        {
            Remaining_IQD = _Total_Post_Cost_IQD - _TotalPaid_IQD;
        }


        private double? _Remaining_IQD;
        [NotMapped]
        public double? Remaining_IQD
        {
            get { return _Remaining_IQD; }
            set
            {
                _Remaining_IQD = value;



                if (isvalidating)
                {
                    if (!_Remaining_IQD.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Remaining_IQD", errors);
                    }
                    else
                    {
                        if ((double)_Remaining_IQD < 0) // by setting paid amount a greater value this can be minuus zero
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("Remaining_IQD", errors);
                        }
                        else
                        {
                            ClearErrors("Remaining_IQD");
                        }
                    }
                } // if is validating...


                OnPropertyChanged(new PropertyChangedEventArgs("Remaining_IQD"));
            }

        }


        private bool _IsAllPaid;
         [NotMapped]
        public bool IsAllPaid
        {
            get { return _IsAllPaid; }
            set
            {
                _IsAllPaid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsAllPaid"));
            }
        }


        
        private double? _Total_Post_Cost_EUR;
        [NotMapped]
        public double? Total_Post_Cost_EUR
        {
            get { return _Total_Post_Cost_EUR; }
            set
            {
                _Total_Post_Cost_EUR = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Total_Post_Cost_EUR"));
            }
        }

        private double? _Total_Paid_EUR;
        [NotMapped]
        public double? Total_Paid_EUR
        {
            get { return _Total_Paid_EUR; }
            set
            {
                _Total_Paid_EUR = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Total_Paid_EUR"));
            }
        }



        private double? _EuropaToPay;
        public double? EuropaToPay
        {
            get { return _EuropaToPay; }
            set
            {
                _EuropaToPay = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EuropaToPay"));
            }
        }


        private double? _Currency_Rate_1_IQD;
        public double? Currency_Rate_1_IQD
        {
            get { return _Currency_Rate_1_IQD; }
            set
            {
                _Currency_Rate_1_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currency_Rate_1_IQD"));
                set_total_psot_Cost_eur();
                set_total_paid_eur();
            }
        }


        private string _Currency_Type;
        [StringLength(255)]
        public string Currency_Type
        {
            get { return _Currency_Type; }
            set
            {
                _Currency_Type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currency_Type"));
            }
        }


        /*
        [StringLength(255)]
        public string PriceDoorToDoorEach10KG_IQD
        {
            get;set;
        }
        */


        private decimal? _PriceDoorToDoorEach10KG;
        public decimal? PriceDoorToDoorEach10KG
        {
            get { return _PriceDoorToDoorEach10KG; }
            set
            {
                _PriceDoorToDoorEach10KG = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PriceDoorToDoorEach10KG"));
            }
        }



        private double? _Price_KG_IQD;
        public double? Price_KG_IQD
        {
            get { return _Price_KG_IQD; }
            set
            {
                _Price_KG_IQD = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Price_KG_IQD"));
            }
        }


        private double? _StartPrice_1_to_7KG;
        public double? StartPrice_1_to_7KG
        {
            get { return _StartPrice_1_to_7KG; }
            set
            {
                _StartPrice_1_to_7KG = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartPrice_1_to_7KG"));
            }
        }


        private double? _BoxPackigingFactor;
        public double? BoxPackigingFactor
        {
            get { return _BoxPackigingFactor; }
            set
            {
                _BoxPackigingFactor = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BoxPackigingFactor"));
            }
        }



        /*
        [StringLength(255)]
        public string CountryAgent
        {
            get;set;
        }
        */

        private string _Agent;
        [StringLength(255)]
        public string Agent
        {
            get { return _Agent; }
            set
            {
                _Agent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Agent"));
            }
        }


        private string _Street_Name_No;
        [StringLength(255)]
        public string Street_Name_No
        {
            get { return _Street_Name_No; }
            set
            {

                _Street_Name_No = value;
                if (isvalidating)
                {
                    if (_Have_Local_Post.HasValue)
                    {

                        if ((bool)_Have_Local_Post)
                        {
                            if (string.IsNullOrEmpty( _Street_Name_No))
                            {
                                List<string> errors = new List<string>();
                                errors.Add(CommonMessages.RequiredValue);
                                SetErrors("Street_Name_No", errors);
                            }
                            else
                            {
                                if (MustValidateStreetNameForDigitsAndChars)
                                {
                                    bool containletters = _Street_Name_No.Any(x => !char.IsLetter(x));
                                    bool containsdigits = _Street_Name_No.Any(x => !char.IsDigit(x));

                                    if (!containletters || !containsdigits)
                                    {
                                        List<string> errors = new List<string>();
                                        errors.Add("Street Name Must Contains Letters And Digits");
                                        SetErrors("Street_Name_No", errors);
                                    }
                                    else
                                    {
                                        ClearErrors("Street_Name_No");
                                    }

                                }
                                else
                                { 
                                    ClearErrors("Street_Name_No");
                                }
                               
                            }
                        }
                        else
                        { ClearErrors("Street_Name_No"); }

                        
                    }
                    else { ClearErrors("Street_Name_No"); }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Street_Name_No"));
            }
        }

        private string _Dep_Appar;
        [StringLength(255)]
        public string Dep_Appar
        {
            get { return _Dep_Appar; }
            set
            {
                _Dep_Appar = value;

                OnPropertyChanged(new PropertyChangedEventArgs("Dep_Appar"));
            }
        }

        private string _ZipCode;
        [StringLength(255)]
        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                _ZipCode = value;
                if (isvalidating)
                {
                    if (_Have_Local_Post.HasValue)
                    {

                        if ((bool)_Have_Local_Post)
                        {
                            if (string.IsNullOrEmpty(_ZipCode))
                            {
                                List<string> errors = new List<string>();
                                errors.Add(CommonMessages.RequiredValue);
                                SetErrors("ZipCode", errors);
                            }
                            else
                            {
                                ClearErrors("ZipCode");
                            }
                        }
                        else
                        { ClearErrors("ZipCode"); }


                    }
                    else { ClearErrors("ZipCode"); }
                }
                if (zipcodeisvalidating)
                {
                    if (_Have_Local_Post.HasValue)
                    {
                        if ((bool)_Have_Local_Post)
                        {
                            if (!string.IsNullOrEmpty(zipcodeErrorMessage))
                            {
                                List<string> errors = new List<string>();
                                errors.Add(zipcodeErrorMessage);
                                SetErrors("ZipCode", errors);
                            }
                            else
                            {
                                ClearErrors("ZipCode");
                            }
                        }
                        else
                        { ClearErrors("ZipCode"); }
                    }
                    else { ClearErrors("ZipCode"); }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ZipCode"));
            }
        }
   

        private string _CityPost;
        [StringLength(255)]
        public string CityPost
        {
            get { return _CityPost; }
            set
            {
                _CityPost = value;
                if (isvalidating)
                {
                    if (_Have_Local_Post.HasValue)
                    {

                        if ((bool)_Have_Local_Post)
                        {
                            if (string.IsNullOrEmpty(_CityPost))
                            {
                                List<string> errors = new List<string>();
                                errors.Add(CommonMessages.RequiredValue);
                                SetErrors("CityPost", errors);
                            }
                            else
                            {
                                ClearErrors("CityPost");
                            }
                        }
                        else
                        { ClearErrors("CityPost"); }


                    }
                    else { ClearErrors("CityPost"); }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("CityPost"));
            }
        }

        /*
        [StringLength(255)]
        public string CountryPost
        {
            get;set;
        }
        */


        private string _Note_Send;
        [StringLength(255)]
        public string Note_Send
        {
            get { return _Note_Send; }
            set
            {

                _Note_Send = value;

                if (isvalidating)
                {
                    if (MustValidateNotesLength)
                    {
                        bool L_havelocalpost = Have_Local_Post.HasValue ? (bool)Have_Local_Post : false;
                        if (!L_havelocalpost)
                        {
                            if (string.IsNullOrEmpty(_Note_Send))
                            {
                                List<string> errors = new List<string>();
                                errors.Add(CommonMessages.RequiredValue);
                                SetErrors("Note_Send", errors);
                            }
                            else
                            {
                                if (_Note_Send.Length < 3)
                                {
                                    List<string> errors = new List<string>();
                                    errors.Add("This Feild Must Have At Least 3 Charachters..");
                                    SetErrors("Note_Send", errors);
                                }
                                else
                                {
                                    ClearErrors("Note_Send");
                                }
                            }
                        }
                        else
                        {
                            ClearErrors("Note_Send");
                        }
                    }
                    else
                    { ClearErrors("Note_Send"); }
                   
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Note_Send"));
            }
        }

        private string _Person_in_charge_Send;
        [StringLength(255)]
        public string Person_in_charge_Send
        {
            get { return _Person_in_charge_Send; }
            set
            {
                _Person_in_charge_Send = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Person_in_charge_Send"));
            }
        }



        private string _Agent_Name_KU;
        [StringLength(255)]
        public string Agent_Name_KU
        {
            get { return _Agent_Name_KU; }
            set
            {
                _Agent_Name_KU = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Agent_Name_KU"));
            }
        }

        /*
        [StringLength(255)]
        public string Local_Post_YES_NO
        {
            get;set;
        }
        */


        private bool? _Have_Local_Post;
        public bool? Have_Local_Post
        {
            get { return _Have_Local_Post; }
            set
            {
                _Have_Local_Post = value;
                if (_Have_Local_Post == false)
                {
                    // reset related properties (though it may not necfessary to put here)
                    _CountryPostId = null;
                    _CityPost = null;
                    _ZipCode = null;
                    _Dep_Appar = null;
                    _Street_Name_No = null;

                }
                OnPropertyChanged(new PropertyChangedEventArgs("Have_Local_Post"));
            }
        }

        private string _Update_Send_KU;
        public string Update_Send_KU
        {
            get { return _Update_Send_KU; }
            set
            {
                _Update_Send_KU = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Update_Send_KU"));
            }
        }


        /*
        [StringLength(255)]
        public string Receive_State
        {
           get;set;
        }
        */

        /*
        [StringLength(255)]
        public string Receive_Date_Time
        {
            get;set;
        }
        */
        /*
        [StringLength(255)]
        public string Note_Received
        {
            get;set;
        }
        */

        /*
        [StringLength(255)]
        public string Agent_EU_ReceiverName
        {
            get;set;
        }
        */

        /*
       [StringLength(255)]
       public string Person_in_charge_Receive
       {
           get;set;
       }
       */
        /*
        [StringLength(255)]
        public string Received_Amount_EU
        {
            get;set;
        }
        */
        /*
        private string _Discount_Post_Cost_Received;
        [StringLength(255)]
        public string Discount_Post_Cost_Received
        {
            get { return _Discount_Post_Cost_Received; }
            set
            {
                _Discount_Post_Cost_Received = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Discount_Post_Cost_Received"));
            }
        }
        */
        /*
        [StringLength(255)]
        public string PaymentWAY_Cash_PIN_Bank
        {
            get;set;
        }
        */

        /*
        [StringLength(255)]
        public string Update_Receive_EU
        {
            get;set;
        }
        */

        /*
        [StringLength(255)]
        public string Sender_ID_Type
        {
            get;set;
        }
        */

        private long? _Num;
        public long? Num
        {
            get { return _Num; }
            set
            {
                _Num = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Num"));
            }
        }



        private decimal? _CommissionKG;
        public decimal? CommissionKG
        {
            get { return _CommissionKG; }
            set
            {
                _CommissionKG = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommissionKG"));
            }
        }

        private decimal? _CommissionBox;
        public decimal? CommissionBox
        {
            get { return _CommissionBox; }
            set
            {
                _CommissionBox = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CommissionBox"));
            }
        }
        private long? _Weight_L_cm;
        public long? Weight_L_cm
        {
            get { return _Weight_L_cm; }
            set
            {
                _Weight_L_cm = value;
                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        if (!_Weight_L_cm.HasValue)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("Weight_L_cm", errors);
                        }
                        else if (_Weight_L_cm == 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("Weight_L_cm", errors);
                        }
                        else
                        {
                            ClearErrors("Weight_L_cm");
                        }
                    }
                    else { ClearErrors("Weight_L_cm"); }
                }
               
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_L_cm"));
            }
        }
        private long? _Weight_W_cm;
        public long? Weight_W_cm
        {
            get { return _Weight_W_cm; }
            set
            {
                _Weight_W_cm = value;

                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        if (!_Weight_W_cm.HasValue)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("Weight_W_cm", errors);
                        }
                        else if (_Weight_W_cm == 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("Weight_W_cm", errors);
                        }
                        else
                        {
                            ClearErrors("Weight_W_cm");
                        }
                    }
                    else { ClearErrors("Weight_W_cm"); }
                }
                
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_W_cm"));
            }
        }
        private long? _Weight_H_cm;
        public long? Weight_H_cm
        {
            get { return _Weight_H_cm; }
            set
            {
                _Weight_H_cm = value;

                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        if (!_Weight_H_cm.HasValue)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("Weight_H_cm", errors);
                        }
                        else if (_Weight_H_cm == 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("Weight_H_cm", errors);
                        }
                        else
                        {
                            ClearErrors("Weight_H_cm");
                        }
                    }
                    else { ClearErrors("Weight_H_cm"); }
                }
               
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_H_cm"));
            }
        }


      
        private double? _Weight_BySizeValue;
        [NotMapped]
        public double? Weight_BySizeValue
        {
            get { return _Weight_BySizeValue; }
            set
            {
                _Weight_BySizeValue = value;
                if (isvalidating)
                {
                    if (_Weight_By_Size_Is_Checked)
                    {
                        double weightBysize = _Weight_BySizeValue.HasValue ? (double)_Weight_BySizeValue : 0;
                        double realWeight = _Weight_Kg.HasValue ? (double)_Weight_Kg : 0;

                        if (weightBysize <= realWeight)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(string.Format("{0} ({1})", "This Value Must Be Greater Than The Real Weight", realWeight.ToString()));
                            SetErrors("Weight_BySizeValue", errors);
                        }
                        else
                        { ClearErrors("Weight_BySizeValue"); }
                    }
                    else
                    { ClearErrors("Weight_BySizeValue"); }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_BySizeValue"));
            }

        }

     
        private bool _Weight_By_Size_Is_Checked;
        [NotMapped]
        public bool Weight_By_Size_Is_Checked
        {
            get { return _Weight_By_Size_Is_Checked; }
            set
            {
                _Weight_By_Size_Is_Checked = value;
                
                OnPropertyChanged(new PropertyChangedEventArgs("Weight_By_Size_Is_Checked"));
            }
        }

        private long? _BranchId;
        public long? BranchId
        {
            get { return _BranchId; }
            set
            {
                _BranchId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BranchId"));
            }
        }


        private long? _UserId;
        public long? UserId
        {
            get { return _UserId; }
            set
            {
                _UserId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserId"));
            }
        }


        private long? _CountryAgentId;
        public long? CountryAgentId
        {
            get { return _CountryAgentId; }
            set
            {
                _CountryAgentId = value;
                if (isvalidating)
                {
                    if (_CountryAgentId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CountryAgentId", errors);
                    }
                    else
                    { ClearErrors("CountryAgentId"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("CountryAgentId"));
            }
        }


        private long? _CountryPostId;
        public long? CountryPostId
        {
            get { return _CountryPostId; }
            set
            {
                _CountryPostId = value;

                if (isvalidating)
                {
                    if (_Have_Local_Post.HasValue)
                    {
                        if ((bool)_Have_Local_Post)
                        {
                            if (_CountryPostId.HasValue == false)
                            {
                                List<string> errors = new List<string>();
                                errors.Add(CommonMessages.RequiredValue);
                                SetErrors("CountryPostId", errors);
                            }
                            else
                            { ClearErrors("CountryPostId"); }
                        }
                        else
                        { ClearErrors("CountryPostId"); }
                    }
                    else
                    { ClearErrors("CountryPostId"); }


                }

                OnPropertyChanged(new PropertyChangedEventArgs("CountryPostId"));
            }
        }

        private long? _AgentId;
        public long? AgentId
        {
            get { return _AgentId; }
            set
            {
                _AgentId = value;
                if (isvalidating)
                {
                    if (_AgentId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("AgentId", errors);
                    }
                    else
                    { ClearErrors("AgentId"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("AgentId"));
            }
        }


        private long? _CityPostId;
        public long? CityPostId
        {
            get { return _CityPostId; }
            set
            {
                _CityPostId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CityPostId"));
            }
        }


        private long? _Person_in_charge_Id;
        public long? Person_in_charge_Id
        {
            get { return _Person_in_charge_Id; }
            set
            {
                _Person_in_charge_Id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Person_in_charge_Id"));
            }
        }




        private byte[] _stamp;
        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] stamp
        {
            get { return _stamp; }
            set
            {
                _stamp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("stamp"));
            }
        }



        //IsImported
        private bool? _IsImported;

        public bool? IsImported
        {
            get { return _IsImported; }
            set
            {
                _IsImported = value;
              
            }
        }






        private Agent _virtual_Agent;
        public virtual Agent virtual_Agent
        {
            get { return _virtual_Agent; }
            set
            {
                _virtual_Agent = value;
                OnPropertyChanged(new PropertyChangedEventArgs("virtual_Agent"));
            }
        }


        private Agent _virtual_Branch;
        public virtual Agent virtual_Branch
        {
            get { return _virtual_Branch; }
            set
            {
                _virtual_Branch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("virtual_Branch"));
            }
        }

        /*
        private Branch _Branch;

        
        public virtual Branch Branch {
            get { return _Branch; }
            set
            {
                _Branch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Branch"));
            }
        }
        */

        private Country _Country;
        public virtual Country Country
        {
            get { return _Country; }
            set
            {
                _Country = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Country"));
            }
        }

        private User _User;

       
        public virtual User User
        {
            get { return _User; }
            set
            {
                _User = value;
                OnPropertyChanged(new PropertyChangedEventArgs("User"));
            }
        }


        private IdentityType _IdentityType;
        public virtual IdentityType IdentityType
        {
            get { return _IdentityType; }
            set
            {
                _IdentityType = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IdentityType"));
            }
        }

        private string _ExchangeTitle;
        [NotMapped]
        public string ExchangeTitle
        {
            get { return _ExchangeTitle; }
            set
            {
                _ExchangeTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ExchangeTitle"));
            }
        }

        private string _AgentComissionTitle;
        [NotMapped]
        public string AgentComissionTitle
        {
            get { return _AgentComissionTitle; }
            set
            {
                _AgentComissionTitle = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AgentComissionTitle"));
            }
        }

        public void Set_Currency_Exchange_Title()
        {
            ExchangeTitle = string.Format("{0} {1} = {2} {3}", "1", Currency_Type, Currency_Rate_1_IQD, "IQD");
        }

        public void Set_AgentComission_Title()
        {
           // AgentComissionTitle = string.Format("{0} {1} = {2} {3}", "1", Currency_Type, Currency_Rate_1_IQD, "IQD");
        }

        public void Set_IndentityTypeShortcut()
        {
            if (IdentityType != null)
            { Sender_ID_Type_Shortcut = IdentityType.Shortcut; }
            else
            {
                Sender_ID_Type_Shortcut = null;
            }
          
        }


        public void set_non_BoundFeilds()
        {
            /*
            // set the weight by size value and calculated weight by volume
            BusinessLogic.ClientCodeBL ccbl = new BusinessLogic.ClientCodeBL();
            ccbl.clientCode = this;
            ccbl.Set_IsWeight_By_SizeChecked();
            ccbl.Calculate_Weight_By_Size();
            */
        }




    }
}
