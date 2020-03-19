using System.Runtime.Serialization;

namespace CGICRMPortalService.Shared.Models
{
    [DataContract]
    public class Response
    {
        #region Public Properties

        [DataMember]
        public ProcessingStatus Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        #endregion
    }
}