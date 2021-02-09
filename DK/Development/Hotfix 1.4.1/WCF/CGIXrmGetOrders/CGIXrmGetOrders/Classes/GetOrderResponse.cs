using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
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