using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class FileLink
    {
        [DataMember]
        public string URL { get; set; }

    }
}