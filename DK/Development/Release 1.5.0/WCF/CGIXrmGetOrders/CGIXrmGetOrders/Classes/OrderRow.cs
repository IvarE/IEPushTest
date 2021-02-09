using System;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class OrderRow
    {
        #region Public Properties

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public decimal Discount { get; set; }

        [DataMember]
        public bool DiscountSpecified { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public bool PriceSpecified { get; set; }

        [DataMember]
        public Byte Quantity { get; set; }

        [DataMember]
        public bool QuantitySpecified { get; set; }

        #endregion
    }
}