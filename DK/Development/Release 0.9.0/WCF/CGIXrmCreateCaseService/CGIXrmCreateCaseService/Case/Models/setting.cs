using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    public class setting
    {

        private string _defaultCustomer;
        [XmlElement("defaultcustomeroncase")]
        public string DefaultCustomer
        {
            get { return _defaultCustomer; }
            set { _defaultCustomer = value; }
        }

    }
}