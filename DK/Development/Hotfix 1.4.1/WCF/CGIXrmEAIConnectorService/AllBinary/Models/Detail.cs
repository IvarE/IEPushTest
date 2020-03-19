using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmEAIConnectorService.AllBinary.Models
{
    [DataContract]
    public class Detail
    {
        #region Public Properties
        [DataMember]
        public string TravelCardNumber { get; set; }
        [DataMember]
        public Guid CustomerId { get; set; }
        [DataMember]
        public AccountCategoryCode CustomerType { get; set; }
        [DataMember]
        public string TravelCardName { get; set; }
        #endregion
    }
}