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
    public class RechargeCardResponse
    {
        /// <summary>
        /// OutstandingChargesResponse från eHandel
        /// </summary>
        private string _rechargeCard;
        [DataMember]
        public string RechargeCard
        {
            get { return _rechargeCard; }
            set { _rechargeCard = value; }
        }

        /// <summary>
        /// Errormessage
        /// </summary>
        private string _errorMessage;
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }

    }
}