using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGICRMPortalService.Models
{
    [DataContract]
    public class GetCustomerResponse:Response
    {
        #region Public Properties
        [DataMember]
        public Customer Customer { get; set; }
        #endregion
    }
}