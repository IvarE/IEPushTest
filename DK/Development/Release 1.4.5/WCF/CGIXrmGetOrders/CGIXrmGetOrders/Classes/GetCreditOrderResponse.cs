using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class GetCreditOrderResponse
    {
        #region Public Properties
        [DataMember]
        public ObservableCollection<CreditOrderMessage> CreditOrderMessage { get; set; }
        #endregion
    }
}