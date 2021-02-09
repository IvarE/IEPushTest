using System.Runtime.Serialization;

namespace CGIXrmTravelCard.TravelCardClasses.Models
{
    [DataContract]
    public class GetCRMCardDetailsResponse
    {
        #region Public Properties

        [DataMember]
        public Card Card { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }

        #endregion
    }
}