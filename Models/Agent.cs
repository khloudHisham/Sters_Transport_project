namespace StersTransport.Models
{
    using StersTransport.GlobalData;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;



    [Table("tbl_Agent")]
    public partial class Agent : INotifyPropertyChanged, INotifyDataErrorInfo
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


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Agent()
        {
            HavePostService = false;
            ClientCodes = new HashSet<ClientCode>();
            ClientCodes1 = new HashSet<ClientCode>();
            Agent_Prices = new HashSet<Agent_Prices>();
            Agent_Prices1 = new HashSet<Agent_Prices>();
        }


        [NotMapped]
        public bool isvalidating
        {
            get;
            set;
        }

        private long _Id;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id
        {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }



        private string _AgentName;
        [StringLength(255)]
        public string AgentName
        {
            get { return _AgentName; }
            set
            {
                _AgentName = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_AgentName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("AgentName", errors);
                    }
                    else
                    {
                        ClearErrors("AgentName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("AgentName"));
            }
        }



        private long? _CountryId;
        public long? CountryId
        {
            get { return _CountryId; }
            set
            {
                _CountryId = value;
                if (isvalidating)
                {
                    if (_CountryId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CountryId", errors);
                    }
                    else
                    { ClearErrors("CountryId"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("CountryId"));
            }
        }



        private string _ContactPersonName;
        [StringLength(255)]
        public string ContactPersonName
        {
            get { return _ContactPersonName; }
            set
            {
                _ContactPersonName = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_ContactPersonName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("ContactPersonName", errors);
                    }
                    else
                    {
                        ClearErrors("ContactPersonName");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("ContactPersonName"));
            }
        }



        private string _CompanyName;
        [StringLength(255)]
        public string CompanyName
        {
            get { return _CompanyName; }
            set
            {
                _CompanyName = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_CompanyName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CompanyName", errors);
                    }
                    else
                    {
                        ClearErrors("CompanyName");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("CompanyName"));
            }
        }



        private string _PhoneNo1;
        [StringLength(255)]
        public string PhoneNo1
        {
            get { return _PhoneNo1; }
            set
            {
                _PhoneNo1 = value;
                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_PhoneNo1))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("PhoneNo1", errors);
                    }
                    else
                    {
                        ClearErrors("PhoneNo1");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("PhoneNo1"));
            }
        }



        private string _PhoneNo2;
        [StringLength(255)]
        public string PhoneNo2
        {
            get { return _PhoneNo2; }
            set
            {
                _PhoneNo2 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PhoneNo2"));
            }
        }


        private string _Address;
        [StringLength(255)]
        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
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
                OnPropertyChanged(new PropertyChangedEventArgs("ZipCode"));
            }
        }


        private long? _CityId;
     
        public long? CityId {
            get { return _CityId; }
            set
            {
                _CityId = value;

                if (isvalidating)
                {
                    if (_CityId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CityId", errors);
                    }
                    else
                    { ClearErrors("CityId"); }

                }


                OnPropertyChanged(new PropertyChangedEventArgs("CityId"));
            }
        }


        private string _E_mail;
        [Column("E-mail")]
        [StringLength(255)]
        public string E_mail
        {
            get { return _E_mail; }
            set
            {
                _E_mail = value;
                OnPropertyChanged(new PropertyChangedEventArgs("E_mail"));
            }
        }


        private string _Web;
        [StringLength(255)]
        public string Web
        {
            get { return _Web; }
            set
            {
                _Web = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Web"));
            }
        }



        private long? _CurrencyId;
        public long? CurrencyId
        {
            get { return _CurrencyId; }
            set
            {
                _CurrencyId = value;

                if (isvalidating)
                {
                    if (_CurrencyId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CurrencyId", errors);
                    }
                    else
                    { ClearErrors("CurrencyId"); }

                }


                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyId"));
            }
        }



        private decimal? _CurrencyAgainst1IraqDinar;
        public decimal? CurrencyAgainst1IraqDinar
        {
            get { return _CurrencyAgainst1IraqDinar; }
            set
            {
                _CurrencyAgainst1IraqDinar = value;


                /*
                if (isvalidating)
                {
                    if (!_CurrencyAgainst1IraqDinar.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CurrencyAgainst1IraqDinar", errors);
                    }
                    else if (_CurrencyAgainst1IraqDinar == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("CurrencyAgainst1IraqDinar", errors);
                    }
                    else
                    {
                        ClearErrors("CurrencyAgainst1IraqDinar");
                    }
                }
               */


                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyAgainst1IraqDinar"));
            }
        }


        private decimal? _PriceKGIraqDinar;
        public decimal? PriceKGIraqDinar
        {
            get { return _PriceKGIraqDinar; }
            set
            {
                _PriceKGIraqDinar = value;

                /*
                if (isvalidating)
                {
                    if (!_PriceKGIraqDinar.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("PriceKGIraqDinar", errors);
                    }
                    else if (_PriceKGIraqDinar == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("PriceKGIraqDinar", errors);
                    }
                    else
                    {
                        ClearErrors("PriceKGIraqDinar");
                    }
                }
              */


                OnPropertyChanged(new PropertyChangedEventArgs("PriceKGIraqDinar"));
            }
        }


        private decimal? _Price1to5_7KGIraqDinar;
        public decimal? Price1to5_7KGIraqDinar
        {
            get { return _Price1to5_7KGIraqDinar; }
            set
            {
                _Price1to5_7KGIraqDinar = value;

                /*
                if (isvalidating)
                {
                    if (!_Price1to5_7KGIraqDinar.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Price1to5_7KGIraqDinar", errors);
                    }
                    else if (_Price1to5_7KGIraqDinar == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("Price1to5_7KGIraqDinar", errors);
                    }
                    else
                    {
                        ClearErrors("Price1to5_7KGIraqDinar");
                    }
                }
             */


                OnPropertyChanged(new PropertyChangedEventArgs("Price1to5_7KGIraqDinar"));
            }
        }


        /*
        private string _PriceDoorToDoorEach10kgIraqDinarOrALL_IN;
        [StringLength(255)]
        public string PriceDoorToDoorEach10kgIraqDinarOrALL_IN { get; set; }
        */


        /*
        [StringLength(255)]
        public string InsertDate { get; set; }

    
      
        */
        public int? CommissionType { get; set; }


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


        private decimal? _PriceDoorToDoorEach10kgIraqDinar;
        public decimal? PriceDoorToDoorEach10kgIraqDinar
        {
            get { return _PriceDoorToDoorEach10kgIraqDinar; }
            set
            {
                _PriceDoorToDoorEach10kgIraqDinar = value;

                /*
                if (isvalidating)
                {
                    if (!_PriceDoorToDoorEach10kgIraqDinar.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("PriceDoorToDoorEach10kgIraqDinar", errors);
                    }
                    
                    else
                    {
                        ClearErrors("PriceDoorToDoorEach10kgIraqDinar");
                    }
                }
             */


                OnPropertyChanged(new PropertyChangedEventArgs("_PriceDoorToDoorEach10kgIraqDinar"));
            }
        }



        private bool? _HavePostService;
        public bool? HavePostService
        {
            get { return _HavePostService; }
            set
            {
                _HavePostService = value;
                OnPropertyChanged(new PropertyChangedEventArgs("HavePostService"));
            }
        }



        public string AddressAR { get; set; }

        public string AddressKu { get; set; }


        [StringLength(255)]
        public string Note { get; set; }

        private string _CharactersPrefix;
        [StringLength(5)]
        public string CharactersPrefix
        {
            get { return _CharactersPrefix; }
            set
            {
                _CharactersPrefix = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_CharactersPrefix))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CharactersPrefix", errors);
                    }
                    else
                    {
                        ClearErrors("CharactersPrefix");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("CharactersPrefix"));
                SetCodePreview();
            }
        }

        private string _YearPrefix;
        [StringLength(5)]
        public string YearPrefix
        {
            get { return _YearPrefix; }
            set
            {
                _YearPrefix = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_YearPrefix))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("YearPrefix", errors);
                    }
                    else
                    {
                        ClearErrors("YearPrefix");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("YearPrefix"));
                SetCodePreview();
            }
        }


        private long? _NumberOfDigits; // in fact im surprized this database feild has bigint datatype(from the previous database design...keep for now ) 
        public long? NumberOfDigits
        {
            get { return _NumberOfDigits; }
            set
            {
                _NumberOfDigits = value;

                if (isvalidating)
                {
                    if (_NumberOfDigits.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("NumberOfDigits", errors);
                    }
                    else if ((long)_NumberOfDigits > 7)
                    {
                        List<string> errors = new List<string>();
                        errors.Add("Max Value Is 7");
                        SetErrors("NumberOfDigits", errors);
                    }
                    else
                    { ClearErrors("NumberOfDigits"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("NumberOfDigits"));
                SetCodePreview();
            }
        }

        private string _codeStyle;
        [NotMapped]
        public string codeStyle
        {
            get { return _codeStyle; }
            set
            {
                _codeStyle = value;



                OnPropertyChanged(new PropertyChangedEventArgs("codeStyle"));
            }
        }

        private string _InvoiceLanguage;
        [StringLength(50)]
        public string InvoiceLanguage
        {
            get { return _InvoiceLanguage; }
            set
            {
                _InvoiceLanguage = value;

                OnPropertyChanged(new PropertyChangedEventArgs("InvoiceLanguage"));
            }
        }


        private bool _isarabiclanguage;
        [NotMapped]
        public bool isarabiclanguage
        {
            get { return _isarabiclanguage; }
            set
            {
                _isarabiclanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isarabiclanguage"));
            }
        }


        private bool _iskurdishlanguage;
        [NotMapped]
        public bool iskurdishlanguage
        {
            get { return _iskurdishlanguage; }
            set
            {
                _iskurdishlanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("iskurdishlanguage"));
            }
        }

        private bool _isenglishlanguage;
        [NotMapped]
        public bool isenglishlanguage
        {
            get { return _isenglishlanguage; }
            set
            {
                _iskurdishlanguage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("isenglishlanguage"));
            }
        }



        // this is not the only way (and not the best) for doing this (we can do it in converters..later)
        // the best way of all is to make another database table for languages ....
        public void setInvoiceLanguage() // on demand
        {
            if (isarabiclanguage)
            { InvoiceLanguage = "Ar"; }
            else if (iskurdishlanguage)
            { InvoiceLanguage = "Ku"; }
            else if (isenglishlanguage)
            {
               InvoiceLanguage = "En";
            }
        }

        public void setlanguageflags() // on demand
        {
            switch (InvoiceLanguage)
            {
                case "Ar":
                    isarabiclanguage = true;
                    iskurdishlanguage = false;
                    isenglishlanguage = false;
                    break;
                case "Ku":
                    iskurdishlanguage = true;
                    isarabiclanguage = false;
                    isenglishlanguage = false;
                    break;
                case "En":
                    isenglishlanguage = true;
                    iskurdishlanguage = false;
                    isarabiclanguage = false;
                    break;
                default:
                    break;
            }
        }


        private bool? _IsLocalCompanyBranch;
        public bool? IsLocalCompanyBranch
        {
            get { return _IsLocalCompanyBranch; }
            set
            {
                _IsLocalCompanyBranch = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsLocalCompanyBranch"));
            }
        }


        private bool? _AgentIsDisabled;
        public bool? AgentIsDisabled
        {
            get { return _AgentIsDisabled; }
            set
            {
                _AgentIsDisabled = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AgentIsDisabled"));
            }
        }

        public void SetCodePreview()
        {
            try
            {
                int num = 1;
                string leadingzerostringformatparameter = string.Format("{0}{1}", "D", NumberOfDigits.ToString());
                string numberportion = num.ToString(leadingzerostringformatparameter);
                codeStyle = CharactersPrefix + "-" + YearPrefix + numberportion;
            }
            catch (Exception) { }
        }

        public string PhonesDisplayString { get; set; }

        public void set_PhonesDisplayString()
        {
            PhonesDisplayString = string.Empty;
            if (!string.IsNullOrEmpty(PhoneNo1))
            {
                PhonesDisplayString += PhoneNo1;
            }
            if (!string.IsNullOrEmpty(PhoneNo2))
            {
                PhonesDisplayString += " / " + PhoneNo2;
            }
        }


        public Int16? PostDomainID { get; set; }




        private ICollection<ClientCode> _ClientCodes;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientCode> ClientCodes
        {
            get { return _ClientCodes; }
            set
            {
                _ClientCodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClientCodes"));
            }
        }


        private ICollection<ClientCode> _ClientCodes1;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientCode> ClientCodes1
        {
            get { return _ClientCodes1; }
            set
            {
                _ClientCodes1 = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ClientCodes1"));
            }
        }



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


        private Currency _Currency;
        public virtual Currency Currency
        {
            get { return _Currency; }
            set
            {
                _Currency = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Currency"));
            }
        }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent_Prices> Agent_Prices { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent_Prices> Agent_Prices1 { get; set; }
    }
}
