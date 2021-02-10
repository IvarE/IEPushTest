using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class OrderHeader
    {
        #region Public Properties

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public string OrderStatus { get; set; }

        [DataMember]
        public decimal OrderTotal { get; set; }

        [DataMember]
        public decimal OrderTotalVat { get; set; }

        [DataMember]
        public string OrderType { get; set; }

        [DataMember]
        public ObservableCollection<OrderRow> OrderItems { get; set; }

        [DataMember]
        public ObservableCollection<Payment> Payments { get; set; }

        [DataMember]
        public ObservableCollection<ShippingAddress> ShippingAddress { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        #endregion
    }
}