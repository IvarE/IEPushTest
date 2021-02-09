using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGICRMPortalService.Models
{
    [DataContract]
    public class CheckCustomerExistResponse : Response
    {
        private AccountCategoryCode? _CustomerType = null;
        [DataMember]
        public AccountCategoryCode? CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        
        //public AccountCategoryCode CustomerType { get; set; }
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
        private bool _CustomerExists;
        [DataMember]
        public bool CustomerExists
        {
            get { return _CustomerExists; }
            set { _CustomerExists = value; }
        }
        
    }
}