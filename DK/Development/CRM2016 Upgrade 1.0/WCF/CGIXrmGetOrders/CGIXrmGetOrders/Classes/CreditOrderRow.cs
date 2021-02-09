using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CGIXrmGetOrders.Classes
{
    /// <summary>
    /// This is DTO for GetSavedCreditOrderRows outgoing request
    /// </summary>
    public class CreditOrderRow
    {
        #region Public Properties
        //<cgi_accountid>6C4BE848-31F6-E311-80CE-0050569010AD</cgi_accountid>
        [XmlElement("cgi_accountid")]
        [DataMember]
        public string Accountid { get; set; }

        //<cgi_contactid>6C4BE848-31F6-E311-80CE-0050569010AD</cgi_contactid>
        [XmlElement("cgi_contactid")]
        [DataMember]
        public string Contactid { get; set; }

        //<cgi_date>2014-11-24</cgi_date>
        [XmlElement("cgi_date")]
        [DataMember]
        public string Date { get; set; }

        //<cgi_name>PO5000139</cgi_name>
        [XmlElement("cgi_name")]
        [DataMember]
        public string Name { get; set; }

        //<cgi_ordernumber>PO5000139</cgi_ordernumber>
        [XmlElement("cgi_ordernumber")]
        [DataMember]
        public string OrderNumber { get; set; }

        //<cgi_referencenumber>283856916</cgi_referencenumber>
        [XmlElement("cgi_referencenumber")]
        [DataMember]
        public string ReferenceNumber { get; set; }

        //<cgi_success>true</cgi_success>
        [XmlElement("cgi_success")]
        [DataMember]
        public string Success { get; set; }

        //<cgi_sum>10</cgi_sum>
        [XmlElement("cgi_sum")]
        [DataMember]
        public string Sum { get; set; }

        //<cgi_time>11:52</cgi_time>
        [XmlElement("cgi_time")]
        [DataMember]
        public string Time { get; set; }

        //<cgi_productnumber>AAAAAA</cgi_productnumber>
        [XmlElement("cgi_productnumber")]
        [DataMember]
        public string Productnumber { get; set; }

        // possibly not needed in reply when showing rows, if removed also update stored procs
        //<cgi_reason>För mycket betalt</cgi_reason>
        [XmlElement("cgi_reason")]
        [DataMember]
        public string Reason { get; set; }

        //<cgi_createdby>johnmccain</cgi_createdby>
        [XmlElement("cgi_createdby")]
        [DataMember]
        public string CreatedBy { get; set; }

        #endregion
    }
}