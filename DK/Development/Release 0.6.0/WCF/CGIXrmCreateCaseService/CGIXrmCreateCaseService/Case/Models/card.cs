using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    public class card
    {
        //travelcard.cgi_travelcardid
        private string _travelCardId;
        [XmlElement("cgi_travelcardid")]
        public string TravelCardId
        {
            get { return _travelCardId; }
            set { _travelCardId = value; }
        }
    }
}