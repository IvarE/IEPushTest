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
    public class CreditOrderRow_ViewObject : XrmBaseNotify
    {
        private string _row;
        public string Row
        {
            get { return _row; }
            set { _row = value; OnPropertyChanged("Row"); }
        }

        private string _orderNumber;
        public string OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; OnPropertyChanged("OrderNumber"); }
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("Date"); }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set { _time = value; OnPropertyChanged("Time"); }
        }

        private string _product;
        public string Product
        {
            get { return _product; }
            set { _product = value; OnPropertyChanged("Product"); }
        }

        private string _referenceNumber;
        public string ReferenceNumber
        {
            get { return _referenceNumber; }
            set { _referenceNumber = value; OnPropertyChanged("ReferenceNumber"); }
        }

        private string _createdby;
        public string CreatedBy
        {
            get { return _createdby; }
            set { _createdby = value; OnPropertyChanged("CreatedBy"); }
        }

        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; OnPropertyChanged("Reason"); }
        }

        private string _sum;
        public string Sum
        {
            get { return _sum; }
            set { _sum = value; OnPropertyChanged("Sum"); }
        }
    }
}
