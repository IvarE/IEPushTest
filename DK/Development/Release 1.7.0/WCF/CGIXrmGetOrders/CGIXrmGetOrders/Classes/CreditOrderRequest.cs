using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders.Classes
{
    [DataContract]
    public class CreditOrderRequest
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public ObservableCollection<CreditRow> CreditRows { get; set; }
        
        #endregion
    }
}