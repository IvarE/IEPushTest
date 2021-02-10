using System;
using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class UpdateAutoRgCaseResponse : UpdateAutoRgResponse
    {
        #region Public Properties
        [DataMember]
        public Guid RefundID { get; set; }
        #endregion
    }
}