using System;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class CreditResponse
    {
        #region Public Properties

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public string ReferenceNumber { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        #endregion
    }
}