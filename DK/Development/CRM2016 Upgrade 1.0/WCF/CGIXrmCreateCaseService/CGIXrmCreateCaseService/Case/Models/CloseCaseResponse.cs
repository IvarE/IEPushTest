using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class CloseCaseResponse
    {
        #region Public Properties
        [DataMember]
        public Guid CaseId { get; set; }
 
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
        #endregion
    }
}