using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;


namespace CGIXrmEAIConnectorService
{
    [DataContract]
    public class GetCustomerDetailsResponse : Response
    {

        private Customer _Customer;
        [DataMember]
        public Customer Customer
        {
            get { return _Customer; }
            set { _Customer = value; }
        }

        private AccountCategoryCode _RequestAccountCategoryCode;
        [DataMember]
        public AccountCategoryCode RequestAccountCategoryCode
        {
            get { return _RequestAccountCategoryCode; }
            set { _RequestAccountCategoryCode = value; }
        }


    }
}