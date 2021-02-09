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
    public class orderheader : XrmBaseNotify
    {

        private DateTime _OrderDate;
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; OnPropertyChanged("OrderDate"); }
        }

        private string _OrderNumber;
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; OnPropertyChanged("OrderNumber"); }
        }

        private string _OrderStatus;
        public string OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; OnPropertyChanged("OrderStatus"); }
        }

        private decimal _OrderTotal;
        public decimal OrderTotal
        {
            get { return _OrderTotal; }
            set { _OrderTotal = value; OnPropertyChanged("OrderTotal"); }
        }

        private decimal _OrderTotalVat;
        public decimal OrderTotalVat
        {
            get { return _OrderTotalVat; }
            set { _OrderTotalVat = value; OnPropertyChanged("OrderTotalVat"); }
        }

        private string _OrderType;
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; OnPropertyChanged("OrderType"); }
        }

        private ObservableCollection<orderrow> _OrderItems;
        public ObservableCollection<orderrow> OrderItems
        {
            get { return _OrderItems; }
            set { _OrderItems = value; OnPropertyChanged("OrderItems"); }
        }

        private ObservableCollection<payment> _Payments;
        public ObservableCollection<payment> Payments
        {
            get { return _Payments; }
            set { _Payments = value; OnPropertyChanged("Payments"); }
        }

        private ObservableCollection<ShippingAddress_ViewObject> _ShippingAddress;
        public ObservableCollection<ShippingAddress_ViewObject> ShippingAddress
        {
            get { return _ShippingAddress; }
            set { _ShippingAddress = value; OnPropertyChanged("ShippingAddress"); }
        }

        private string _creditAmount;
        public string CreditAmount
        {
            get { return _creditAmount; }
            set { _creditAmount = value; OnPropertyChanged("CreditAmount"); }
        }

        private bool _isCellEnabled;
        public bool IsCellEnabled
        {
            get { return _isCellEnabled; }
            set { _isCellEnabled = value; OnPropertyChanged("IsCellEnabled"); }
        }

        private string _fontStyle;
        public string FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; OnPropertyChanged("FontStyle"); }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private decimal _valueCodeAmount;
        public decimal ValueCodeAmount
        {
            get { return _valueCodeAmount; }
            set { _valueCodeAmount = value; OnPropertyChanged("ValueCodeAmount"); }
        }

        private decimal _otherAmount;
        public decimal OtherAmount
        {
            get { return _otherAmount; }
            set { _otherAmount = value; OnPropertyChanged("OtherAmount"); }
        }

        private decimal _amountThatCanBeCredit;
        public decimal AmountThatCanBeCredit
        {
            get { return _amountThatCanBeCredit; }
            set { _amountThatCanBeCredit = value; OnPropertyChanged("AmountThatCanBeCredit"); }
        }

        private decimal _amountThatCanNotBeCredit;
        public decimal AmountThatCanNotBeCredit
        {
            get { return _amountThatCanNotBeCredit; }
            set { _amountThatCanNotBeCredit = value; OnPropertyChanged("AmountThatCanNotBeCredit"); }
        }

        public string GetDate
        {
            get
            {
                return _OrderDate.ToShortDateString().ToString();
            }
        }

        public string GetDateTime
        {
            get
            {
                return _OrderDate.ToString("yyyy-MM-dd HH:mm");
            }
        }


    }
}
