using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    /*
         <Customer>
        <Email>user@user.se</Email>
        <AccountNumber>A273A13D-746B-454E-BC66-143A012EF5E9 (Id i SeKund om kunden är finns eller är inloggad)</AccountNumber>
        <AccountNumber2>ST123456 (Kundnummer i SeKund)</AccountNumber2>
        <IsCompany>false</IsCompany>
        <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
      </Customer>
     */

    [DataContract]
    public class Customer
    {

        private string _Email;
        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _AccountNumber;
        [DataMember]
        public string AccountNumber
        {
            get { return _AccountNumber; }
            set { _AccountNumber = value; }
        }

        private string _AccountNumber2;
        [DataMember]
        public string AccountNumber2
        {
            get { return _AccountNumber2; }
            set { _AccountNumber2 = value; }
        }

        private bool? _IsCompany;
        [DataMember]
        public bool? IsCompany
        {
            get { return _IsCompany; }
            set { _IsCompany = value; }
        }

        // Queried from CRM so far, not from biztalk.
        
        private string name;
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        
        private string _ExtraInfo;
        [DataMember]
        public string ExtraInfo
        {
            get { return _ExtraInfo; }
            set { _ExtraInfo = value; }
        }

    }
}