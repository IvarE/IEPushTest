using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class Payment
    {
        private string _Code;
        [DataMember]
        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        private string _Name;
        [DataMember]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private string _ReferenceNumber;
        [DataMember]
        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { _ReferenceNumber = value; }
        }

        private decimal _Sum;
        [DataMember]
        public decimal Sum
        {
            get { return _Sum; }
            set { _Sum = value; }
        }

        private bool _SumSpecified;
        [DataMember]
        public bool SumSpecified
        {
            get { return _SumSpecified; }
            set { _SumSpecified = value; }
        }

        private string _TransactionType;
        [DataMember]
        public string TransactionType
        {
            get { return _TransactionType; }
            set { _TransactionType = value; }
        }

        private string _GiftCardCode;
        [DataMember]
        public string GiftCardCode
        {
            get { return _GiftCardCode; }
            set { _GiftCardCode = value; }
        }

        private string _TransactionId;
        [DataMember]
        public string TransactionId
        {
            get { return _TransactionId; }
            set { _TransactionId = value; }
        }
    }
}