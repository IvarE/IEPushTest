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
    public class GetCardDetailsResponse
    {
        #region Public Properties
        /// <summary>
        /// Carddetails from BIFF
        /// </summary>
        private string _cardDetails;
        [DataMember]
        public string CardDetails
        {
            get { return _cardDetails; }
            set { _cardDetails = value; }
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
        #endregion
    }
}