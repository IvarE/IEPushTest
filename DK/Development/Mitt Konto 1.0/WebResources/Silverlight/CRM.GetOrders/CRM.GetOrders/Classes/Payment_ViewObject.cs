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
    public class Payment_ViewObject : XrmBaseNotify
    {

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set { _Code = value; OnPropertyChanged("Code"); }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged("Name"); }
        }

        private string _ReferenceNumber;
        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { _ReferenceNumber = value; OnPropertyChanged("ReferenceNumber"); }
        }

        private decimal _Sum;
        public decimal Sum
        {
            get { return _Sum; }
            set { _Sum = value; OnPropertyChanged("Sum"); }
        }

        private string _TransactionType;
        public string TransactionType
        {
            get { return _TransactionType; }
            set { _TransactionType = value; }
        }

        private string _GiftCardCode;
        public string GiftCardCode
        {
            get { return _GiftCardCode; }
            set { _GiftCardCode = value; }
        }

        private string _TransactionId;
        public string TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }

        private string _Name_TransactionType_GiftCardCode;
        public string Name_TransactionType_GiftCardCode
        {
            get {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (string.IsNullOrEmpty(Name) == false)
                {
                    sb.Append(string.Format("Typ: {0} ",Name));
                }
                if (string.IsNullOrEmpty(TransactionType) == false)
                {
                    sb.Append(string.Format("Transaktionstyp: {0} ",TransactionType));
                }
                if (string.IsNullOrEmpty(GiftCardCode)==false) {
                    sb.Append(string.Format("Värdekod: {0} ",GiftCardCode));
                }
                return sb.ToString();
            }
            private set { _Name_TransactionType_GiftCardCode = value; }
        }
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }
}
