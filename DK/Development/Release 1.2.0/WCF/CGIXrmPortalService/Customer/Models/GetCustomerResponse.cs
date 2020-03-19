using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGICRMPortalService.Models
{
    [DataContract]
    public class GetCustomerResponse:Response
    {
        [DataMember]
        public Customer Customer { get; set; }
    }
}