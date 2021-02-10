using System;
using Microsoft.Xrm.Sdk;
using CGIXrmWin;

namespace CGICRMPortalService
{
    [XrmEntity("cgi_travelcard")]
    public class CrmTravelCard
    {

        private Guid _TravelCardId;
        [XrmPrimaryKey]
        [Xrm("cgi_travelcardid")]
        public Guid TravelCardId
        {
            get { return _TravelCardId; }
            set { _TravelCardId = value; }
        }

        private string _CardNumber;
        [Xrm("cgi_travelcardnumber")]
        public string CardNumber
        {
            get { return _CardNumber; }
            set { _CardNumber = value; }
        }

        private string _CardName;
        [Xrm("cgi_travelcardname")]
        public string CardName
        {
            get { return _CardName; }
            set { _CardName = value; }
        } 

        private string _PeriodCardTypeTitle;
        [Xrm("cgi_periodic_card_type")]
        public string PeriodCardTypeTitle
        {
            get { return _PeriodCardTypeTitle; }
            set { _PeriodCardTypeTitle = value; }
        }

        private DateTime _PeriodValidFrom;
        [Xrm("cgi_validfrom")]
        public DateTime PeriodValidFrom
        {
            get { return _PeriodValidFrom; }
            set { _PeriodValidFrom = value; }
        }

        private DateTime _PeriodValidTo;
        [Xrm("cgi_validto")]
        public DateTime PeriodValidTo
        {
            get { return _PeriodValidTo; }
            set { _PeriodValidTo = value; }
        }

        private string _ValueCardTypeTitle;
        [Xrm("cgi_value_card_type")]
        public string ValueCardTypeTitle
        {
            get { return _ValueCardTypeTitle; }
            set { _ValueCardTypeTitle = value; }
        }

        //private EntityReference _CardType;
        //[Xrm("cgi_cardtypeid")]
        //public EntityReference CardType
        //{
        //    get { return _CardType; }
        //    set { _CardType = value; }
        //}

        //private string _CardTypeName;
        //[Xrm("cgi_cardtypeid", DecodePart = XrmDecode.Name)]
        //public string CardTypeName
        //{
        //    get { return _CardTypeName; }
        //    set { _CardTypeName = value; }
        //}

        //private Guid _CardTypeValue;
        //[Xrm("cgi_cardtypeid", DecodePart = XrmDecode.Value)]
        //public Guid CardTypeValue
        //{
        //    get { return _CardTypeValue; }
        //    set { _CardTypeValue = value; }
        //}

        private EntityReference _Contact;
        [Xrm("cgi_contactid")]
        public EntityReference Contact
        {
            get { return _Contact; }
            set { _Contact = value; }
        }

        private EntityReference _Account;
        [Xrm("cgi_accountid")]
        public EntityReference Account
        {
            get { return _Account; }
            set { _Account = value; }
        }

        private Boolean _Blocked;
        [Xrm("cgi_blocked")]
        public Boolean Blocked
        {
            get { return _Blocked; }
            set { _Blocked = value; }
        }

        private string _Status;
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        DateTime _AutoloadConnectionDate;
        [Xrm("cgi_autoloadconnectiondate")]
        public DateTime AutoloadConnectionDate
        {
            get { return _AutoloadConnectionDate; }
            set { _AutoloadConnectionDate = value; }
        }
        DateTime _AutoloadDisconnectionDate;
        [Xrm("cgi_autoloaddisconnectiondate")]
        public DateTime AutoloadDisconnectionDate
        {
            get { return _AutoloadDisconnectionDate; }
            set { _AutoloadDisconnectionDate = value; }
        }


        string _CreditCardMask;
        [Xrm("cgi_creditcardmask")]
        public string CreditCardMask
        {
            get { return _CreditCardMask; }
            set { _CreditCardMask = value; }
        }
        int _FailedAttemptsToChargeMoney;
        [Xrm("cgi_failedattemptstochargemoney")]
        public int FailedAttemptsToChargeMoney
        {
            get { return _FailedAttemptsToChargeMoney; }
            set { _FailedAttemptsToChargeMoney = value; }
        }
        int _AutoloadStatus;
        [Xrm("cgi_autoloadstatus")]
        public int AutoloadStatus
        {
            get { return _AutoloadStatus; }
            set { _AutoloadStatus = value; }
        }
        DateTime _LatestChargeDate;
        [Xrm("cgi_latestchargedate")]
        public DateTime LatestChargeDate
        {
            get { return _LatestChargeDate; }
            set { _LatestChargeDate = value; }
        }
        int _ValueCardTypeId;
        [Xrm("cgi_valuecardtypeid")]
        public int ValueCardTypeId
        {
            get { return _ValueCardTypeId; }
            set { _ValueCardTypeId = value; }
        }
        string _VerifyId;
        [Xrm("cgi_verifyid")]
        public string VerifyId
        {
            get { return _VerifyId; }
            set { _VerifyId = value; }
        }
        Money _LatestAutoloadAmount;
        [Xrm("cgi_latestautoloadamount")]
        public Money LatestAutoloadAmount
        {
            get { return _LatestAutoloadAmount; }
            set { _LatestAutoloadAmount = value; }
        }
        string _Currency;
        [Xrm("cgi_currency")]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }
        int _CardCategory;
        [Xrm("cgi_cardcategory")]
        public int CardCategory
        {
            get { return _CardCategory; }
            set { _CardCategory = value; }
        }
        DateTime _LatestFailedAttempt;
        [Xrm("cgi_latestfailedattempt")]
        public DateTime LatestFailedAttempt
        {
            get { return _LatestFailedAttempt; }
            set { _LatestFailedAttempt = value; }
        }
        private int _PeriodCardTypeId;
        [Xrm("cgi_periodcardtypeid")]
        public int PeriodCardTypeId
        {
            get { return _PeriodCardTypeId; }
            set { _PeriodCardTypeId = value; }
        }

    }
}
