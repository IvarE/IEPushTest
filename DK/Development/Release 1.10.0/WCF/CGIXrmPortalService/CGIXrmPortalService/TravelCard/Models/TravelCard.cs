using System;
using System.Runtime.Serialization;
using CGICRMPortalService.Shared.Models;

namespace CGICRMPortalService.TravelCard.Models
{
    [DataContract]
    public class TravelCard
    {
        #region Public Properties

        [DataMember(IsRequired = true)]
        public string CardNumber { get; set; }

        [DataMember(IsRequired = false)]
        public string CardName { get; set; }

        [DataMember]
        public int PeriodCardTypeId { get; set; }

        [DataMember]
        public string PeriodCardTypeTitle { get; set; }

        [DataMember]
        public string ValueCardTypeTitle { get; set; }

        [DataMember]
        public DateTime PeriodValidFrom { get; set; }

        [DataMember]
        public DateTime PeriodValidTo { get; set; }

        [DataMember(IsRequired = true)]
        public AccountCategoryCode CustomerType { get; set; }

        [DataMember(IsRequired = false)]
        public Boolean Blocked { get; set; }

        [DataMember(IsRequired = true)]
        public Guid AccountId { get; set; }

        [DataMember]
        public DateTime AutoloadConnectionDate { get; set; }

        [DataMember]
        public DateTime AutoloadDisconnectionDate { get; set; }

        [DataMember]
        public string CreditCardMask { get; set; }

        [DataMember]
        public int FailedAttemptsToChargeMoney { get; set; }

        [DataMember]
        public int AutoloadStatus { get; set; }

        [DataMember]
        public DateTime LatestChargeDate { get; set; }

        [DataMember]
        public int ValueCardTypeId { get; set; }

        [DataMember]
        public string VerifyId { get; set; }

        [DataMember]
        public decimal LatestAutoloadAmount { get; set; }

        [DataMember]
        public string Currency { get; set; }

        [DataMember]
        public int CardCategory { get; set; }

        [DataMember]
        public DateTime LatestFailedAttempt { get; set; }

        #endregion
    }
}