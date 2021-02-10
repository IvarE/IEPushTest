using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class UpdateAutoRgCaseRequest
    {
        #region Public Properties
        [DataMember]
        public Guid CaseID { get; set; }//used to get the parent Incident

        //if true the refund transaction will be closed as Approved - Transaction Pending and the transaction will be executed by workflow
        //if false the the transaction will be closed as Declined and no further actions will be taken
        [DataMember]
        public bool Approved { get; set; }

        [DataMember]
        public int RefundType { get; set; } //used to get the RgolSetting which holds the ReImBursementFormId used when creating the refund

        [DataMember]
        public decimal Value { get; set; } //used as amount of the refund

        [DataMember]
        public string Currency { get; set; } //not used


        [DataMember]
        public string InternalMessage { get; set; }//saved as cgi_comments on the refund

        [DataMember]
        public string CustomerMessage { get; set; }//saved as cgi_CustomerMessage on the refund



        [DataMember]
        public decimal CompensationClaimFromRGOL { get; set; }
        
        [DataMember]
        public string RGOLCaseLog { get; set; }
        
        [DataMember]
        public bool ReqReceipt { get; set; }
        
        [DataMember]
        public bool IsCompleted { get; set; }
        #endregion
    }
}


