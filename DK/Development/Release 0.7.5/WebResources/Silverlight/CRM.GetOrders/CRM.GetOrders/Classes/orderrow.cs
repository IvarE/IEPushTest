using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using CGIXrm;
using System.Collections.ObjectModel;
using CRM.GetOrders.GetOrdersServiceReference;
using System.Reflection;
using System.Linq;

namespace CRM.GetOrders
{
    public class orderrow : XrmBaseNotify
    {

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; OnPropertyChanged("Code"); }
        }

        private decimal _discount;
        public decimal Discount
        {
            get { return _discount; }
            set { _discount = value; OnPropertyChanged("Discount"); }
        }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }
        
        private decimal _price;
        public decimal Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
        }
        
        private Byte _quantity;
        public Byte Quantity
        {
            get { return _quantity; }
            set { _quantity = value; OnPropertyChanged("Quantity"); }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private bool _isRowEnabled;
        public bool IsRowEnabled
        {
            get { return _isRowEnabled; }
            set { _isRowEnabled = value; OnPropertyChanged("IsRowEnabled"); }
        }

        private string _amountChange;
        public string AmountChange
        {
            get { return _amountChange; }
            set { _amountChange = value; OnPropertyChanged("AmountChange"); }
        }

        // TODO wrong place for this field? should be in creditorderrow or creditrow? no those exist in WCF project
        private string _reason;
        public string Reason       {
            get { return _reason; }
            set { _reason = value; OnPropertyChanged("Reason"); }
        }
        
        private decimal _rowPrice;
        public decimal RowPrice
        {
            get { return _rowPrice; }
            set { _rowPrice = value; }
        }

        public void CalculateRowPrice()
        {
            if (_quantity != 0)
            {
                _rowPrice = _price / _quantity;
            }
            else
            {
                _rowPrice = 0;
            }
        }

        
    }
}
