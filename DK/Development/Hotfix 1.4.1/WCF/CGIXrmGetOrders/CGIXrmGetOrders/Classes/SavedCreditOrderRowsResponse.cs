using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using System.ServiceModel;

namespace CGIXrmGetOrders
{
    [DataContract]
    public class SavedCreditOrderRowsResponse
    {
        #region Public Properties
        private List<CreditOrderRow> _orderList;
        [DataMember]
        public List<CreditOrderRow> OrderList
        {
            get { return _orderList; }
            set { _orderList = value; }
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