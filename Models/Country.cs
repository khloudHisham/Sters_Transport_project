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


    [Table("tbl_Country")]
    public partial class Country : INotifyPropertyChanged, INotifyDataErrorInfo
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


        [NotMapped]
        public bool isvalidating
        {
            get;
            set;
        }



        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Country()
        {
            
            ClientCodes = new HashSet<ClientCode>();
            Agents = new HashSet<Agent>();
            Cities = new HashSet<City>();
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


        private string _CountryName;

        [Required]
        [StringLength(250)]
        public string CountryName
        {
            get { return _CountryName; }
            set
            {
                _CountryName = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_CountryName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CountryName", errors);
                    }
                    else
                    {
                        ClearErrors("CountryName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("CountryName"));
            }
        }


        private string _CountryNameAR;

        [StringLength(250)]
        public string CountryNameAR
        {
            get { return _CountryNameAR; }
            set
            {
                _CountryNameAR = value;
 
                OnPropertyChanged(new PropertyChangedEventArgs("CountryNameAR"));
            }
        }


        private string _CountryNameKU;

        [StringLength(250)]
        public string CountryNameKU
        {
            get { return _CountryNameKU; }
            set
            {
                _CountryNameKU = value;

                OnPropertyChanged(new PropertyChangedEventArgs("CountryNameKU"));
            }
        }




        private string _Alpha_2_Code;
        [StringLength(10)]
        public string Alpha_2_Code
        {
            get { return _Alpha_2_Code; }
            set
            {
                _Alpha_2_Code = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_Alpha_2_Code))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("_Alpha_2_Code", errors);
                    }
                    else
                    {
                        ClearErrors("_Alpha_2_Code");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("_Alpha_2_Code"));
            }
        }


        private int? _Zip_Code_Digit_1;
        public int? Zip_Code_Digit_1
        {
            get { return _Zip_Code_Digit_1; }
            set
            {
                _Zip_Code_Digit_1 = value;

                if (isvalidating)
                {
                    if (_Zip_Code_Digit_1.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Zip_Code_Digit_1", errors);
                    }
                    else
                    { ClearErrors("Zip_Code_Digit_1"); }

                }

                OnPropertyChanged(new PropertyChangedEventArgs("Zip_Code_Digit_1"));
            }
        }


        private int? _Zip_Code_Digit_2;
        public int? Zip_Code_Digit_2
        {
            get { return _Zip_Code_Digit_2; }
            set
            {
                _Zip_Code_Digit_2 = value;

                if (isvalidating)
                {
                    if (_Zip_Code_Digit_2.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Zip_Code_Digit_2", errors);
                    }
                    else
                    { ClearErrors("Zip_Code_Digit_2"); }

                }

                OnPropertyChanged(new PropertyChangedEventArgs("Zip_Code_Digit_2"));
            }
        }


        private int? _Zip_Code_TXT;
        public int? Zip_Code_TXT
        {
            get { return _Zip_Code_TXT; }
            set
            {
                _Zip_Code_TXT = value;

                if (isvalidating)
                {
                    if (_Zip_Code_TXT.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Zip_Code_TXT", errors);
                    }
                    else
                    { ClearErrors("Zip_Code_TXT"); }

                }

                OnPropertyChanged(new PropertyChangedEventArgs("Zip_Code_TXT"));
            }
        }

        [StringLength(250)]
        public string Zip_Code_LINK { get; set; }


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
        public decimal? CurrencyAgainst1IraqDinar {
            get { return _CurrencyAgainst1IraqDinar; }
            set
            {
                _CurrencyAgainst1IraqDinar = value;

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

                  



                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyAgainst1IraqDinar"));
            }
        }


        private byte[] _ImgForBoxLabel;
        public byte[] ImgForBoxLabel
        {
            get { return _ImgForBoxLabel; }
            set
            {
                _ImgForBoxLabel = value;

                
                OnPropertyChanged(new PropertyChangedEventArgs("ImgForBoxLabel"));
            }
        }

        private byte[] _ImgForPostLabel;
        public byte[] ImgForPostLabel
        {
            get { return _ImgForPostLabel; }
            set
            {
                _ImgForPostLabel = value;


                OnPropertyChanged(new PropertyChangedEventArgs("ImgForPostLabel"));
            }
        }


        private bool? _WeHaveAgentsThereIn;
        public bool? WeHaveAgentsThereIn {
            get { return _WeHaveAgentsThereIn; }
            set
            {
                _WeHaveAgentsThereIn = value;


                OnPropertyChanged(new PropertyChangedEventArgs("WeHaveAgentsThereIn"));
            }
        }


        private bool? _CheckMaximumWeighBox;
        public bool? CheckMaximumWeighBox
        {
            get { return _CheckMaximumWeighBox; }
            set
            {
                _CheckMaximumWeighBox = value;


                OnPropertyChanged(new PropertyChangedEventArgs("CheckMaximumWeighBox"));
            }
        }


        private decimal? _MaximumWeighBox;
        [Column(TypeName = "numeric")]
        public decimal? MaximumWeighBox
        {
            get { return _MaximumWeighBox; }
            set
            {
                _MaximumWeighBox = value;

                if (isvalidating)
                {
                    bool checkmaxweight = CheckMaximumWeighBox.HasValue ? (bool)CheckMaximumWeighBox : false;
                    if (checkmaxweight)
                    {
                        if (!_MaximumWeighBox.HasValue)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("MaximumWeighBox", errors);
                        }
                        else if (_MaximumWeighBox == 0)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                            SetErrors("MaximumWeighBox", errors);
                        }
                        else
                        { ClearErrors("MaximumWeighBox"); }
                    }
                    else
                    {
                        ClearErrors("MaximumWeighBox");
                    }

                }

                OnPropertyChanged(new PropertyChangedEventArgs("MaximumWeighBox"));
            }
        }


        private long? _MinimumKG;
        public long? MinimumKG {
            get { return _MinimumKG; }
            set
            {
                _MinimumKG = value;

               

                OnPropertyChanged(new PropertyChangedEventArgs("MinimumKG"));
            }
        }


        public int? Special_Index { get; set; }



        private string _continent;
        [StringLength(50)]
        public string continent
        {
            get { return _continent; }
            set
            {
                _continent = value;

               
                OnPropertyChanged(new PropertyChangedEventArgs("continent"));
            }
        }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientCode> ClientCodes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent> Agents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City> Cities { get; set; }
    }
}
