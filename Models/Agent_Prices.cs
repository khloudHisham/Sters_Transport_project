using StersTransport.GlobalData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StersTransport.Models
{
    public partial class Agent_Prices: INotifyPropertyChanged, INotifyDataErrorInfo
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



        private long _ID;
        public long ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }


        private long? _Agent_Id;
        public long? Agent_Id
        {
            get { return _Agent_Id; }
            set
            {
                _Agent_Id = value;
                if (isvalidating)
                {
                    if (_Agent_Id.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Agent_Id", errors);
                    }
                    else
                    { ClearErrors("Agent_Id"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("Agent_Id"));
            }
        }

        private long? _Agent_Id_Destination;

        public long? Agent_Id_Destination
        {
            get { return _Agent_Id_Destination; }
            set
            {
                _Agent_Id_Destination = value;
                if (isvalidating)
                {
                    if (_Agent_Id_Destination.HasValue == false)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Agent_Id_Destination", errors);
                    }
                    else
                    { ClearErrors("Agent_Id_Destination"); }

                }
                OnPropertyChanged(new PropertyChangedEventArgs("Agent_Id_Destination"));
            }
        }



        private decimal? _PriceKG;
        public decimal? PriceKG
        {
            get { return _PriceKG; }
            set
            {
                _PriceKG = value;
                if (isvalidating)
                {
                    if (!_PriceKG.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("PriceKG", errors);
                    }
                    else if (_PriceKG == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("PriceKG", errors);
                    }
                    else
                    {
                        ClearErrors("PriceKG");
                    }
                }
                OnPropertyChanged(new PropertyChangedEventArgs("PriceKG"));
            }
        }


        private decimal? _Price1to5_7KG;
        public decimal? Price1to5_7KG
        {
            get { return _Price1to5_7KG; }
            set
            {
                _Price1to5_7KG = value;

                if (isvalidating)
                {
                    if (!_Price1to5_7KG.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("Price1to5_7KG", errors);
                    }
                    else if (_Price1to5_7KG == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("Price1to5_7KG", errors);
                    }
                    else
                    {
                        ClearErrors("Price1to5_7KG");
                    }
                }



                OnPropertyChanged(new PropertyChangedEventArgs("Price1to5_7KG"));
            }
        }


        private decimal? _PriceDoorToDoor;
        public decimal? PriceDoorToDoor {
            get { return _PriceDoorToDoor; }
            set
            {
                _PriceDoorToDoor = value;
                if (isvalidating)
                {
                    if (!_PriceDoorToDoor.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("PriceDoorToDoor", errors);
                    }
                    
                    else
                    {
                        ClearErrors("PriceDoorToDoor");
                    }
                }

                OnPropertyChanged(new PropertyChangedEventArgs("PriceDoorToDoor"));
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


        private decimal? _BoxPackaging;
        public decimal? BoxPackaging {
            get { return _BoxPackaging; }
            set
            {
                _BoxPackaging = value;

                if (isvalidating)
                {
                    if (!_BoxPackaging.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("BoxPackaging", errors);
                    }
                    else if (_BoxPackaging == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("BoxPackaging", errors);
                    }
                    else
                    {
                        ClearErrors("BoxPackaging");
                    }
                }


                OnPropertyChanged(new PropertyChangedEventArgs("BoxPackaging"));
            }
        }


        private double? _CurrencyEQ;
        public double? CurrencyEQ
        {
            get { return _CurrencyEQ; }
            set
            {
                _CurrencyEQ = value;

                if (isvalidating)
                {
                    if (!_CurrencyEQ.HasValue)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.RequiredValue);
                        SetErrors("CurrencyEQ", errors);
                    }
                    else if (_CurrencyEQ == 0)
                    {
                        List<string> errors = new List<string>();
                        errors.Add(CommonMessages.ValueMustBeGreaterThanZero);
                        SetErrors("CurrencyEQ", errors);
                    }
                    else
                    {
                        ClearErrors("CurrencyEQ");
                    }
                }


                OnPropertyChanged(new PropertyChangedEventArgs("CurrencyEQ"));
            }
        }



        public virtual Agent virtual_Agent { get; set; }
        public virtual Agent virtual_Agent1 { get; set; }
    }
}
