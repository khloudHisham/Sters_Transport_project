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


    [Table("tbl_Currency")]
    public partial class Currency : INotifyPropertyChanged, INotifyDataErrorInfo
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

        private bool _NameIsReadOnly;
        [NotMapped]
        public bool NameIsReadOnly
        {
            get { return _NameIsReadOnly; }
            set
            {
                _NameIsReadOnly = value;

                OnPropertyChanged(new PropertyChangedEventArgs("NameIsReadOnly"));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Currency()
        {
            Agents = new HashSet<Agent>();
            NameIsReadOnly = false;
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



        private string _Name;
        [StringLength(50)]
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_Name))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Name", errors);
                    }
                    else
                    {
                        ClearErrors("Name");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        private string _NameAR;
        [StringLength(50)]
        public string NameAR
        {
            get { return _NameAR; }
            set
            {
                _NameAR = value;
 
                OnPropertyChanged(new PropertyChangedEventArgs("NameAR"));
            }
        }


        private string _NameKU;
        [StringLength(50)]
        public string NameKU
        {
            get { return _NameKU; }
            set
            {
                _NameKU = value;

                OnPropertyChanged(new PropertyChangedEventArgs("NameKU"));
            }
        }




        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Agent> Agents { get; set; }
    }
}
