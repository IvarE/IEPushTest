using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmTravelCard
{
    [DataContract]
    public class Zone
    {

        //<cgi_zoneid>101</cgi_zoneid>
        private string _zoneId;
        [DataMember]
        [XmlElement("cgi_zoneid")]
        public string ZoneId
        {
            get { return _zoneId; }
            set { _zoneId = value; }
        }

        //<cgi_name>Mölle</cgi_name>
        private string _zoneName;
        [DataMember]
        [XmlElement("cgi_name")]
        public string ZoneName
        {
            get { return _zoneName; }
            set { _zoneName = value; }
        }

    }
}