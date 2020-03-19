using System.ServiceModel;
using CGIXrmTravelCard.TravelCardClasses.Models;

namespace CGIXrmTravelCard
{
    [ServiceContract]
    public interface ITravelCard
    {
        #region Public Methods
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
 #endregion
    }
}
