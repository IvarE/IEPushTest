using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]   
    public enum CustomerType
    { 
        [EnumMember]
        Private = 0,
        [EnumMember]
        Organisation = 1
    }





}