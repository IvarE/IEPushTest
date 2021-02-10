using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using System.ServiceModel;

namespace CGIXrmTravelCard
{
    [DataContract]
    public class GetCRMCardDetailsResponse
    {
        private Card _card;
        [DataMember]
        public Card Card
        {
            get { return _card; }
            set { _card = value; }
        }

        private string _errorMessage;
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }
}