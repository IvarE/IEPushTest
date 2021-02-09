using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetTravelCardTransactionsResponse
    {
        #region Public Properties

        [DataMember]
        public List<TravelCardTransaction> TravelCardTransactions { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}