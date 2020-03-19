using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class OrderHeader
    {
        #region Public Properties
        private DateTime _OrderDate;
        [DataMember]
        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = value; }
        }

        private string _OrderNumber;
        [DataMember]
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }

        private string _OrderStatus;
        [DataMember]
        public string OrderStatus
        {
            get { return _OrderStatus; }
            set { _OrderStatus = value; }
        }

        private decimal _OrderTotal;
        [DataMember]
        public decimal OrderTotal
        {
            get { return _OrderTotal; }
            set { _OrderTotal = value; }
        }

        private decimal _OrderTotalVat;
        [DataMember]
        public decimal OrderTotalVat
        {
            get { return _OrderTotalVat; }
            set { _OrderTotalVat = value; }
        }

        private string _OrderType;
        [DataMember]
        public string OrderType
        {
            get { return _OrderType; }
            set { _OrderType = value; }
        }

        private ObservableCollection<OrderRow> _OrderItems;
        [DataMember]
        public ObservableCollection<OrderRow> OrderItems
        {
            get { return _OrderItems; }
            set { _OrderItems = value; }
        }

        private ObservableCollection<Payment> _Payments;
        [DataMember]
        public ObservableCollection<Payment> Payments
        {
            get { return _Payments; }
            set { _Payments = value; }
        }

        private ObservableCollection<ShippingAddress> _ShipmentAddress;
        [DataMember]
        public ObservableCollection<ShippingAddress> ShippingAddress
        {
            get { return _ShipmentAddress; }
            set { _ShipmentAddress = value; }
        }

        private Customer _Customer;
        [DataMember]
        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }
        #endregion
    }
}