using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmGetOrders
{
    /// <summary>
    /// This is DTO for GetSavedCreditOrderRows outgoing request
    /// </summary>
    public class CreditOrderRow
    {
        #region Public Properties
        //<cgi_accountid>6C4BE848-31F6-E311-80CE-0050569010AD</cgi_accountid>
        private string _accountid;
        [XmlElement("cgi_accountid")]
        [DataMember]
        public string Accountid
        {
            get { return _accountid; }
            set { _accountid = value; }
        }

        //<cgi_contactid>6C4BE848-31F6-E311-80CE-0050569010AD</cgi_contactid>
        private string _contactid;
        [XmlElement("cgi_contactid")]
        [DataMember]
        public string Contactid
        {
            get { return _contactid; }
            set { _contactid = value; }
        }
        
        //<cgi_date>2014-11-24</cgi_date>
        private string _date;
        [XmlElement("cgi_date")]
        [DataMember]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        //<cgi_name>PO5000139</cgi_name>
        private string _name;
        [XmlElement("cgi_name")]
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        //<cgi_ordernumber>PO5000139</cgi_ordernumber>
        private string _orderNumber;
        [XmlElement("cgi_ordernumber")]
        [DataMember]
        public string OrderNumber
        {
            get { return _orderNumber; }
            set { _orderNumber = value; }
        }

        //<cgi_referencenumber>283856916</cgi_referencenumber>
        private string _referenceNumber;
        [XmlElement("cgi_referencenumber")]
        [DataMember]
        public string ReferenceNumber
        {
            get { return _referenceNumber; }
            set { _referenceNumber = value; }
        }

        //<cgi_success>true</cgi_success>
        private string _success;
        [XmlElement("cgi_success")]
        [DataMember]
        public string Success
        {
            get { return _success; }
            set { _success = value; }
        }

        //<cgi_sum>10</cgi_sum>
        private string _sum;
        [XmlElement("cgi_sum")]
        [DataMember]
        public string Sum
        {
            get { return _sum; }
            set { _sum = value; }
        }

        //<cgi_time>11:52</cgi_time>
        private string _time;
        [XmlElement("cgi_time")]
        [DataMember]
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        //<cgi_productnumber>AAAAAA</cgi_productnumber>
        private string _productnumber;
        [XmlElement("cgi_productnumber")]
        [DataMember]
        public string Productnumber
        {
            get { return _productnumber; }
            set { _productnumber = value; }
        }

        // possibly not needed in reply when showing rows, if removed also update stored procs
        //<cgi_reason>För mycket betalt</cgi_reason>
        private string _reason;
        [XmlElement("cgi_reason")]
        [DataMember]
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        //<cgi_createdby>johnmccain</cgi_createdby>
        private string _createdBy;
        [XmlElement("cgi_createdby")]
        [DataMember]
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = value; }
        }
        #endregion
    }
}