using System.Runtime.Serialization;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService.CMSIndex.Models
{
    [DataContract]
    public class CreateIndexRequest
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public Article KbArticleForIntranet { get; set; }

        [DataMember]
        public Article KbArticleForExternalWeb { get; set; }

        [DataMember]
        public ActionType RequestActionType { get; set; }

        #endregion
    }
}