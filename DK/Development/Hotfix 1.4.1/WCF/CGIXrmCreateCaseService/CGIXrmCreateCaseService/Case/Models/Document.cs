using System.Runtime.Serialization;

namespace CGIXrmCreateCaseService.Case.Models
{
    [DataContract]
    public class Document
    {
        #region Public Properties
        [DataMember]
        public string Subject { get; set; }
        
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] DocumentBody { get; set; }

        [DataMember]
        public string NoteText { get; set; }
        #endregion
    }
}