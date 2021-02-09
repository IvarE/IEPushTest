using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class CreditOrderRequest
    {
        [DataMember]
        public ObservableCollection<CreditRow> CreditRows { get; set; }
    }
}