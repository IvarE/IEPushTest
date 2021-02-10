using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    /// <summary>
    /// This is DTO for CreditOrder incoming request
    /// </summary>
    [DataContract]
    public class CreditRow
    {
        #region Public Properties
        [DataMember]
        public string OrderNumber { get; set; }

        [DataMember]
        public string Sum { get; set; }

        [DataMember]
        public string ProductNumber { get; set; }

        [DataMember]
        public string Quantity { get; set; }

        [DataMember]
        public string AccountId { get; set; }

        [DataMember]
        public string ContactId { get; set; }

        [DataMember]
        public string Reason { get; set; }

        [DataMember]
        public string CreatedBy { get; set; }
        #endregion
    }
}