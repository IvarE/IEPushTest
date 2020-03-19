using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class OrderRow
    {
        #region Public Properties
        private string _Code;
        [DataMember]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private decimal _Discount;
        [DataMember]
        public decimal Discount
        {
            get { return _Discount; }
            set { _Discount = value; }
        }
        private bool _DiscountSpecified;
        [DataMember]
        public bool DiscountSpecified
        {
            get { return _DiscountSpecified; }
            set { _DiscountSpecified = value; }
        }
        private string _Name;
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        private decimal _Price;
        [DataMember]
        public decimal Price
        {
            get { return _Price; }
            set { _Price = value; }
        }
        private bool _PriceSpecified;
        [DataMember]
        public bool PriceSpecified
        {
            get { return _PriceSpecified; }
            set { _PriceSpecified = value; }
        }

        private Byte _Quantity;
        [DataMember]
        public Byte Quantity
        {
            get { return _Quantity; }
            set { _Quantity = value; }
        }

        private bool _QuantitySpecified;
        [DataMember]
        public bool QuantitySpecified
        {
            get { return _QuantitySpecified; }
            set { _QuantitySpecified = value; }
        }
        #endregion
    }
}