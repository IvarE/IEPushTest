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
    public class GetTravelCardTransactionsResponse
    {
        #region Public Properties
        private List<TravelCardTransaction> _travelCardTransactions;
        [DataMember]
        public List<TravelCardTransaction> TravelCardTransactions
        {
            get { return _travelCardTransactions; }
            set { _travelCardTransactions = value; }
        }

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