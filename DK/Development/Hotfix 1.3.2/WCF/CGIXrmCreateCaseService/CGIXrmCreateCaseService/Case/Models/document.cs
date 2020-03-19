using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    [DataContract]
    public class document
    {
        [DataMember]
        public string Subject { get; set; }
        
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] DocumentBody { get; set; }

        [DataMember]
        public string NoteText { get; set; }

    }
}