using System.Runtime.Serialization;
namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class RemoveIndexRequest
    {
        string _IntranetIndexId;
        [DataMember]
        public string IntranetIndexId
        {
            get { return _IntranetIndexId; }
            set { _IntranetIndexId = value; }
        }
        string _ExternalWebIndexId;
        [DataMember]
        public string ExternalWebIndexId
        {
            get { return _ExternalWebIndexId; }
            set { _ExternalWebIndexId = value; }
        }

    }
}