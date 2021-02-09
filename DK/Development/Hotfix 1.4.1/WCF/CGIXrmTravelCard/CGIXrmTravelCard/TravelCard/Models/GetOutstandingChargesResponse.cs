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
    public class GetOutstandingChargesResponse
    {
        #region Public Properties
        /// <summary>
        /// OutstandingChargesResponse från eHandel
        /// </summary>
        private string _outstandingCharges;
        [DataMember]
        public string OutstandingCharges
        {
            get { return _outstandingCharges; }
            set { _outstandingCharges = value; }
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