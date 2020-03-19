using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGICRMPortalService.Models
{
    [DataContract]
    public class CreateCustomerResponse:Response
    {

        //private ProcessingStatus _Status;
        //[DataMember]
        //public ProcessingStatus Status
        //{
        //    get { return _Status; }
        //    set { _Status = value; }
        //}

        private Guid _AccountId;
        [DataMember]
        public Guid AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }
        private string _AccountNumber;
        [DataMember]
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
            
        }

        //private string _Message;
        //[DataMember]
        //public string Message
        //{
        //    get { return _Message; }
        //    set { _Message = value; }
            
        //}
    }
}