using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGICRMPortalService.Models
{
    [DataContract]
    public class Response
    {
        private ProcessingStatus _Status;
        [DataMember]
        public ProcessingStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }
        private string _Message;
        [DataMember]
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }
}