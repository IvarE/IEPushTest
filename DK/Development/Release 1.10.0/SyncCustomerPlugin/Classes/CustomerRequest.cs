
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace SyncCustomerPlugin
{
    [XmlRoot(ElementName = "CustomerId")]
    public class CustomerId
    {
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "SyncFromCrmtoEPiRequestParameters", Namespace = "http://www.skanetrafiken.com/DK/INTSTDK008/SyncCustomerJSONRequest/20150701")]
    public class SyncFromCrmtoEPiRequestParameters
    {
        [XmlElement(ElementName = "CustomerId")]
        public CustomerId CustomerId { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
    }

}