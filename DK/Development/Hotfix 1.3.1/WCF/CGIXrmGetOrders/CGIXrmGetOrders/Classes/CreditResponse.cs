using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class CreditResponse
    {
        private DateTime _Date;
        [DataMember]
        public DateTime Date
        {
            get { return _Date; }
            set { _Date = value; }
        }

        private string _Message;
        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }

        private string _OrderNumber;
        [DataMember]
        public string OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }

        private string _ReferenceNumber;
        [DataMember]
        public string ReferenceNumber
        {
            get { return _ReferenceNumber; }
            set { _ReferenceNumber = value; }
        }

        private bool _Success;
        [DataMember]
        public bool Success
        {
            get { return _Success; }
            set { _Success = value; }
        }

        private decimal _Sum;
        [DataMember]
        public decimal Sum
        {
            get { return _Sum; }
            set { _Sum = value; }
        }
    }
}