using System.Collections.Generic;
using System.Runtime.Serialization;
using CGIXrmEAIConnectorService.Shared.Models;

namespace CGIXrmEAIConnectorService.AllBinary.Models
{
    [DataContract]
    public class GetCustomerIdForTravelCardResponse : Response
    {
        #region Public Properties
        [DataMember]
        public List<Detail> Details { get; set; }
        #endregion
    }
}