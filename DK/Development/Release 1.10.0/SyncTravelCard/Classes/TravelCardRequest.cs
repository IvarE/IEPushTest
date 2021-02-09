using System;
using System.Xml.Serialization;
using System.Collections.Generic;
namespace SyncTravelCardPlugin
{
    [XmlRoot(ElementName = "SyncFromCrmtoEPiRequestParameters", Namespace = "http://www.skanetrafiken.com/DK/INTSTDK008/SyncCustomerCardRequestJSON/20150701")]
    public class SyncFromCrmtoEPiRequestParameters
    {
        [XmlElement(ElementName = "CustomerId")]
        public string CustomerId { get; set; }
        [XmlAttribute(AttributeName = "ns", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Ns { get; set; }
    }

}