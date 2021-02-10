using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class GetOrderResponse
    {
        #region Public Properties
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public ObservableCollection<OrderHeader> Orders { get; set; }
        #endregion
    }
}