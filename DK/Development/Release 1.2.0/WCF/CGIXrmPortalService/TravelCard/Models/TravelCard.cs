using CGICRMPortalService.Models;
using Microsoft.Xrm.Sdk;
using System;
using System.Runtime.Serialization;


namespace CGICRMPortalService
{
    [DataContract]
    public class TravelCard
    {
        //private Guid _TravelCardId;
        //[DataMember]
        //public Guid TravelCardId
        //{
        //    get { return _TravelCardId; }
        //    set { _TravelCardId = value; }
        //}

        private string _CardNumber;
        [DataMember(IsRequired = true)]
        public string CardNumber
        {
            get { return _CardNumber; }
            set { _CardNumber = value; }
        }

        private string _CardName;
        [DataMember(IsRequired = false)]
        public string CardName
        {
            get { return _CardName; }
            set { _CardName = value; }
        }

        private int _PeriodCardTypeId;
        [DataMember]
        public int PeriodCardTypeId
        {
            get { return _PeriodCardTypeId; }
            set { _PeriodCardTypeId = value; }
        }

        private string _PeriodCardTypeTitle;
        [DataMember]
        public string PeriodCardTypeTitle
        {
            get { return _PeriodCardTypeTitle; }
            set { _PeriodCardTypeTitle = value; }
        }

        private string _ValueCardTypeTitle;
        [DataMember]
        public string ValueCardTypeTitle
        {
            get { return _ValueCardTypeTitle; }
            set { _ValueCardTypeTitle = value; }
        }

        private DateTime _PeriodValidFrom;
        [DataMember]
        public DateTime PeriodValidFrom
        {
            get { return _PeriodValidFrom; }
            set { _PeriodValidFrom = value; }
        }


        private DateTime _PeriodValidTo;
        [DataMember]
        public DateTime PeriodValidTo
        {
            get { return _PeriodValidTo; }
            set { _PeriodValidTo = value; }
        }


        //private string _CardType;
        //[DataMember]
        //public string CardType
        //{
        //    get { return _CardType; }
        //    set { _CardType = value; }
        //}
        private AccountCategoryCode _CustomerType;
        [DataMember(IsRequired = true)]
        public AccountCategoryCode CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }

        private Boolean _Blocked;
        [DataMember(IsRequired = false)]
        public Boolean Blocked
        {
            get { return _Blocked; }
            set { _Blocked = value; }
        }

        private Guid _AccountId;
        [DataMember(IsRequired = true)]
        public Guid AccountId
        {
            get { return _AccountId; }
            set { _AccountId = value; }
        }

        DateTime _AutoloadConnectionDate;
        [DataMember]
        public DateTime AutoloadConnectionDate
        {
            get { return _AutoloadConnectionDate; }
            set { _AutoloadConnectionDate = value; }
        }
        DateTime _AutoloadDisconnectionDate;
        [DataMember]
        public DateTime AutoloadDisconnectionDate
        {
            get { return _AutoloadDisconnectionDate; }
            set { _AutoloadDisconnectionDate = value; }
        }


        string _CreditCardMask;
        [DataMember]
        public string CreditCardMask
        {
            get { return _CreditCardMask; }
            set { _CreditCardMask = value; }
        }
        int _FailedAttemptsToChargeMoney;
        [DataMember]
        public int FailedAttemptsToChargeMoney
        {
            get { return _FailedAttemptsToChargeMoney; }
            set { _FailedAttemptsToChargeMoney = value; }
        }
        int _AutoloadStatus;
        [DataMember]
        public int AutoloadStatus
        {
            get { return _AutoloadStatus; }
            set { _AutoloadStatus = value; }
        }
        DateTime _LatestChargeDate;
        [DataMember]
        public DateTime LatestChargeDate
        {
            get { return _LatestChargeDate; }
            set { _LatestChargeDate = value; }
        }
        int _ValueCardTypeId;
        [DataMember]
        public int ValueCardTypeId
        {
            get { return _ValueCardTypeId; }
            set { _ValueCardTypeId = value; }
        }
        string _VerifyId;
        [DataMember]
        public string VerifyId
        {
            get { return _VerifyId; }
            set { _VerifyId = value; }
        }
        decimal _LatestAutoloadAmount;
        [DataMember]
        public decimal LatestAutoloadAmount
        {
            get { return _LatestAutoloadAmount; }
            set { _LatestAutoloadAmount = value; }
        }
        string _Currency;
        [DataMember]
        public string Currency
        {
            get { return _Currency; }
            set { _Currency = value; }
        }
        int _CardCategory;
        [DataMember]
        public int CardCategory
        {
            get { return _CardCategory; }
            set { _CardCategory = value; }
        }
        DateTime _LatestFailedAttempt;
        [DataMember]
        public DateTime LatestFailedAttempt
        {
            get { return _LatestFailedAttempt; }
            set { _LatestFailedAttempt = value; }
        }


    }
}