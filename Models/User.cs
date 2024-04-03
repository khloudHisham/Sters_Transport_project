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


    [Table("tbl_User")]
    public partial class User : INotifyPropertyChanged, INotifyDataErrorInfo
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
        public User()
        {
            ClientCodes = new HashSet<ClientCode>();
            UserStateLoging = false;
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


        private string _UserName;
        [StringLength(255)]
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_UserName))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("UserName", errors);
                    }
                    else
                    {
                        ClearErrors("UserName");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("UserName"));
            }
        }


        private string _PasswordUser;
        [StringLength(255)]
        public string PasswordUser
        {
            get { return _PasswordUser; }
            set
            {
                _PasswordUser = value;


                OnPropertyChanged(new PropertyChangedEventArgs("PasswordUser"));
            }
        }

        private long? _BranchId;

        public long? BranchId {
            get { return _BranchId; }
            set
            {
                _BranchId = value;

                if (isvalidating)
                {
                    if (_BranchId.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("BranchId", errors);
                    }
                    else
                    { ClearErrors("BranchId"); }

                }

                OnPropertyChanged(new PropertyChangedEventArgs("BranchId"));
            }
        }



        [StringLength(255)]
        public string PasswordChangeLastTimeDate { get; set; }


        private string _Authorization;
        [StringLength(255)]
        public string Authorization {
            get { return _Authorization; }
            set
            {
                _Authorization = value;

                if (isvalidating)
                {
                    if (string.IsNullOrEmpty(_Authorization))
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Authorization", errors);
                    }
                    else
                    {
                        ClearErrors("Authorization");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Authorization"));
            }
        }



        [StringLength(255)]
        public string Loging_In_DateTime { get; set; }

        [StringLength(255)]
        public string Loging_Out_DateTime { get; set; }

        [StringLength(255)]
        public string PasswordChangeHistory { get; set; }


        private bool? _UserStateLoging;
        public bool? UserStateLoging {
            get { return _UserStateLoging; }
            set
            {
                _UserStateLoging = value;


                OnPropertyChanged(new PropertyChangedEventArgs("UserStateLoging"));
            }
        }


        private string _Email;
        public string Email
        { get { return _Email; }
            set { _Email = value;

                if (isvalidating)
                {
                    if (!string.IsNullOrEmpty(_Email))
                    {
                        if (Helpers.EmailHelper.IsValidEmail(_Email) == false)
                        {
                            List<string> errors = new List<string>();
                            errors.Add(CommonMessages.RequiredValue);
                            SetErrors("Email", errors);
                        }
                        else
                        { ClearErrors("Email"); }
                       
                    }
                    else
                    {
                        ClearErrors("Email");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("Email"));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        
        public virtual ICollection<ClientCode> ClientCodes { get; set; }
    }
}
