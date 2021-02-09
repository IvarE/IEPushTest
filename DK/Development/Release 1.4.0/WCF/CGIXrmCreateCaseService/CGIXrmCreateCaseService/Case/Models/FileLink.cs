using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class FileLink
    {
        #region Public properties

        [DataMember]
        public string Url { get; set; }

        #endregion
    }
}