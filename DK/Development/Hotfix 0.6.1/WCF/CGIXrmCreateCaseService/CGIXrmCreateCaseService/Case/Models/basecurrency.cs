using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService
{
    public class basecurrency
    {

        //org.basecurrencyid
        private string _baseCurrencyId;
        [XmlElement("basecurrencyid")]
        public string BaseCurrencyId
        {
            get { return _baseCurrencyId; }
            set { _baseCurrencyId = value; }
        }

	    //org.basecurrencyidname
        private string _baseCurrencyIdName;
        [XmlElement("basecurrencyidname")]
        public string BaseCurrencyIdName
        {
            get { return _baseCurrencyIdName; }
            set { _baseCurrencyIdName = value; }
        }

    }
}