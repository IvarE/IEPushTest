using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class UpdateAutoRgCaseBiffTransactionsRequest
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public Guid CaseID { get; set; }//used to get the parent Incident

        [DataMember]
        public BiffTransaction[] BiffTransactions { get; set; }
        
        #endregion
    }
}


