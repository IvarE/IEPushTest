using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetCardTransactionsResponse
    {
        #region Public Properties

        [DataMember]
        public string Transactions { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}