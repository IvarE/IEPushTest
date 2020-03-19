using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CGIXrmGetOrders
{
    /*
      <Address>Gata 1B</Address>
        <City>Stad</City>
        <Co>C/O</Co>
        <CompanyName>Företagsnamn (om beställaren är en företagskund)</CompanyName>
        <Country>Land</Country>
        <CellPhoneNumber>0701234567</CellPhoneNumber>
        <Email>E-post</Email>
        <FirstName>Förnamn</FirstName>
        <LastName>Efternamn</LastName>
        <PostalCode>43125</PostalCode>
        <ExtraInfo>Här kan extra information läggas framöver</ExtraInfo>
     */

    [DataContract]
    public class ShippingAddress
    {
        private string _Address;
        [DataMember]
        public string Address
        {
            get { return _Address; }
            set { _Address = value; }
        }

        private string _City;
        [DataMember]
        public string City
        {
            get { return _City; }
            set { _City = value; }
        }

        private string _Co;
        [DataMember]
        public string Co
        {
            get { return _Co; }
            set { _Co = value; }
        }

        private string _CompanyName;
        [DataMember]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }

        private string _Country;
        [DataMember]
        public string Country
        {
            get { return _Country; }
            set { _Country = value; }
        }

        private string _CellPhoneNumber;
        [DataMember]
        public string CellPhoneNumber
        {
            get { return _CellPhoneNumber; }
            set { _CellPhoneNumber = value; }
        }

        private string _Email;
        [DataMember]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }

        private string _FirstName;
        [DataMember]
        public string FirstName
        {
            get { return _FirstName; }
            set { _FirstName = value; }
        }

        private string _LastName;
        [DataMember]
        public string LastName
        {
            get { return _LastName; }
            set { _LastName = value; }
        }

        private string _PostalCode;
        [DataMember]
        public string PostalCode
        {
            get { return _PostalCode; }
            set { _PostalCode = value; }
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