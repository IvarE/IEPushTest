using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class UpdateAutoRgCaseResponse
    {
        #region Public Properties
        [DataMember]
        public Guid RefundID { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
        #endregion
    }
}