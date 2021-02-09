using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmEAIConnectorService
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

        private bool _IsNull;
        [DataMember]
        public bool IsNull
        {
            get { return _IsNull; }
            set { _IsNull = value; }
        }

    }
}