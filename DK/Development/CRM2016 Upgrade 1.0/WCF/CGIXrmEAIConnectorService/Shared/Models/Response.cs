using System.Runtime.Serialization;

namespace CGIXrmEAIConnectorService.Shared.Models
{
    #region Public Properties
    [DataContract]
    public class Response
    {
        [DataMember]
        public ProcessingStatus Status { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public bool IsNull { get; set; }
    }
    #endregion
}