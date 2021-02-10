using System.Runtime.Serialization;
namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class CreateIndexRequest
    {
        #region Public Properties
        [DataMember]
        public Article KbArticleForIntranet { get; set; }
        [DataMember]
        public Article KbArticleForExternalWeb { get; set; }
        [DataMember]
        public ActionType RequestActionType { get; set; }
        #endregion
    }
}