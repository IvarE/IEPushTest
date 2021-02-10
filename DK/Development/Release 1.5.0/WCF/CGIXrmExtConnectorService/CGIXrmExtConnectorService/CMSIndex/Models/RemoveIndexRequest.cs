using System.Runtime.Serialization;

namespace CGIXrmExtConnectorService.CMSIndex.Models
{
    [DataContract]
    public class RemoveIndexRequest
    {
        #region Public Properties -------------------------------------------------------------------------------------

        [DataMember]
        public string IntranetIndexId { get; set; }

        [DataMember]
        public string ExternalWebIndexId { get; set; }

        #endregion
    }
}