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


    [Table("tbl_City")]
    public partial class City : INotifyPropertyChanged, INotifyDataErrorInfo
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




        public City()
        {
            WeHaveAgentsThereIn = false;
        }

        [NotMapped]
        public bool isvalidating
        {
            get;
            set;
        }




        private long _Id;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id {
            get { return _Id; }
            set
            {
                _Id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }



        private string _CityName;
        [StringLength(250)]
        public string CityName
        {
            get { return _CityName; }
            set
            {
                _CityName = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_CityName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CityName", errors);
                    }
                    else
                    {
                        ClearErrors("CityName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("CityName"));
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


        private bool? _WeHaveAgentsThereIn;
        public bool? WeHaveAgentsThereIn {
            get { return _WeHaveAgentsThereIn; }
            set
            {
                _WeHaveAgentsThereIn = value;
                OnPropertyChanged(new PropertyChangedEventArgs("WeHaveAgentsThereIn"));
            }
        }


        private Country _Country;
        public virtual Country Country { get; set; }
    }
}
