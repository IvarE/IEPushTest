using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

using System.Collections.ObjectModel;

namespace CGIXrmTravelCard
{
    [ServiceContract]
    public interface ITravelCard
    {

        [OperationContract]
        GetCardDetailsResponse GetCardDetails(string cardnumber);

        [OperationContract]
        GetCRMCardDetailsResponse GetCardFromCRM(string cardnumber);

		[OperationContract]
		GetCRMCardDetailsResponse GetCardFromCRMExtended(string cardnumber);

        [OperationContract]
        GetCardTransactionsResponse GetCardTransactions(string cardnumber, string maxtrasactions, string datefrom, string dateto);

        [OperationContract]
        GetZoneNamesResponse GetZoneNames();

        [OperationContract]
        GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid);

        [OperationContract]
        GetOutstandingChargesResponse GetGetOutstandingCharges(string cardnumber);

        [OperationContract]
        RechargeCardResponse RechargeCard(string cardnumber);

    }

}
