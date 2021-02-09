using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetZoneNamesResponse
    {
        #region Public Properties

        [DataMember]
        public List<Zone> Zones { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}