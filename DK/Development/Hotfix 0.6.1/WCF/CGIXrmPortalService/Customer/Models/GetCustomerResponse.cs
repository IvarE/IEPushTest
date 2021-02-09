using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGICRMPortalService.Models
{
    [DataContract]
    public class GetCustomerResponse:Response
    {

        //internal ProcessingStatus _Status;
        //[DataMember]
        //public ProcessingStatus Status
        //{
        //    get { return _Status; }
        //}
        //internal string _Message;
        //[DataMember]
        //public string Message
        //{
        //    get { return _Message; }

        //}
        private Customer _Customer;
        [DataMember]
        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }
        
        
    }
}