using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGIXrmEAIConnectorService
{
    [DataContract]
    public class GetCustomerIdForTravelCardResponse : Response
    {
        [DataMember]
        public List<Detail> Details { get; set; }
    }

    [DataContract]
    public class Detail
    {
        [DataMember]
        public string TravelCardNumber { get; set; }
        [DataMember]
        public Guid CustomerId { get; set; }
        [DataMember]
        public AccountCategoryCode CustomerType { get; set; }
        [DataMember]
        public string TravelCardName { get; set; }
    }
}