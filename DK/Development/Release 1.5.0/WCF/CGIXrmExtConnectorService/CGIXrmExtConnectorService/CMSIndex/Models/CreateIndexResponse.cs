using System.Runtime.Serialization;
using CGIXrmExtConnectorService.Shared.Models;

namespace CGIXrmExtConnectorService.CMSIndex.Models
{
    [DataContract]
    public class CreateIndexResponse:Response
    {
        #region Public Properties

        [DataMember]
        public bool IntranetIndexStatus { get; set; }

        [DataMember]
        public string IntranetIndexId { get; set; }

        [DataMember]
        public bool ExternalWebIndexStatus { get; set; }

        [DataMember]
        public string ExternalWebIndexId { get; set; }

        #endregion
    }
}