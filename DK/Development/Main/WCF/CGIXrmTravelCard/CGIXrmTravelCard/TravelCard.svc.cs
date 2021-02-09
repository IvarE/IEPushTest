using System;
using System.Net;
using CGIXrmTravelCard.TravelCardClasses;
using CGIXrmTravelCard.TravelCardClasses.Models;

namespace CGIXrmTravelCard
{
    public class TravelCard : ITravelCard
    {
        #region Public Methods
        public RechargeCardResponse RechargeCard(string cardnumber)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.RechargeCard(cardnumber);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetOutstandingChargesResponse GetGetOutstandingCharges(string cardnumber)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetGetOutstandingCharges(cardnumber);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetCardDetailsResponse GetCardDetails(string cardnumber)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetCardDetails(cardnumber);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetCRMCardDetailsResponse GetCardFromCRMExtended(string cardnumber)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetCardFromCRMExtended(cardnumber);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetCRMCardDetailsResponse GetCardFromCRM(string cardnumber)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetCardFromCRM(cardnumber);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetCardTransactionsResponse GetCardTransactions(string cardnumber, string maxtrasactions, string datefrom, string dateto)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetCardTransactions(cardnumber, maxtrasactions, datefrom, dateto);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetZoneNamesResponse GetZoneNames()
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetZoneNames();
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        public GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid)
        {
            try
            {
                TravelCardManager man = new TravelCardManager();
                return man.GetTravelCardTransactions(travelcardtransactionid);
            }
            catch (Exception ex)
            {
                throw new WebException(ex.Message);
            }
        }

        #endregion
    }
}
