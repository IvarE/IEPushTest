using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class Payment
    {
        #region Public Properties

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string ReferenceNumber { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public bool SumSpecified { get; set; }

        [DataMember]
        public string TransactionType { get; set; }

        [DataMember]
        public string GiftCardCode { get; set; }

        [DataMember]
        public string TransactionId { get; set; }

        #endregion
    }
}