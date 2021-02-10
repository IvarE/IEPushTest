using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CGIXrmExtConnectorService
{
    [DataContract]
    public class CreateIndexResponse:Response
    {
        #region Public Properties
        bool _IntranetIndexStatus;
        [DataMember]
        public bool IntranetIndexStatus
        {
            get { return _IntranetIndexStatus; }
            set { _IntranetIndexStatus = value; }
        }

        string _IntranetIndexId;
        [DataMember]
        public string IntranetIndexId
        {
            get { return _IntranetIndexId; }
            set { _IntranetIndexId = value; }
        }
        
        bool _ExternalWebIndexStatus;
        [DataMember]
        public bool ExternalWebIndexStatus
        {
            get { return _ExternalWebIndexStatus; }
            set { _ExternalWebIndexStatus = value; }
        }

        string _ExternalWebIndexId;
        [DataMember]
        public string ExternalWebIndexId
        {
            get { return _ExternalWebIndexId; }
            set { _ExternalWebIndexId = value; }
        }
        #endregion
    }
}