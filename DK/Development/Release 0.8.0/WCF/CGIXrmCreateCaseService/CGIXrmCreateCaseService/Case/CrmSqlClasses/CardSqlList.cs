using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace CGIXrmCreateCaseService
{
    [XmlRoot("cards")]
    public class CardSqlList
    {
        [XmlElement("card")]
        public List<card> Cards { get; set; }
    }
}