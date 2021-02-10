using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class CreateAutoRGCaseResponse
    {
        [DataMember]
        public string CaseID { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}