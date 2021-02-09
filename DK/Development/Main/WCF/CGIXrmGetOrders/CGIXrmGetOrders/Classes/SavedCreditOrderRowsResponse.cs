using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class SavedCreditOrderRowsResponse
    {
        #region Public Properties

        [DataMember]
        public List<CreditOrderRow> OrderList { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}