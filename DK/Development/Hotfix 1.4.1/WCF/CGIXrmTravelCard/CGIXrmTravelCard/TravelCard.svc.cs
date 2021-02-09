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
    public class TravelCard : ITravelCard
    {
        #region Public Methods
        public RechargeCardResponse RechargeCard(string cardnumber)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.RechargeCard(cardnumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetOutstandingChargesResponse GetGetOutstandingCharges(string cardnumber)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetGetOutstandingCharges(cardnumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetCardDetailsResponse GetCardDetails(string cardnumber)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetCardDetails(cardnumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

		public GetCRMCardDetailsResponse GetCardFromCRMExtended(string cardnumber) {
		  try {
			TravelCardManager _man = new TravelCardManager();
			return _man.GetCardFromCRMExtended(cardnumber);
		  }
		  catch (Exception ex) {
			throw ex;
		  }
		}

        public GetCRMCardDetailsResponse GetCardFromCRM(string cardnumber)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetCardFromCRM(cardnumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetCardTransactionsResponse GetCardTransactions(string cardnumber, string maxtrasactions, string datefrom, string dateto)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetCardTransactions(cardnumber, maxtrasactions, datefrom, dateto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetZoneNamesResponse GetZoneNames()
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetZoneNames();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GetTravelCardTransactionsResponse GetTravelCardTransactions(string travelcardtransactionid)
        {
            try
            {
                TravelCardManager _man = new TravelCardManager();
                return _man.GetTravelCardTransactions(travelcardtransactionid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 #endregion
    }
}
