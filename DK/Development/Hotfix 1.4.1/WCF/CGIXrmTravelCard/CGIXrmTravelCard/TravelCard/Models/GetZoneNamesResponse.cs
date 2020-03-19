using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CGIXrmWin;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using System.ServiceModel;

namespace CGIXrmTravelCard
{
    [DataContract]
    public class GetZoneNamesResponse
    {
        #region Public Properties

        private List<Zone> _zones;
        [DataMember]
        public List<Zone> Zones
        {
            get { return _zones; }
            set { _zones = value; }
        }

        private string _errorMessage;
        [DataMember]
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        #endregion
    }
}