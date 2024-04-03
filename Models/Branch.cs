namespace StersTransport.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using StersTransport.GlobalData;
    using System.Collections;
    using System.ComponentModel;



    [Table("tbl_Branch")]
    public partial class Branch: INotifyPropertyChanged, INotifyDataErrorInfo
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
        public Branch()
        {
            ClientCodes = new HashSet<ClientCode>();
            IsLocalCompanyBranch = false;
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


        private string _BranchName;
        [StringLength(255)]
        public string BranchName
        {
            get { return _BranchName; }
            set
            {
                _BranchName = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_BranchName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("BranchName", errors);
                    }
                    else
                    {
                        ClearErrors("BranchName");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("BranchName"));
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


        private string _BranchCompany;
        [StringLength(255)]
        public string BranchCompany
        {
            get { return _BranchCompany; }
            set
            {
                _BranchCompany = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_BranchCompany))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("BranchCompany", errors);
                    }
                    else
                    {
                        ClearErrors("BranchCompany");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("BranchCompany"));
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

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_Address))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Address", errors);
                    }
                    else
                    {
                        ClearErrors("Address");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Address"));
            }
        }


        
        public string AddressAR { get; set; }

        public string AddressKu { get; set; }



        private long? _CityId;
       
        public long? CityId
        {
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


        // this is not the only way (and not the best) for doing this (we can do it in converters..later)
        // the best way of all is to make another database table for languages ....
        public void setInvoiceLanguage() // on demand
        {
            if (isarabiclanguage)
            { InvoiceLanguage = "Ar"; }
            else if (iskurdishlanguage)
            { InvoiceLanguage = "Ku"; }
        }

        public void setlanguageflags() // on demand
        {
            switch (InvoiceLanguage)
            {
                case "Ar":
                    isarabiclanguage = true;
                    break;
                case "Ku":
                    iskurdishlanguage = true;
                    break;
                default:
                    break;
            }
        }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]



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


        public virtual ICollection<ClientCode> ClientCodes { get; set; }
    }
}
