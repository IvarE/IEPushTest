using System;
using System.Runtime.Serialization;
using CGIXrmEAIConnectorService.Shared.Models;

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