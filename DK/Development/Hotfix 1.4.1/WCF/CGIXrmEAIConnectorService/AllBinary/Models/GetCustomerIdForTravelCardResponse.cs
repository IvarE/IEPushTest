using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using CGIXrmEAIConnectorService.AllBinary.Models;


namespace CGIXrmEAIConnectorService
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